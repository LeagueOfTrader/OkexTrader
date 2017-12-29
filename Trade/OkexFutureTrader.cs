using com.okcoin.rest.future;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OkexTrader.Common;

namespace OkexTrader.Trade
{
    public enum OkexFutureContractType
    {
        FC_ThisWeek,
        FC_NextWeek,
        FC_Quarter
    }
    
    public enum OkexFutureInstrumentType
    {
        FI_BTC,
        FI_LTC,
        FI_ETH,
        FI_ETC,
        FI_BCH,
    }    

    public enum OkexFutureTradeDirectionType
    {
        FTD_Buy,
        FTD_Sell
    }

    public enum OkexKLineType
    {
        KL_1Min,
        KL_3Min,
        KL_5Min,
        KL_15Min,
        KL_30Min,
        KL_1Hour,
        KL_2Hour,
        KL_4Hour,
        KL_6Hour,
        KL_12Hour,
        KL_Day,
        KL_3Day,
        KL_Week
    }

    enum OkexContractTradeType
    {
        TT_OpenBuy = 1,     // 开多
        TT_OpenSell,        // 开空
        TT_CloseBuy,        // 平多
        TT_CloseSell        // 平空
    }

    enum OkexOrderStatusType
    {
        OS_NotTraded = 0,
        OS_PartiallyTraded = 1,
        OS_AllTraded = 2,
        OS_Canceled = -1,
        OS_CancelProcessing = 4,
        OS_Canceling = 5
    }

    class OkexFutureMarketData
    {
        private long contractID;
        public long contract_id
        {
            get { return contractID; }
            set { contractID = value; }
        }
        private double buyPrice;
        public double buy
        {
            get { return buyPrice; }
            set { buyPrice = value; }
        }
        private double sellPrice;
        public double sell
        {
            get { return sellPrice; }
            set { sellPrice = value; }
        }
        private double highPrice;
        public double high
        {
            get { return highPrice; }
            set { highPrice = value; }
        }
        private double lowPrice;
        public double low
        {
            get { return lowPrice; }
            set { lowPrice = value; }
        }
        private double lastPrice;
        public double last
        {
            get { return lastPrice; }
            set { lastPrice = value; }
        }
        private double volume;
        public double vol
        {
            get { return volume; }
            set { volume = value; }
        }
        private double dayHighPrice;
        public double day_high
        {
            get { return dayHighPrice; }
            set { dayHighPrice = value; }
        }
        private double dayLowPrice;
        public double day_low
        {
            get { return dayLowPrice; }
            set { dayLowPrice = value; }
        }
        private double coinVolume;
        public double coin_vol
        {
            get { return coinVolume; }
            set { coinVolume = value; }
        }
        private uint unitAmout;
        public uint unit_amount
        {
            get { return unitAmout; }
            set { unitAmout = value; }
        }
    }

    class OkexOrderInfo
    {
        public double price = 0.0;
        public double volume = 0;
    }

    class OkexFutureDepthData
    {
        public OkexOrderInfo[] asks = new OkexOrderInfo[5];
        public OkexOrderInfo[] bids = new OkexOrderInfo[5];

        public OkexFutureDepthData()
        {
            for(int i = 0; i < 5; i++)
            {
                bids[i] = new OkexOrderInfo();
                asks[i] = new OkexOrderInfo();
            }
        }
    }

    class OkexFutureTradeInfo
    {
        private long dateTime;
        public long date
        {
            get { return dateTime; }
            set { dateTime = value; }
        }
        private long millisec;
        public long date_ms
        {
            get { return millisec; }
            set { millisec = value; }
        }
        private long volume;
        public long amount
        {
            get { return volume; }
            set { volume = value; }
        }
        private double tradePrice;
        public double price
        {
            get { return tradePrice; }
            set { tradePrice= value; }
        }
        
        private long tradeID;
        public long tid
        {
            get { return tradeID; }
            set { tradeID = value; }
        }
        private string tradeType;
        public string type
        {
            get { return tradeType; }
            set { tradeType = value; }
        }
    }

    class OkexKLineData
    {
        public long timestamp;  //时间戳
        public double open;
        public double high;
        public double low;
        public double close;
        public long volume;
        public double refValue; //交易量转化BTC或LTC数量
    }

    class OkexContractInfo
    {
        private string contractType;
        public string contract_type
        {
            get { return contractType; }
            set { contractType = value; }
        }

