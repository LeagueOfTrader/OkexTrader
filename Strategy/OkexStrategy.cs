using OkexTrader.FutureTrade;
using OkexTrader.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.Strategy
{
    class OkexLocalOrderBriefInfo
    {
        public long orderID;
        public OkexFutureInstrumentType instrumentType;
        public OkexFutureContractType contractType;
        public uint leverRate;

        public OkexLocalOrderBriefInfo(long id, OkexFutureInstrumentType fi, OkexFutureContractType fc, uint lr)
        {
            orderID = id;
            instrumentType = fi;
            contractType = fc;
            leverRate = lr;
        }
    }

    abstract class OkexStrategy : FutureTradeEntity
    {
        public abstract void update();

        protected override void onTradeOrdered(long orderID)
        {
            //
        }
    }
}
