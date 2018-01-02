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
    class MarketDataUpdater
    {
        private OkexFutureInstrumentType m_instrument;
        private OkexFutureContractType m_contract;
        private Timer m_timer = null;

        public MarketDataUpdater(OkexFutureInstrumentType inst, OkexFutureContractType cntr)
        {
            m_instrument = inst;
            m_contract = cntr;
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
            if(m_timer != null)
            {
                m_timer.Stop();
            }
        }

        private void queryMarketData()
        {
            OkexFutureMarketData md = OkexFutureTrader.Instance.getMarketData(m_instrument, m_contract);
            if (md != null)
            {
                MarketDataMgr.Instance.saveMarketData(m_instrument, m_contract, md);                
            }
        }

        private void queryDepthData()
        {
            OkexFutureDepthData dd = OkexFutureTrader.Instance.getMarketDepthData(m_instrument, m_contract);
            if (dd != null)
            {
                dd.receiveTimestamp = DateUtil.getCurTimestamp();//System.Environment.TickCount;
                MarketDataMgr.Instance.saveDepthData(m_instrument, m_contract, dd);
            }
        }
    }
}
