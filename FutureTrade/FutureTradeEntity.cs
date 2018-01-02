using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OkexTrader.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.FutureTrade
{
    class FutureTradeEntity
    {
        public void trade(OkexFutureInstrumentType instrument, OkexFutureContractType contract,
            double price, long volume, OkexContractTradeType type, uint leverRate = 10)
        {
            OkexFutureTrader.Instance.tradeAsync(instrument, contract, price, volume, type, onTradeResult, leverRate);
        }

        private void onTradeResult(String str)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool ret = (bool)jo["result"];
            if (!ret)
            {
                return;
            }

            long orderID = (long)jo["order_id"];
            onTradeOrdered(orderID);
        }

        protected virtual void onTradeOrdered(long orderID)
        {
            string content = "Trade ordered: " + orderID;
            Console.WriteLine(content);
            System.Diagnostics.Debug.WriteLine(content);
        }
    }
}
