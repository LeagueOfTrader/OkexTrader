using OkexTrader.Common;
using OkexTrader.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.MarketData
{
    class MarketDataMgr : Singleton<MarketDataMgr>
    {
        private Dictionary<OkexFutureInstrumentType, List<OkexFutureContractType>> m_subscribedContracts =
            new Dictionary<OkexFutureInstrumentType, List<OkexFutureContractType>>();

        private Dictionary<OkexFutureInstrumentType, Dictionary<OkexFutureContractType, OkexFutureDepthData>> m_depthData =
            new Dictionary<OkexFutureInstrumentType, Dictionary<OkexFutureContractType, OkexFutureDepthData>>();

        private Dictionary<OkexFutureInstrumentType, Dictionary<OkexFutureContractType, OkexFutureMarketData>> m_marketData =
            new Dictionary<OkexFutureInstrumentType, Dictionary<OkexFutureContractType, OkexFutureMarketData>>();

        public void subscribeInstrument(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            if (!m_subscribedContracts.ContainsKey(instrument))
            {
                List<OkexFutureContractType> contractsList = new List<OkexFutureContractType>();
                m_subscribedContracts.Add(instrument, contractsList);
            }

            if (m_subscribedContracts[instrument].Contains(contract))
            {
                return;
            }

            m_subscribedContracts[instrument].Add(contract);
        }

        public void update(float deltaTime)
        {
            foreach (var keyVal in m_subscribedContracts)
            {
                OkexFutureInstrumentType inst = keyVal.Key;
                foreach(var contract in keyVal.Value)
                {
                    queryMarketData(inst, contract);
                    queryDepthData(inst, contract);
                }
            }
        }

        private void queryMarketData(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            OkexFutureMarketData md = OkexFutureTrader.Instance.getMarketData(instrument, contract);
            if(md != null)
            {
                //saveMarketData(instrument, contract, ref md);
                m_marketData[instrument][contract] = md;
            }
        }

        private void queryDepthData(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            OkexFutureDepthData dd = OkexFutureTrader.Instance.getMarketDepthData(instrument, contract);
            if (dd != null)
            {
                //saveDepthData(instrument, contract, ref dd);
                m_depthData[instrument][contract] = dd;
            }
        }

        //private void saveMarketData(OkexFutureInstrumentType instrument, OkexFutureContractType contract, ref OkexFutureMarketData marketData)
        //{
        //    if (!m_marketData.ContainsKey(instrument))
        //    {
        //        Dictionary<OkexFutureContractType, OkexFutureMarketData> mdMap = new Dictionary<OkexFutureContractType, OkexFutureMarketData>();
        //        m_marketData.Add(instrument, mdMap);
        //    }

        //    m_marketData[instrument][contract] = marketData;
        //}

        //private void saveDepthData(OkexFutureInstrumentType instrument, OkexFutureContractType contract, ref OkexFutureDepthData depthData)
        //{
        //    if (!m_marketData.ContainsKey(instrument))
        //    {
        //        Dictionary<OkexFutureContractType, OkexFutureMarketData> mdMap = new Dictionary<OkexFutureContractType, OkexFutureMarketData>();
        //        m_marketData.Add(instrument, mdMap);
        //    }

        //    m_depthData[instrument][contract] = depthData;
        //}

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
    }
}
