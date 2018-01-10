using OkexTrader.Trade;
using OkexTrader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OkexTrader.MarketData
{
    class StockDataUpdater
    {
        private OkexCoinType m_commodityCoin;
        private OkexCoinType m_currencyCoin;
        private Timer m_timer = null;

        public StockDataUpdater(OkexCoinType comm, OkexCoinType curr)
        {
            m_commodityCoin = comm;
            m_currencyCoin = curr;
        }

        public void start()
        {
            m_timer = new Timer(GlobalSetting.marketDataInterval);
            m_timer.Elapsed += new ElapsedEventHandler(update);
            m_timer.Start();

        }

        public void update(object sender, ElapsedEventArgs e)
        {
            queryMarketData();
            queryDepthData();
        }

        public void stop()
        {
            if (m_timer != null)
            {
                m_timer.Stop();
            }
        }

        private void queryMarketData()
        {
            OkexStockMarketData md = OkexStockTrader.Instance.getStockMarketData(m_commodityCoin, m_currencyCoin);
            if (md != null)
            {
                StockDataMgr.Instance.saveMarketData(m_commodityCoin, m_currencyCoin, md);
            }
        }

        private void queryDepthData()
        {
            OkexStockDepthData dd = OkexStockTrader.Instance.getStockDepthData(m_commodityCoin, m_currencyCoin);
            if (dd != null)
            {
                dd.receiveTimestamp = DateUtil.getCurTimestamp();
                StockDataMgr.Instance.saveDepthData(m_commodityCoin, m_currencyCoin, dd);
            }
        }
    }
}
