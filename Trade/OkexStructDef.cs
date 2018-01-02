using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public long timestamp;
        public long receiveTimestamp;
    }

    class OkexOrderInfo
    {
        public double price = 0.0;
        public long volume = 0;
    }

    class OkexFutureDepthData
    {
        public OkexOrderInfo[] asks = new OkexOrderInfo[5];
        public OkexOrderInfo[] bids = new OkexOrderInfo[5];
        public long sendTimestamp = 0;
        public long receiveTimestamp = 0;

        public OkexFutureDepthData()
        {
            for (int i = 0; i < 5; i++)
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
            set { tradePrice = value; }
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

            if (dir == OkexFutureTradeDirectionType.FTD_Buy)
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
}
