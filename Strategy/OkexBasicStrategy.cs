using OkexTrader.FutureTrade;
using OkexTrader.MarketData;
using OkexTrader.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.Strategy
{
    abstract class OkexBasicStrategy : OkexStrategy
    {
        protected long getPositionByContract(OkexFutureInstrumentType instrument, OkexFutureContractType contract, OkexFutureTradeDirectionType direction)
        {
            return OkexFutureTrader.Instance.getHoldPositionAmount(instrument, contract, direction);
        }

        protected long getAvailablePositionByContract(OkexFutureInstrumentType instrument, OkexFutureContractType contract, OkexFutureTradeDirectionType direction)
        {
            OkexPositionBriefInfo bi10 = OkexFutureTrader.Instance.getHoldPosition(instrument, contract, direction, 10);
            OkexPositionBriefInfo bi20 = OkexFutureTrader.Instance.getHoldPosition(instrument, contract, direction, 20);

            long availablePosition = 0;
            if (bi10 != null)
            {
                availablePosition += bi10.available;
            }
            if (bi20 != null)
            {
                availablePosition += bi20.available;
            }

            return availablePosition;
        }

        protected long getOrderedPositionByContract(OkexFutureInstrumentType instrument, OkexFutureContractType contract, OkexFutureTradeDirectionType direction, bool inOpenOrder)
        {
            List<OkexFutureOrderBriefInfo> info;
            long orderedPosition = 0;
            //

            return orderedPosition;
        }

        protected double getCurPrice(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            OkexFutureMarketData md = OkexFutureTrader.Instance.getMarketData(instrument, contract);
            if (md != null)
            {
                //if(m_tradeDirection == OkexFutureTradeDirectionType.TT_Buy)
                //{
                //    return md.sell;
                //}
                //else
                //{
                //    return md.buy;
                //}
                return md.last;
            }
            return 0.0;
        }

        protected double getCurBuyPrice(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            OkexFutureMarketData md = MarketDataMgr.Instance.getMarketData(instrument, contract);
            if (md != null)
            {
                return md.buy;
            }
            return 0.0;
        }

        protected double getCurSellPrice(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            OkexFutureMarketData md = MarketDataMgr.Instance.getMarketData(instrument, contract);
            if (md != null)
            {
                return md.sell;
            }
            return 0.0;
        }

        protected bool isTradeTypeMatchDirection(OkexContractTradeType tradeType, OkexFutureTradeDirectionType direction)
        {
            if (direction == OkexFutureTradeDirectionType.FTD_Buy)
            {
                return (tradeType == OkexContractTradeType.TT_CloseBuy || tradeType == OkexContractTradeType.TT_OpenBuy);
            }
            else
            {
                return (tradeType == OkexContractTradeType.TT_CloseSell || tradeType == OkexContractTradeType.TT_OpenSell);
            }
        }

        //public override void update()
        //{
        //    //
        //}
    }
}
