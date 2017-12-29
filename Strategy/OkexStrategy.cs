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

    abstract class OkexStrategy
    {
        public abstract void update();

        protected Dictionary<long, OkexLocalOrderBriefInfo> m_openOrders = new Dictionary<long, OkexLocalOrderBriefInfo>();
        protected Dictionary<long, OkexLocalOrderBriefInfo> m_closeOrders = new Dictionary<long, OkexLocalOrderBriefInfo>();

        protected void trade(OkexFutureInstrumentType instrument, OkexFutureContractType contract, double price, double volume, OkexContractTradeType type, uint leverRate = 10)
        {
            return;
            long orderID = OkexFutureTrader.Instance.trade(instrument, contract, price, volume, type, leverRate);
            //return orderID;
            OkexLocalOrderBriefInfo lobi = new OkexLocalOrderBriefInfo(orderID, instrument, contract, leverRate);

            if (type == OkexContractTradeType.TT_CloseSell || type == OkexContractTradeType.TT_CloseBuy)
            {
                m_closeOrders.Add(orderID, lobi);
            }
            else
            {
                m_openOrders.Add(orderID, lobi);
            }
        }

        protected void checkOrders()
        {
            List<long> idsToRemove = new List<long>();
            foreach (var itor in m_openOrders)
            {
                bool ret = false;
                OkexLocalOrderBriefInfo lobi = itor.Value;
                OkexFutureOrderBriefInfo bi;
                ret = OkexFutureTrader.Instance.getOrderInfoByID(lobi.instrumentType, lobi.contractType, lobi.orderID, out bi);

                if (ret)
                {
                    if (bi.status != OkexOrderStatusType.OS_NotTraded && bi.status != OkexOrderStatusType.OS_PartiallyTraded)
                    {
                        idsToRemove.Add(lobi.orderID);
                    }
                }
            }
            foreach (var itor in m_closeOrders)
            {
                bool ret = false;
                OkexLocalOrderBriefInfo lobi = itor.Value;
                OkexFutureOrderBriefInfo bi;
                ret = OkexFutureTrader.Instance.getOrderInfoByID(lobi.instrumentType, lobi.contractType, lobi.orderID, out bi);

                if (ret)
                {
                    if (bi.status != OkexOrderStatusType.OS_NotTraded && bi.status != OkexOrderStatusType.OS_PartiallyTraded)
                    {
                        idsToRemove.Add(lobi.orderID);
                    }
                }
            }

            foreach (long id in idsToRemove)
            {
                m_openOrders.Remove(id);
                m_closeOrders.Remove(id);
            }
        }


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
            bool ret = OkexFutureTrader.Instance.getCurOrdersInfo(instrument, contract, out info);
            if (ret)
            {
                foreach (var bi in info)
                {
                    bool countIn = false;
                    if (isTradeTypeMatchDirection(bi.tradeType, direction))
                    {
                        if (bi.status == OkexOrderStatusType.OS_NotTraded || bi.status == OkexOrderStatusType.OS_PartiallyTraded)
                        {
                            if (inOpenOrder)
                            {
                                if (m_openOrders.ContainsKey(bi.orderID))
                                {
                                    countIn = true;
                                }
                            }
                            else
                            {
                                if (m_closeOrders.ContainsKey(bi.orderID))
                                {
                                    countIn = true;
                                }
                            }

                            if (countIn)
                            {
                                orderedPosition += bi.amount;
                            }
                        }
                    }
                }
            }

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
            OkexFutureMarketData md = OkexFutureTrader.Instance.getMarketData(instrument, contract);
            if (md != null)
            {
                return md.buy;
            }
            return 0.0;
        }

        protected double getCurSellPrice(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            OkexFutureMarketData md = OkexFutureTrader.Instance.getMarketData(instrument, contract);
            if (md != null)
            {
                return md.sell;
            }
            return 0.0;
        }

        protected bool isTradeTypeMatchDirection(OkexContractTradeType tradeType, OkexFutureTradeDirectionType direction)
        {
            if(direction == OkexFutureTradeDirectionType.FTD_Buy)
            {
                return (tradeType == OkexContractTradeType.TT_CloseBuy || tradeType == OkexContractTradeType.TT_OpenBuy);
            }
            else
            {
                return (tradeType == OkexContractTradeType.TT_CloseSell || tradeType == OkexContractTradeType.TT_OpenSell);
            }
        }
    }
}
