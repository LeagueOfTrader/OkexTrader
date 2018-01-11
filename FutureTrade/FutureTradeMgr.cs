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
    class FutureTradeMgr : Singleton<FutureTradeMgr>
    {
        List<FutureTradeEntity> m_entityList = new List<FutureTradeEntity>();
        List<FutureDevolveEntity> m_devolveList = new List<FutureDevolveEntity>();

        public void trade(OkexFutureInstrumentType instrument, OkexFutureContractType contract,
            double price, long volume, OkexContractTradeType type, uint leverRate = 10, 
            FutureTradeEntity.TradeEventHandler callback = null, long queryInterval = 1000)
        {
            FutureTradeEntity entity = new FutureTradeEntity(instrument, contract, queryInterval);
            if (callback != null)
            {
                entity.setTradeEventHandler(callback);
            }
            OkexFutureTrader.Instance.tradeAsync(instrument, contract, price, volume, type, entity.onAsyncCallback, leverRate);
            m_entityList.Add(entity);
        }

        public void update()
        {
            updateFutureEntities();
            updateDevolveEntities();
        }

        private void updateFutureEntities()
        {
            List<int> eraseList = new List<int>();
            for(int i = 0; i < m_entityList.Count; i++)
            {
                TradeQueryResult status = m_entityList[i].getStatus();
                if (status == TradeQueryResult.TQR_Finished 
                    || status == TradeQueryResult.TQR_Failed)
                {
                    eraseList.Add(i);
                }
            }
            if(eraseList.Count > 0)
            {
                for(int i = eraseList.Count - 1; i >= 0; i--)
                {
                    m_entityList.RemoveAt(eraseList[i]);
                }
            }
        }

        private void updateDevolveEntities()
        {
            List<int> eraseList = new List<int>();
            for (int i = 0; i < m_devolveList.Count; i++)
            {
                bool ret = m_devolveList[i].isResponsed();
                if(ret)
                {
                    eraseList.Add(i);
                }
            }
            if (eraseList.Count > 0)
            {
                for (int i = eraseList.Count - 1; i >= 0; i--)
                {
                    m_devolveList.RemoveAt(eraseList[i]);
                }
            }
        }

        public void devolve(OkexFutureInstrumentType instrument, OkexDevolveType devolveDir, double amount, FutureDevolveEntity.DevolveEventHandler callback = null)
        {
            FutureDevolveEntity entity = new FutureDevolveEntity();
            if (callback != null)
            {
                entity.setEventHandler(callback);
            }
            OkexFutureTrader.Instance.devolveAsync(instrument, devolveDir, amount, entity.onAsyncCallback);
            m_devolveList.Add(entity);
        }
    }
}
