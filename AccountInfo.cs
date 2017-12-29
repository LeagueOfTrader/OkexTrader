using OkexTrader.Common;
using OkexTrader.Trade;
using OkexTrader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader
{
    class AccountInfo : Singleton<AccountInfo>
    {
        private Dictionary<OkexFutureInstrumentType, OkexAccountInfo> m_accountInfo = null;
        private bool m_inited = false;

        public bool inited
        {
            get { return m_inited; }
        }

        public void init()
        {
            m_inited = OkexFutureTrader.Instance.getUserInfo(out m_accountInfo);
        }

        public List<OkexContractInfo> getContracts(OkexFutureInstrumentType fi)
        {
            OkexAccountInfo info;
            if(m_accountInfo.TryGetValue(fi, out info))
            {
                return info.contractsInfo;
            }

            return null;
        }

        public List<OkexContractInfo> getContractsByType(OkexFutureInstrumentType fi, OkexFutureContractType fc)
        {
            List<OkexContractInfo> allContracts = getContracts(fi);
            if(allContracts == null)
            {
                return null;
            }

            List<OkexContractInfo> info = new List<OkexContractInfo>();
            foreach(var ci in allContracts)
            {
                OkexFutureContractType contractType = OkexDefValueConvert.parseContractType(ci.contract_type);
                if(contractType == fc)
                {
                    info.Add(ci);
                }
            }

            return info;
        }
    }
}
