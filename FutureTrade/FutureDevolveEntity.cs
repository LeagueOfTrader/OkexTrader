using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OkexTrader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.FutureTrade
{
    class FutureDevolveEntity : HttpAsyncResponser
    {
        public delegate void DevolveEventHandler(bool success);
        protected DevolveEventHandler m_devolveEventHandler;

        protected override void onResponsed(String str)
        {
            onDevolveResult(str);
        }

        public void setEventHandler(DevolveEventHandler handler)
        {
            m_devolveEventHandler += handler;
        }

        private void onDevolveResult(string str)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool ret = (bool)jo["result"];
            if(m_devolveEventHandler != null)
            {
                m_devolveEventHandler(ret);
            }
        }
    }
}
