using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OkexTrader.Common;
using OkexTrader.Trade;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OkexTrader.FutureTrade
{
    class FutureTradeTracer
    {
        public string localID;
        public long orderID = 0;
        public Timer resultTimer = null;
        public Timer queryTimer = null;
        public OkexFutureInstrumentType instrument;
        public OkexFutureContractType contract;
        public FutureTradeEntity entity;

        public FutureTradeTracer(string id, FutureTradeEntity fte, long queryInterval = 100)
        {
            localID = id;
            entity = fte;    
            
            queryTimer = new Timer(queryInterval);
            resultTimer = new Timer(10);

            queryTimer.Elapsed += new ElapsedEventHandler(queryTrade);
            resultTimer.Elapsed += new ElapsedEventHandler(onTimeout);
        }

        public void start()
        {
            if(queryTimer != null)
            {
                queryTimer.Start();
            }

            if(resultTimer != null)
            {
                resultTimer.Start();
            }
        }

        public void stop()
        {
            if (queryTimer != null)
            {
                queryTimer.Stop();
            }

            if (resultTimer != null)
            {
                resultTimer.Stop();
            }
        }

        private void queryTrade(object sender, ElapsedEventArgs e)
        {
            if(orderID == 0)
            {
                entity.onTradeEvent(orderID, OkexTradeQueryResultType.TQR_Timeout, null);
                return;
            }

            OkexFutureOrderBriefInfo info;
            if(resultTimer != null)
            {
                resultTimer.Start();
            }
            bool ret = OkexFutureTrader.Instance.getOrderInfoByID(instrument, contract, orderID, out info);
            if (ret)
            {
                if (resultTimer != null)
                {
                    resultTimer.Stop();
                }
                OkexTradeQueryResultType tqr = getResultType(info.status);
                entity.onTradeEvent(orderID, tqr, info);
                if(tqr == OkexTradeQueryResultType.TQR_Finished)
                {
                    stop();
                    FutureTradeMgr.Instance.removeTracer(localID);
                }
            }
        }

        private void onTimeout(object sender, ElapsedEventArgs e)
        {
            entity.onTradeEvent(orderID, OkexTradeQueryResultType.TQR_Timeout, null);
        }

        public void onTradeResult(String str)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool ret = (bool)jo["result"];
            if (!ret)
            {
                return;
            }

            orderID = (long)jo["order_id"];
            entity.onTradeOrdered(orderID);
            FutureTradeMgr.Instance.bindOrderID(orderID, localID);
            //start();
        }

        private static OkexTradeQueryResultType getResultType(OkexOrderStatusType status)
        {
            OkexTradeQueryResultType ret = OkexTradeQueryResultType.TQR_Unfinished;
            if(status == OkexOrderStatusType.OS_AllTraded
               || status == OkexOrderStatusType.OS_Canceled)
            {
                return OkexTradeQueryResultType.TQR_Finished;
            }

            return ret;
        }
    }

    class FutureTradeMgr : Singleton<FutureTradeMgr>
    {
        ConcurrentDictionary<string, FutureTradeTracer> m_tracers = new ConcurrentDictionary<string, FutureTradeTracer>();
        Dictionary<long, string> m_idMap = new Dictionary<long, string>();

        public void trade(FutureTradeEntity entity,
            OkexFutureInstrumentType instrument, OkexFutureContractType contract,
            double price, long volume, OkexContractTradeType type, uint leverRate = 10)
        {
            if (entity == null)
            {
                return;
            }

            long queryInterval = entity.queryInterval;
            string guid = Guid.NewGuid().ToString();
            FutureTradeTracer tracer = new FutureTradeTracer(guid, entity, queryInterval);
            m_tracers.TryAdd(guid, tracer);
            OkexFutureTrader.Instance.tradeAsync(instrument, contract, price, volume, type, tracer.onTradeResult, leverRate);
            tracer.start();
        }

        public void bindOrderID(long orderID, string localID)
        {
            m_idMap[orderID] = localID;
        }

        public void stopTrace(long orderID)
        {
            if (!m_idMap.ContainsKey(orderID))
            {
                return;
            }

            string localID = m_idMap[orderID];
            if (!m_tracers.ContainsKey(localID))
            {
                return;
            }

            m_tracers[localID].stop();

            removeTracer(localID);
        }

        public void removeTracer(string localID)
        {
            if (!m_tracers.ContainsKey(localID))
            {
                return;
            }

            FutureTradeTracer ftt;
            bool ret = m_tracers.TryRemove(localID, out ftt);

            if (ret && ftt != null)
            {
                long id = ftt.orderID;
                if (id != 0)
                {
                    m_idMap.Remove(id);
                }
            }
        }
    }
}
