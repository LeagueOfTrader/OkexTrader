using OkexTrader.Common;
using OkexTrader.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.StockTrade
{
    class StockTradeMgr : Singleton<StockTradeMgr>
    {
        List<StockTradeEntity> m_stockEntityList = new List<StockTradeEntity>();

        public void trade(OkexCoinType comm, OkexCoinType curr,
            double price, double volume, OkexStockTradeType type, 
            StockTradeEntity.StockTradeEventHandler callback = null, long queryInterval = 1000)
        {
            StockTradeEntity entity = new StockTradeEntity(comm, curr, queryInterval);
            entity.setTradeEventHandler(callback);
            OkexStockTrader.Instance.tradeAsync(comm, curr, type, price, volume, entity.onAsyncCallback);
            m_stockEntityList.Add(entity);
        }

        public void update()
        {
            updateStockEntities();
        }

        private void updateStockEntities()
        {
            List<int> eraseList = new List<int>();
            for (int i = 0; i < m_stockEntityList.Count; i++)
            {
                TradeQueryResult status = m_stockEntityList[i].getStatus();
                if (status == TradeQueryResult.TQR_Finished
                    || status == TradeQueryResult.TQR_Failed)
                {
                    eraseList.Add(i);
                }
            }
            if (eraseList.Count > 0)
            {
                for (int i = eraseList.Count - 1; i >= 0; i--)
                {
                    m_stockEntityList.RemoveAt(eraseList[i]);
                }
            }
        }
    }
}
