using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkexTrader.Trade;

namespace OkexTrader
{
    class Program
    {
        static void output(String content)
        {
            Console.WriteLine(content);
            System.Diagnostics.Debug.WriteLine(content);
        }

        static void Main(string[] args)
        {
            OkexFutureTrader ft = new OkexFutureTrader();
            //String str = "----";

            //ft.getMarketData(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter);

            //OkexFutureDepthData dd = ft.getMarketDepthData(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter);

            //ft.getTradesInfo(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter);

            //ft.getFutureIndex(OkexFutureInstrumentType.FI_LTC);

            //ft.getExchangeRate();

            //ft.getEstimatePrice(OkexFutureInstrumentType.FI_LTC);

            //List<OkexKLineData> kl = ft.getKLineData(OkexFutureInstrumentType.FI_BTC, OkexFutureContractType.FC_NextWeek, OkexKLineType.KL_1Min);

            //ft.getHoldAmount(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter);

            //Dictionary<OkexFutureInstrumentType, OkexAccountInfo> info;
            //bool ret = ft.getUserInfo(out info);

            List<OkexPositionInfo> info;
            ft.getFuturePosition(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter, out info);

            return;
            //long orderID = ft.trade(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter, 3000.0, 1, OkexContractTradeType.TT_OpenSell, 20, false);
            //ft.cancel(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter, orderID);

            //AccountInfo ai = new AccountInfo();
            //ai.init();

            //List<OkexContractInfo> info = ai.getContractsByType(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter);
            //if(info != null)
            //{
            //    int n = info.Count;
            //    output(n.ToString());
            //}
        }

        
    }
}
