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
    class MarketDataMgr : Singleton<MarketDataMgr>
    {
        private ConcurrentDictionary<OkexFutureInstrumentType, List<OkexFutureContractType>> m_subscribedContracts =
            new ConcurrentDictionary<OkexFutureInstrumentType, List<OkexFutureContractType>>();

        private ConcurrentDictionary<OkexFutureInstrumentType, ConcurrentDictionary<OkexFutureContractType, OkexFutureDepthData>> m_depthData =
            new ConcurrentDictionary<OkexFutureInstrumentType, ConcurrentDictionary<OkexFutureContractType, OkexFutureDepthData>>();

        private ConcurrentDictionary<OkexFutureInstrumentType, ConcurrentDictionary<OkexFutureContractType, OkexFutureMarketData>> m_marketData =
            new ConcurrentDictionary<OkexFutureInstrumentType, ConcurrentDictionary<OkexFutureContractType, OkexFutureMarketData>>();

        private ConcurrentDictionary<int, MarketDataUpdater> m_dataUpdaters = new ConcurrentDictionary<int, MarketDataUpdater>();

        public void subscribeInstrument(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            if (!m_subscribedContracts.ContainsKey(instrument))
            {
                List<OkexFutureContractType> contractsList = new List<OkexFutureContractType>();
                m_subscribedContracts.TryAdd(instrument, contractsList);
            }

            if (m_subscribedContracts[instrument].Contains(contract))
            {
                return;
            }

            m_subscribedContracts[instrument].Add(contract);

            int id = genTargetID(instrument, contract);
            MarketDataUpdater mdu = new MarketDataUpdater(instrument, contract);
            m_dataUpdaters.TryAdd(id, mdu);
            mdu.start();
        }

        public void unsubscribeInstrument(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            if (!m_subscribedContracts.ContainsKey(instrument))
            {
                return;
            }

            if (!m_subscribedContracts[instrument].Contains(contract))
            {
                return;
            }

            m_subscribedContracts[instrument].Remove(contract);
            int id = genTargetID(instrument, contract);
            if (m_dataUpdaters.ContainsKey(id))
            {
                MarketDataUpdater mdu = m_dataUpdaters[id];
                mdu.stop();
                m_dataUpdaters.TryRemove(id, out mdu);
            }
        }

        //public void update()
        //{
        //    foreach (var keyVal in m_subscribedContracts)
        //    {
        //        OkexFutureInstrumentType inst = keyVal.Key;
        //        foreach(var contract in keyVal.Value)
        //        {
        //            queryMarketData(inst, contract);
        //            queryDepthData(inst, contract);
        //        }
        //    }
        //}

        //private void queryMarketData(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        //{
        //    OkexFutureMarketData md = OkexFutureTrader.Instance.getMarketData(instrument, contract);
        //    if(md != null)
        //    {
        //        //saveMarketData(instrument, contract, ref md);
        //        m_marketData[instrument][contract] = md;
        //    }
        //}

        //private void queryDepthData(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        //{
        //    OkexFutureDepthData dd = OkexFutureTrader.Instance.getMarketDepthData(instrument, contract);
        //    if (dd != null)
        //    {
        //        //saveDepthData(instrument, contract, ref dd);
        //        m_depthData[instrument][contract] = dd;
        //    }
        //}

        public void saveMarketData(OkexFutureInstrumentType instrument, OkexFutureContractType contract, OkexFutureMarketData marketData)
        {
            if (!m_marketData.ContainsKey(instrument))
            {
                ConcurrentDictionary<OkexFutureContractType, OkexFutureMarketData> mdMap = new ConcurrentDictionary<OkexFutureContractType, OkexFutureMarketData>();
                m_marketData.TryAdd(instrument, mdMap);
            }

            m_marketData[instrument][contract] = marketData;
        }

        public void saveDepthData(OkexFutureInstrumentType instrument, OkexFutureContractType contract, OkexFutureDepthData depthData)
        {
            if (!m_depthData.ContainsKey(instrument))
            {
                ConcurrentDictionary<OkexFutureContractType, OkexFutureDepthData> ddMap = new ConcurrentDictionary<OkexFutureContractType, OkexFutureDepthData>();
                m_depthData.TryAdd(instrument, ddMap);
            }

            m_depthData[instrument][contract] = depthData;
        }

        public OkexFutureDepthData getDepthData(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            if (!m_depthData.ContainsKey(instrument))
            {
                return null;
            }

            if (!m_depthData[instrument].ContainsKey(contract))
            {
                return null;
            }

            return m_depthData[instrument][contract];
        }

        public OkexFutureMarketData getMarketData(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            if (!m_marketData.ContainsKey(instrument))
            {
                return null;
            }

            if (!m_marketData[instrument].ContainsKey(contract))
            {
                return null;
            }

            return m_marketData[instrument][contract];
        }

        private int genTargetID(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            return (int)instrument * 10000 + (int)contract;
        }

        public OkexFutureDepthData getDepthDataWithTimeLimit(OkexFutureInstrumentType instrument, OkexFutureContractType contract, long limitMillisec)
        {
            OkexFutureDepthData dd = getDepthData(instrument, contract);
            if(dd == null)
            {
                return null;
            }

            long curTimestamp = DateUtil.getCurTimestamp();
            if(curTimestamp - dd.receiveTimestamp - GlobalSetting.marketDataBias > limitMillisec)
            {
                return null;
            }

            return dd;
        }

        public OkexFutureMarketData getMarketDataWithTimeLimit(OkexFutureInstrumentType instrument, OkexFutureContractType contract, long limitMillisec)
        {
            OkexFutureMarketData md = getMarketData(instrument, contract);
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
