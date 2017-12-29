using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OkexTrader.Common;
using OkexTrader.Strategy;
using OkexTrader.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader
{
    class StrategyMgr : Singleton<StrategyMgr>
    {
        Dictionary<string, OkexFutureInstrumentType> strInstrumentMap = new Dictionary<string, OkexFutureInstrumentType>()
        {
            { "BTC", OkexFutureInstrumentType.FI_BTC },
            { "LTC", OkexFutureInstrumentType.FI_LTC },
            { "ETH", OkexFutureInstrumentType.FI_ETH },
            { "ETC", OkexFutureInstrumentType.FI_ETC },
            { "BCH", OkexFutureInstrumentType.FI_BCH }
        };
        Dictionary<string, OkexFutureContractType> strContractMap = new Dictionary<string, OkexFutureContractType>()
        {
            { "this_week", OkexFutureContractType.FC_ThisWeek },
            { "next_week", OkexFutureContractType.FC_NextWeek },
            { "quarter", OkexFutureContractType.FC_Quarter }
        };
        //
        Dictionary<string, OkexStrategy> m_strategies = new Dictionary<string, OkexStrategy>();
        const string runCfg = "Data/run.cfg";
        public void init()
        {
            string contents = System.IO.File.ReadAllText(runCfg);
            JArray arr = JArray.Parse(contents);
            foreach(var item in arr)
            {
                string sf = (string)item;
                string filePath = "Data/" + sf + ".cfg";
                runStrategy(filePath);
            }
        }

        public void runStrategy(string strategyFile)
        {
            string str = System.IO.File.ReadAllText(strategyFile);
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            string type = (string)jo["StrategyType"];
            string info = jo["StrategyInfo"].ToString();
            OkexStrategy s = generateStrategy(type, info);
            if(s != null)
            {
                m_strategies.Add(strategyFile, s);
            }
        }

        public OkexStrategy generateStrategy(string type, string info)
        {
            OkexStrategy s = null;

            if(type == "TPByBasis")
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(info);
                string strInst = (string)jo["Instrument"];
                OkexFutureInstrumentType fi = strInstrumentMap[strInst];
                string strSC = (string)jo["SpotContract"];
                OkexFutureContractType sc = strContractMap[strSC];
                string strFC = (string)jo["ForwardContract"];
                OkexFutureContractType fc = strContractMap[strFC];
                string strDir = (string)jo["Direction"];
                OkexFutureTradeDirectionType dir = OkexFutureTradeDirectionType.FTD_Sell;
                if(strDir.Equals("buy", StringComparison.OrdinalIgnoreCase))
                {
                    dir = OkexFutureTradeDirectionType.FTD_Buy;
                }
                else if(strDir.Equals("sell", StringComparison.OrdinalIgnoreCase))
                {
                    dir = OkexFutureTradeDirectionType.FTD_Sell;
                }
                OkexBasisCalcType bc = OkexBasisCalcType.BC_Ratio;
                string strType = (string)jo["Type"];
                if(strType.Equals("ratio", StringComparison.OrdinalIgnoreCase))
                {
                    bc = OkexBasisCalcType.BC_Ratio;
                }
                else if(strType.Equals("diff", StringComparison.OrdinalIgnoreCase))
                {
                    bc = OkexBasisCalcType.BC_Diff;
                }

                s = new OkexTransferPositionByBasis(fi, sc, fc, bc, dir);

                //double boardLot = (double)jo["BoardLot"];
                double basis = (double)jo["Basis"];
                double safe = (double)jo["Safe"];
                double limit = (double)jo["Limit"];
                uint count = (uint)jo["Count"];
                double param = (double)jo["Param"];
                ((OkexTransferPositionByBasis)s).init(basis, safe, limit, count, param);
            }

            return s;
        }

        public void update()
        {
            foreach(var keyVal in m_strategies)
            {
                OkexStrategy s = keyVal.Value;
                if(s != null)
                {
                    s.update();
                }
            }
        }
    }
}
