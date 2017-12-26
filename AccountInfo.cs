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

        //public AccountInfo()
        //{

        //}
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

            //string today = DateUtil.getDateToday();

            List<OkexContractInfo> info = new List<OkexContractInfo>();
            foreach(var ci in allContracts)
            {
                //long contractID = ci.contract_id;
                //info.Add(ci);
                OkexFutureContractType contractType = parseContractType(ci.contract_type);
                if(contractType == fc)
                {
                    info.Add(ci);
                }
            }

            return info;
        }

        private OkexFutureContractType parseContractType(string str)
        {
            if(str == "this_week")
            {
                return OkexFutureContractType.FC_ThisWeek;
            }
            else if(str == "next_week")
            {
                return OkexFutureContractType.FC_NextWeek;
            }
            else //if(str == "quarter")
            {
                return OkexFutureContractType.FC_Quarter;
            }
        }
    }
}