        private double freezeAmount;
        public double freeze
        {
            get { return freezeAmount; }
            set { freezeAmount = value; }
        }

        private double contractBalance; //账户余额
        public double balance
        {
            get { return contractBalance; }
            set { contractBalance = value; }
        }

        private long contractID;
        public long contract_id
        {
            get { return contractID; }
            set { contractID = value; }
        }

        private double availableAmount;
        public double available
        {
            get { return availableAmount; }
            set { availableAmount = value; }
        }

        private double contractBond; //保证金
        public double bond
        {
            get { return contractBond; }
            set { contractBond = value; }
        }

        private double realProfit; //已实现盈亏
        public double profit
        {
            get { return realProfit; }
            set { realProfit = value; }
        }

        private double unrealProfit; //未实现盈亏
        public double unprofit
        {
            get { return unrealProfit; }
            set { unrealProfit = value; }
        }
    }

    class OkexAccountInfo
    {
        private double accountBalance; // 账户余额
        public double balance
        {
            get { return accountBalance; }
            set { accountBalance = value; }
        }

        private double accountRights; // 账户权益
        public double rights
        {
            get { return accountRights; }
            set { accountRights = value; }
        }

        public List<OkexContractInfo> contractsInfo = new List<OkexContractInfo>(); 
    }

    class OkexPositionBriefInfo
    {
        public long contractID;
        public OkexFutureInstrumentType instrument;
        public OkexFutureContractType contractType;
        public OkexFutureTradeDirectionType direction;
        public uint leverRate;

        public long amount;
        public long available;
        public double bond;
        public double avgPrice;
        public double costPrice;
        public double flatPrice;

        public OkexPositionBriefInfo()
        {
            //
        }

        public OkexPositionBriefInfo(OkexPositionInfo info, OkexFutureInstrumentType inst, 
                                    OkexFutureContractType ct, OkexFutureTradeDirectionType dir)
        {
            contractID = info.contract_id;
            instrument = inst;
            contractType = ct;
            direction = dir;

            leverRate = info.lever_rate;

            if(dir == OkexFutureTradeDirectionType.FTD_Buy)
            {
                amount = info.buy_amount;
                available = info.buy_available;
                avgPrice = info.buy_price_avg;
                costPrice = info.buy_price_cost;
                bond = info.buy_bond;
                flatPrice = info.buy_flatprice;
            }
            else
            {
                amount = info.sell_amount;
                available = info.sell_available;
                avgPrice = info.sell_price_avg;
                costPrice = info.sell_price_cost;
                bond = info.sell_bond;
                flatPrice = info.sell_flatprice;
            }
        }
    }

    class OkexPositionInfo              //逐仓，仓位信息
    {
        private long contractID;
        public long contract_id
        {
            get { return contractID; }
            set { contractID = value; }
        }

        private string contractType;
        public string contract_type
        {
            get { return contractType; }
            set { contractType = value; }
        }

        private long createDate;
        public long create_date
        {
            get { return createDate; }
            set { createDate = value; }
        }

        private string positionSymbol;
        public string symbol
        {
            get { return positionSymbol; }
            set { positionSymbol = value; }
        }

        private uint leverRate; //杠杆率
        public uint lever_rate
        {
            get { return leverRate; }
            set { leverRate = value; }
        }

        // 多仓
        private long buyAmount;
        public long buy_amount
        {
            get { return buyAmount; }
            set { buyAmount = value; }
        }

        private long buyAvailable; //多仓可平仓数量 
        public long buy_available
        {
            get { return buyAvailable; }
            set { buyAvailable = value; }
        }

        private double buyBond; //多仓保证金
        public double buy_bond
        {
            get { return buyBond; }
            set { buyBond = value; }
        }

        private double buyFlatPrice; // 多仓强平价格
        public double buy_flatprice
        {
            get { return buyFlatPrice; }
            set { buyFlatPrice = value; }
        }

        private double buyProfitLossRatio; // 多仓盈亏比
        public double buy_profit_lossratio
        {
            get { return buyProfitLossRatio; }
            set { buyProfitLossRatio = value; }
        }

        private double buyPriceAvg; //开仓平均价
        public double buy_price_avg
        {
            get { return buyPriceAvg; }
            set { buyPriceAvg = value; }
        }

        private double buyPriceCost; //结算基准价
        public double buy_price_cost
        {
            get { return buyPriceCost; }
            set { buyPriceCost = value; }
        }

