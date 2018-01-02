using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OkexTrader.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OkexTrader.FutureTrade
{
    public enum OkexTradeQueryResultType
    {
        TQR_Timeout,
        TQR_Unfinished,
        TQR_Finished
    }

    abstract class FutureTradeEntity
    {
        public delegate void TradeEventHandler(long orderID, OkexTradeQueryResultType result, OkexFutureOrderBriefInfo info);
        public event TradeEventHandler tradeEventHandler;

        public long queryInterval = 10;

        public void trade(OkexFutureInstrumentType instrument, OkexFutureContractType contract,
            double price, long volume, OkexContractTradeType type, uint leverRate = 10)
        {
            FutureTradeMgr.Instance.trade(this, instrument, contract, price, volume, type, leverRate);            
        }

        abstract public void onTradeOrdered(long orderID);

        public void onTradeEvent(long orderID, OkexTradeQueryResultType result, OkexFutureOrderBriefInfo info)
        {
            tradeEventHandler(orderID, result, info);
        }
    }
}
