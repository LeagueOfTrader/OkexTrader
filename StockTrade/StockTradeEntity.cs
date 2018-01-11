using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OkexTrader.Common;
using OkexTrader.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.StockTrade
{
    class StockTradeEntity : TradeEntity
    {
        public delegate void StockTradeEventHandler(long orderID, TradeQueryResult result, OkexStockOrderBriefInfo info);
        private event StockTradeEventHandler m_stockTradeEventHandler;

        private OkexCoinType m_commodity;
        private OkexCoinType m_currency;

        public StockTradeEntity(OkexCoinType comm, OkexCoinType curr, long queryInterval = 1000) : base(queryInterval)
        {
            m_commodity = comm;
            m_currency = curr;
        }

        public void setTradeEventHandler(StockTradeEventHandler handler)
        {
            m_stockTradeEventHandler += handler;
        }

        public void onTradeEvent(long orderID, TradeQueryResult result, OkexStockOrderBriefInfo info)
        {
            if (m_stockTradeEventHandler != null)
            {
                m_stockTradeEventHandler(orderID, result, info);
            }
        }

        protected override void onTradeOrdered(String str)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool ret = (bool)jo["result"];
            if (!ret)
            {
                onTradeEvent(m_orderID, TradeQueryResult.TQR_Failed, null);
                return;
            }

            m_orderID = (long)jo["order_id"];
            start();
        }

        protected override void query()
        {
            lock (m_lock)
            {
                if (!m_valid)
                {
                    return;
                }
                if (m_orderID == 0)
                {
                    onTradeEvent(m_orderID, TradeQueryResult.TQR_Timeout, null);
                    return;
                }
                OkexStockOrderBriefInfo info;
                if (m_resultTimer != null)
                {
                    m_resultTimer.Start();
                }
                bool ret = OkexStockTrader.Instance.getOrderInfoByID(m_commodity, m_currency, m_orderID, out info);
                if (ret)
                {
                    if (m_resultTimer != null)
                    {
                        m_resultTimer.Stop();
                    }
                    TradeQueryResult tqr = getResultType(info.status);
                    onTradeEvent(m_orderID, tqr, info);
                    if (tqr == TradeQueryResult.TQR_Finished)
                    {
                        stop();
                    }
                }
            }
        }

        protected override void timeout()
        {
            onTradeEvent(m_orderID, TradeQueryResult.TQR_Timeout, null);
        }
    }
}