        //private double buyProfitReal; //多仓已实现盈余
        //public double buy_profit_real
        //{
        //    get { return buyProfitReal; }
        //    set { buyProfitReal = value; }
        //}

        // 空仓
        private long sellAmount;
        public long sell_amount
        {
            get { return sellAmount; }
            set { sellAmount = value; }
        }

        private long sellAvailable;     //空仓可平仓数量 
        public long sell_available
        {
            get { return sellAvailable; }
            set { sellAvailable = value; }
        }

        private double sellBond;        //空仓保证金
        public double sell_bond
        {
            get { return sellBond; }
            set { sellBond = value; }
        }

        private double sellFlatPrice; // 空仓强平价格
        public double sell_flatprice
        {
            get { return sellFlatPrice; }
            set { sellFlatPrice = value; }
        }

        private double sellProfitLossRatio; // 空仓盈亏比
        public double sell_profit_lossratio
        {
            get { return sellProfitLossRatio; }
            set { sellProfitLossRatio = value; }
        }

        private double sellPriceAvg; //开仓平均价
        public double sell_price_avg
        {
            get { return sellPriceAvg; }
            set { sellPriceAvg = value; }
        }

        private double sellPriceCost; //结算基准价
        public double sell_price_cost
        {
            get { return sellPriceCost; }
            set { sellPriceCost = value; }
        }

        //private double sellProfitReal; //空仓已实现盈余
        //public double sell_profit_real
        //{
        //    get { return sellProfitReal; }
        //    set { sellProfitReal = value; }
        //}
    }

    class OkexFutureOrderBriefInfo
    {
        public string contractName;
        public OkexContractTradeType tradeType;
        public int leverRate;
        public double price;
        public long amount;
        public OkexOrderStatusType status;
        public long orderID;
    }

    class OkexFutureTrader : Singleton<OkexFutureTrader>
    {     
        FutureRestApiV1 getRequest;
        FutureRestApiV1 postRequest;

        string[] contractTypeName = { "this_week", "next_week", "quarter" };
        string[] instrumentQuotationName = { "btc_usd", "ltc_usd", "eth_usd", "etc_usd" , "bch_usd" };
        string[] coinName = { "btc", "ltc", "eth", "etc", "bch" };

        string[] kLineTypeName = {"1min", "3min","5min", "15min", "30min", "1hour", "2hour", "4hour", "6hour", "12hour", "day", "3day", "week"};

        public OkexFutureTrader()
        {
            getRequest = new FutureRestApiV1(OkexParam.url_prex);
            postRequest = new FutureRestApiV1(OkexParam.url_prex, OkexParam.api_key, OkexParam.secret_key);
        }

        // 行情
        public OkexFutureMarketData getMarketData(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            string str = getRequest.future_ticker(instrumentQuotationName[(int)instrument], contractTypeName[(int)contract]);
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            //if (!futureData.ContainsKey(instrument))
            //{
            //    OkexFutureData fd = new OkexFutureData();
            //    futureData.Add(instrument, fd);
            //}

            //if (!futureData[instrument].marketData.ContainsKey(contract))
            //{
            //    futureData[instrument].marketData.Add
            //}
            OkexFutureMarketData md = new OkexFutureMarketData();
            md = (OkexFutureMarketData)JsonConvert.DeserializeObject(jo["ticker"].ToString(), typeof(OkexFutureMarketData));
            return md;
        }

        // 盘口信息
        public OkexFutureDepthData getMarketDepthData(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            OkexFutureDepthData dd = new OkexFutureDepthData();
            string str = getRequest.future_depth(instrumentQuotationName[(int)instrument], contractTypeName[(int)contract]);
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            JArray bidArr = JArray.Parse(jo["bids"].ToString());
            JArray askArr = JArray.Parse(jo["asks"].ToString());
            for (int i = 0; i < 5; i++)
            {
                JArray ordArr = JArray.Parse(bidArr[i].ToString());
                double p = (double)ordArr[0];
                double v = (double)ordArr[1];
                dd.bids[i].price = p;
                dd.bids[i].volume = v;

                ordArr = JArray.Parse(askArr[i].ToString());
                p = (double)ordArr[0];
                v = (double)ordArr[1];
                dd.asks[i].price = p;
                dd.asks[i].volume = v;
            }
            return dd;
        }

