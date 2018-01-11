using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.Trade
{
    class OkexDefValueConvert
    {
        // future
        static string[] contractTypeName = { "this_week", "next_week", "quarter" };
        static Dictionary<string, OkexFutureContractType> contractTypeMap = new Dictionary<string, OkexFutureContractType>()
                                                                    { { "this_week", OkexFutureContractType.FC_ThisWeek },
                                                                        { "next_week", OkexFutureContractType.FC_NextWeek },
                                                                        { "quarter", OkexFutureContractType.FC_Quarter }};

        static string[] instrumentQuotationName = { "btc_usd", "ltc_usd", "eth_usd", "etc_usd", "bch_usd" };
        static Dictionary<string, OkexFutureInstrumentType> instrumentQuotationMap = new Dictionary<string, OkexFutureInstrumentType>()
                                                                    { { "btc_usd", OkexFutureInstrumentType.FI_BTC },
                                                                        { "ltc_usd", OkexFutureInstrumentType.FI_LTC },
                                                                        { "eth_usd", OkexFutureInstrumentType.FI_ETH },
                                                                        { "etc_usd", OkexFutureInstrumentType.FI_ETC },
                                                                        { "bch_usd", OkexFutureInstrumentType.FI_BCH }};

        static string[] coinName = { "btc", "ltc", "eth", "etc", "bch", "usdt" };

        static string[] kLineTypeName = { "1min", "3min", "5min", "15min", "30min", "1hour", "2hour", "4hour", "6hour", "12hour", "day", "3day", "week" };

        // stock
        static Dictionary<string, OkexStockTradeType> stockTradeTypeMap = new Dictionary<string, OkexStockTradeType>() {
            {"sell",  OkexStockTradeType.STT_Sell},
            {"buy",  OkexStockTradeType.STT_Buy},
            {"sell_market",  OkexStockTradeType.STT_SellMarketPrice},
            {"buy_market",  OkexStockTradeType.STT_BuyMarketPrice}
        };

        static Dictionary<string, OkexCoinType> coinTypeMap = new Dictionary<string, OkexCoinType>()
        {
            {"btc", OkexCoinType.CT_BTC },
            {"ltc", OkexCoinType.CT_LTC },
            {"etc", OkexCoinType.CT_ETC },
            {"bch", OkexCoinType.CT_BCH },
            {"eth", OkexCoinType.CT_ETH },
            {"usdt", OkexCoinType.CT_USDT }
        };

        // future

        public static OkexFutureInstrumentType parseInstrument(string str)
        {
            return instrumentQuotationMap[str];
        }

        public static string getInstrumentStr(OkexFutureInstrumentType instrument)
        {
            return instrumentQuotationName[(int)instrument];
        }

        public static OkexFutureContractType parseContractType(string str)
        {
            return contractTypeMap[str];
        }

        public static string getContractTypeStr(OkexFutureContractType contract)
        {
            return contractTypeName[(int)contract];
        }

        public static string getCoinName(OkexFutureInstrumentType instrument)
        {
            return coinName[(int)instrument];
        }

        public static string getKLineTypeStr(OkexKLineType kLineType)
        {
            return kLineTypeName[(int)kLineType];
        }

        // stock
        public static string getCoinName(OkexCoinType ct)
        {
            return coinName[(int)ct];
        }

        public static OkexCoinType parseCoinType(string str)
        {
            return coinTypeMap[str];
        }

        public static string getStockTradeTypeStr(OkexStockTradeType tt)
        {
            string str = "";
            switch (tt)
            {
                case OkexStockTradeType.STT_Buy:
                    str = "buy";
                    break;
                case OkexStockTradeType.STT_Sell:
                    str = "sell";
                    break;
                case OkexStockTradeType.STT_BuyMarketPrice:
                    str = "buy_market";
                    break;
                case OkexStockTradeType.STT_SellMarketPrice:
                    str = "sell_market";
                    break;
                default:
                    break;
            }
            return str;
        }

        public static OkexStockTradeType parseStockTradeType(string str)
        {
            return stockTradeTypeMap[str];
        }
    }
}
