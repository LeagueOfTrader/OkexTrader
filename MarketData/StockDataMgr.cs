using OkexTrader.Common;
using OkexTrader.Trade;
using OkexTrader.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.MarketData
{
    class OkexStockDataSubject
    {
        public OkexCoinType commodityCoin;
        public OkexCoinType currencyCoin;

        public OkexStockDataSubject(OkexCoinType commodity, OkexCoinType currency)
        {
            commodityCoin = commodity;
            currencyCoin = currency;
        }
    }

    class StockDataMgr : Singleton<StockDataMgr>
    {
        private ConcurrentDictionary<uint, OkexStockDataSubject> m_stockDataSubjects =
            new ConcurrentDictionary<uint, OkexStockDataSubject>();

        private ConcurrentDictionary<uint, OkexStockDepthData> m_depthData =
            new ConcurrentDictionary<uint, OkexStockDepthData>();

        private ConcurrentDictionary<uint, OkexStockMarketData> m_marketData =
            new ConcurrentDictionary<uint, OkexStockMarketData>();

        private ConcurrentDictionary<uint, StockDataUpdater> m_dataUpdaters = 
            new ConcurrentDictionary<uint, StockDataUpdater>();

        public static uint genID(OkexCoinType commodityCoin, OkexCoinType currencyCoin)
        {
            uint id = 100 * ((uint)commodityCoin + 1) + ((uint)currencyCoin + 1);
            return id;
        }

        public void subscribeInstrument(OkexCoinType commodity, OkexCoinType currency)
        {
            uint id = genID(commodity, currency);
            if (m_stockDataSubjects.ContainsKey(id))
            {
                return;
            }

            m_stockDataSubjects[id] = new OkexStockDataSubject(commodity, currency);

            StockDataUpdater sdu = new StockDataUpdater(commodity, currency);
            m_dataUpdaters.TryAdd(id, sdu);
            sdu.start();
        }

        public void unsubscribeInstrument(OkexCoinType commodity, OkexCoinType currency)
        {
            uint id = genID(commodity, currency);
            if (!m_stockDataSubjects.ContainsKey(id))
            {
                return;
            }

            OkexStockDataSubject sbj;
            m_stockDataSubjects.TryRemove(id, out sbj);
            if (m_dataUpdaters.ContainsKey(id))
            {
                StockDataUpdater sdu = m_dataUpdaters[id];
                sdu.stop();
                m_dataUpdaters.TryRemove(id, out sdu);
            }
        }

        public void saveMarketData(OkexCoinType commodity, OkexCoinType currency, OkexStockMarketData marketData)
        {
            uint id = genID(commodity, currency);

            m_marketData[id] = marketData;
        }

        public void saveDepthData(OkexCoinType commodity, OkexCoinType currency, OkexStockDepthData depthData)
        {
            uint id = genID(commodity, currency);

            m_depthData[id] = depthData;
        }

        public OkexStockDepthData getDepthData(OkexCoinType commodity, OkexCoinType currency)
        {
            uint id = genID(commodity, currency);
            if (!m_depthData.ContainsKey(id))
            {
                return null;
            }

            return m_depthData[id];
        }

        public OkexStockMarketData getMarketData(OkexCoinType commodity, OkexCoinType currency)
        {
            uint id = genID(commodity, currency);
            if (!m_marketData.ContainsKey(id))
            {
                return null;
            }

            return m_marketData[id];
        }

        public OkexStockDepthData getDepthDataWithTimeLimit(OkexCoinType commodity, OkexCoinType currency, long limitMillisec)
        {
            OkexStockDepthData dd = getDepthData(commodity, currency);
            if (dd == null)
            {
                return null;
            }

            long curTimestamp = DateUtil.getCurTimestamp();
            if (curTimestamp - dd.receiveTimestamp - GlobalSetting.marketDataBias > limitMillisec)
            {
                return null;
            }

            return dd;
        }

        public OkexStockMarketData getMarketDataWithTimeLimit(OkexCoinType commodity, OkexCoinType currency, long limitMillisec)
        {
            OkexStockMarketData md = getMarketData(commodity, currency);
            if (md == null)
            {
                return null;
            }

            long curTimestamp = DateUtil.getCurTimestamp();
            if (curTimestamp - md.receiveTimestamp - GlobalSetting.marketDataBias > limitMillisec)
            {
                return null;
            }

            return md;
        }
    }
}