        // 成交信息
        public List<OkexFutureTradeInfo> getTradesInfo(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            List<OkexFutureTradeInfo> trades = new List<OkexFutureTradeInfo>();
            string str = getRequest.future_trades(instrumentQuotationName[(int)instrument], contractTypeName[(int)contract]);
            JArray arr = JArray.Parse(str);
            
            foreach(var item in arr)
            {
                OkexFutureTradeInfo ti = (OkexFutureTradeInfo)JsonConvert.DeserializeObject(item.ToString(), typeof(OkexFutureTradeInfo));
                trades.Add(ti);
            }

            return trades;
        }

        // 期货指数
        public double getFutureIndex(OkexFutureInstrumentType instrument)
        {
            string str = getRequest.future_index(instrumentQuotationName[(int)instrument]);
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            double futureIdx = (double)jo["future_index"];
            return futureIdx;
        }

        // 美元汇率
        public double getExchangeRate()
        {
            string str = getRequest.exchange_rate();
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            double er = (double)jo["rate"];
            return er;
        }

        // always return 0
        public double getEstimatePrice(OkexFutureInstrumentType instrument)
        {
            string str = getRequest.future_estimated_price(instrumentQuotationName[(int)instrument]);

            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            double ep = (double)jo["forecast_price"];
            return ep;
        }

        // K线
        public List<OkexKLineData> getKLineData(OkexFutureInstrumentType instrument, OkexFutureContractType contract, OkexKLineType type)
        {
            List<OkexKLineData> kLines = new List<OkexKLineData>();
            string str = getRequest.future_kline(instrumentQuotationName[(int)instrument], kLineTypeName[(int)type], contractTypeName[(int)contract], "", "");
            JArray arr = JArray.Parse(str);

            foreach (var item in arr)
            {
                JArray klArr = JArray.Parse(item.ToString());
                OkexKLineData kld = new OkexKLineData();
                kld.timestamp = (long)klArr[0];
                kld.open = (double)klArr[1];
                kld.high = (double)klArr[2];
                kld.low = (double)klArr[3];
                kld.close = (double)klArr[4];
                kld.volume = (long)klArr[5];
                kld.refValue = (double)klArr[6];
                kLines.Add(kld);
            }

            return kLines;
        }

        // 获取当前可用合约总持仓量
        public long getMarketHoldAmount(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            string str = getRequest.future_hold_amount(instrumentQuotationName[(int)instrument], contractTypeName[(int)contract]);
            JArray arr = JArray.Parse(str);
            JObject jo = (JObject)JsonConvert.DeserializeObject(arr[0].ToString());
            long amount = (long)jo["amount"];
            return amount;
        }

        // 账户信息
        public bool getUserInfo(out Dictionary<OkexFutureInstrumentType, OkexAccountInfo> info)
        {
            string str = postRequest.future_userinfo_4fix();
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool result = (bool)jo["result"];

            info = new Dictionary<OkexFutureInstrumentType, OkexAccountInfo>();
            
            if (result == true)
            {
                for (int i = 0; i <= (int)OkexFutureInstrumentType.FI_BCH; i++)
                {
                    OkexFutureInstrumentType fi = (OkexFutureInstrumentType)i;
                    string instrumentName = coinName[i];
                    OkexAccountInfo ai = new OkexAccountInfo();
                    ai.balance = (double)jo["info"][instrumentName]["balance"];
                    ai.rights = (double)jo["info"][instrumentName]["rights"];
                    JArray arr = JArray.Parse(jo["info"][instrumentName]["contracts"].ToString());
                    foreach (var item in arr)
                    {
                        OkexContractInfo ci = (OkexContractInfo)JsonConvert.DeserializeObject(item.ToString(), typeof(OkexContractInfo));
                        ai.contractsInfo.Add(ci);
                    }
                    info.Add(fi, ai);
                }
            }
            return result;
        }

        // 仓位信息
        public bool getFuturePosition(OkexFutureInstrumentType instrument, OkexFutureContractType contract, out List<OkexPositionInfo> info)
        {
            string str = postRequest.future_position_4fix(instrumentQuotationName[(int)instrument], contractTypeName[(int)contract]);
            info = new List<OkexPositionInfo>();

            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool result = (bool)jo["result"];
            if (result)
            {
                JArray arr = JArray.Parse(jo["holding"].ToString());
                foreach(var item in arr)
                {
                    OkexPositionInfo pi = JsonConvert.DeserializeObject<OkexPositionInfo>(item.ToString());
                    info.Add(pi);
                }
            }

            return result;
        }

        // extension for position holding query
        public List<OkexPositionBriefInfo> getHoldPosition(OkexFutureInstrumentType instrument, OkexFutureContractType contract, 
                                    OkexFutureTradeDirectionType direction, uint leverRate)
        {
            List<OkexPositionBriefInfo> briefInfo = new List<OkexPositionBriefInfo>();
            List<OkexPositionInfo> info;
            bool hold = getFuturePosition(instrument, contract, out info);
            if (hold)
            {
                foreach(var pi in info)
                {
                    pi.contract_type
                    OkexPositionBriefInfo bi = new OkexPositionBriefInfo(pi, instrument, contract, direction);
                }
            }

            return briefInfo;
        }

        // 交易 开仓平仓
        public long trade(OkexFutureInstrumentType instrument, OkexFutureContractType contract, double price, double amount, OkexContractTradeType tradeType, 
                            uint leverRate = 10, bool matchPrice = false)
        {
            string strMatchPrice = "";
            if (matchPrice)
            {
                strMatchPrice = "1";
            }
            else
            {
                strMatchPrice = "0";
            }

            if(leverRate != 10 && leverRate != 20)
            {
                leverRate = 10;
            }

            uint nType = (uint)tradeType;
            string str = postRequest.future_trade_ex(instrumentQuotationName[(int)instrument], contractTypeName[(int)contract], price.ToString(), amount.ToString(), nType.ToString(),
                                        strMatchPrice, leverRate.ToString());
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool ret = (bool)jo["result"];
            if (!ret)
            {
                return 0;
            }

            long orderID = (long)jo["order_id"];
            return orderID;
        }

        public bool cancel(OkexFutureInstrumentType instrument, OkexFutureContractType contract, long orderID)
        {
            string str = postRequest.future_cancel(instrumentQuotationName[(int)instrument], contractTypeName[(int)contract], orderID.ToString());
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool ret = (bool)jo["result"];
            
            return ret;
        }

        public bool getCurOrdersInfo(OkexFutureInstrumentType instrument, OkexFutureContractType contract, 
                                    out List<OkexFutureOrderBriefInfo> briefInfo, bool finished = false)
        {
            List<OkexFutureOrderBriefInfo> ordersBriefInfo = new List<OkexFutureOrderBriefInfo>();
            string strFinished = "1";
            if (finished)
            {
                strFinished = "2";
            }
            string str = postRequest.future_order_info(instrumentQuotationName[(int)instrument], contractTypeName[(int)contract], "-1", strFinished, "0", "1");

            briefInfo = new List<OkexFutureOrderBriefInfo>();
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool ret = (bool)jo["result"];

            if (ret)
            {
                JArray arr = JArray.Parse(jo["orders"].ToString());
                foreach(var item in arr)
                {
                    OkexFutureOrderBriefInfo obi = new OkexFutureOrderBriefInfo();
                    obi.amount = (long)item["amount"];
                    obi.contractName = (string)item["contract_name"];
                    obi.leverRate = (int)item["lever_rate"];
                    obi.price = (double)item["price"];
                    obi.tradeType = (OkexContractTradeType)int.Parse((string)item["type"]);
                    obi.status = (OkexOrderStatusType)int.Parse((string)item["status"]);
                    obi.orderID = (long)item["order_id"];
                    briefInfo.Add(obi);
                }
            }

            return ret;
        }

        public bool getOrderInfoByID(OkexFutureInstrumentType instrument, OkexFutureContractType contract,
                                     long orderID, out OkexFutureOrderBriefInfo info)
        {
            List<OkexFutureOrderBriefInfo> ordersBriefInfo = new List<OkexFutureOrderBriefInfo>();
            string str = postRequest.future_order_info(instrumentQuotationName[(int)instrument], contractTypeName[(int)contract], orderID.ToString(), "1", "0", "1");

            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool ret = (bool)jo["result"];

            info = new OkexFutureOrderBriefInfo();
            if (ret)
            {
                JArray arr = JArray.Parse(jo["orders"].ToString());
                foreach (var item in arr)
                {
                    info.amount = (long)item["amount"];
                    info.contractName = (string)item["contract_name"];
                    info.leverRate = (int)item["lever_rate"];
                    info.price = (double)item["price"];
                    info.tradeType = (OkexContractTradeType)int.Parse((string)item["type"]);
                    info.status = (OkexOrderStatusType)int.Parse((string)item["status"]);
                    info.orderID = (long)item["order_id"];
                    break;
                }
            }

            return ret;
        }
    }
}
