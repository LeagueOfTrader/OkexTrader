using OkexTrader.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.Strategy
{
    public enum OkexBasisCalcType
    {
        BC_Ratio,
        BC_Diff
        //BC_Fix
    }

    //public enum OkexContractTrendType
    //{
    //    CT_Premium,  //升水
    //    CT_Agio      //贴水
    //}

    class OkexTransferPositionByBasis : OkexStrategy
    {
        public class OkexBasisDiffPositionData
        {
            public double basisDiff = 0.0;
            public long spotContractPosition = 0;
            public long forwardContractPosition = 0;
        }        

        Dictionary<string, OkexFutureContractType> contractTypeMap = new Dictionary<string, OkexFutureContractType>()
                                                                    { { "this_week", OkexFutureContractType.FC_ThisWeek },
                                                                        { "next_week", OkexFutureContractType.FC_NextWeek },
                                                                        { "quarter", OkexFutureContractType.FC_Quarter }};

        //const double minOrderUnit = 0.1;

        private OkexFutureInstrumentType m_instrument = OkexFutureInstrumentType.FI_LTC;
        private OkexFutureContractType m_spotContract;
        private OkexFutureContractType m_forwardContract;
        //private OkexContractTrendType m_trendType;
        private OkexFutureTradeDirectionType m_tradeDirection;
        private OkexBasisCalcType m_basisCalcType = OkexBasisCalcType.BC_Ratio;

        private double m_basis = 0.0;
        private double m_safeRange = 0.0;
        private double m_limitRange = 0.0;
        private uint m_count = 1;
        private double m_totalPosition = 0.0;
        private double m_ratio = 0.0;
        private double m_diff = 0.0;
        private OkexBasisDiffPositionData[] m_fwBasisDiffArr = null;
        private OkexBasisDiffPositionData[] m_rvBasisDiffArr = null;

        HashSet<long> m_openOrders = new HashSet<long>();
        HashSet<long> m_closeOrders = new HashSet<long>();

        public OkexTransferPositionByBasis(OkexFutureInstrumentType inst, OkexFutureContractType sc, OkexFutureContractType fc,
                                            OkexBasisCalcType type, OkexFutureTradeDirectionType tradeDir)
        {
            m_instrument = inst;
            m_basisCalcType = type;
            m_spotContract = sc;
            m_forwardContract = fc;
            m_tradeDirection = tradeDir;
        }

        public void init(double basis, double safe, double limit, uint count, double param)
        {
            m_count = count;
            m_fwBasisDiffArr = new OkexBasisDiffPositionData[m_count];
            m_rvBasisDiffArr = new OkexBasisDiffPositionData[m_count];
            m_basis = basis;
            m_safeRange = safe;
            m_limitRange = limit;            

            buildBasisDiffArray();

            switch (m_basisCalcType)
            {
                case OkexBasisCalcType.BC_Ratio:
                    m_ratio = param;
                    break;
                case OkexBasisCalcType.BC_Diff:
                    m_diff = param;
                    break;
                //case OkexBasisCalcType.BC_Fix:
                //    break;
                default:
                    break;
            }

            reset();
        }

        public void reset()
        {
            m_totalPosition = getPositionByContract(m_spotContract) + getPositionByContract(m_forwardContract);

            switch (m_basisCalcType)
            {
                case OkexBasisCalcType.BC_Ratio:
                    initWithRatio();
                    break;
                case OkexBasisCalcType.BC_Diff:
                    initWithDiff();
                    break;
                default:
                    break;
            }
        }

        //
        public void setTotalPosition(double total)
        {
            m_totalPosition = total;

            switch (m_basisCalcType)
            {
                case OkexBasisCalcType.BC_Ratio:
                    initWithRatio();
                    break;
                case OkexBasisCalcType.BC_Diff:
                    initWithDiff();
                    break;
                default:
                    break;
            }
        }

        private void initWithRatio() // 每格档百分比
        {
            double avgPosition = m_totalPosition / 2.0;
            double accumVal = m_ratio;
            for (int i = 0; i < m_count; i++)
            {
                m_fwBasisDiffArr[i].spotContractPosition = (1.0 - accumVal) * avgPosition;
                m_fwBasisDiffArr[i].forwardContractPosition = (1.0 + accumVal) * avgPosition;

                m_rvBasisDiffArr[i].spotContractPosition = (1.0 + accumVal) * avgPosition;
                m_rvBasisDiffArr[i].forwardContractPosition = (1.0 - accumVal) * avgPosition;

                accumVal += m_ratio;
            }
        }

        private void initWithDiff() // 每格档固定值
        {
            double avgPosition = m_totalPosition / 2.0;
            double accumVal = m_diff;
            for (int i = 0; i < m_count; i++)
            {
                m_fwBasisDiffArr[i].spotContractPosition = avgPosition - accumVal;
                m_fwBasisDiffArr[i].forwardContractPosition = avgPosition + accumVal;

                m_rvBasisDiffArr[i].spotContractPosition = avgPosition + accumVal;
                m_rvBasisDiffArr[i].forwardContractPosition = avgPosition - accumVal;

                accumVal += m_diff;
            }
        }

        public override void update()
        {
            double spotPrice = getCurPrice(m_spotContract);
            double forwardPrice = getCurPrice(m_forwardContract);

            double basisDiff = forwardPrice - spotPrice;

            int dir = getBasisDiffDirection(basisDiff);
            if (dir == 0)
            {
                return;
            }

            if (dir > 0)
            {
                tryTransferToForward(basisDiff);
            }
            else
            {
                tryTransferToSpot(basisDiff);
            }

            // remove finished orders
            checkOrders();
        }

        private void checkOrders()
        {
            List<long> idsToRemove = new List<long>();
            foreach(long id in m_openOrders)
            {
                bool ret = false;
                OkexFutureOrderBriefInfo bi;
                ret = OkexFutureTrader.Instance.getOrderInfoByID(m_instrument, m_spotContract, id, out bi);
                if (!ret)
                {
                    ret = OkexFutureTrader.Instance.getOrderInfoByID(m_instrument, m_forwardContract, id, out bi);
                }

                if (ret)
                {
                    if(bi.status != OkexOrderStatusType.OS_NotTraded && bi.status != OkexOrderStatusType.OS_PartiallyTraded)
                    {
                        idsToRemove.Add(id);
                    }
                }
            }
            foreach (long id in m_closeOrders)
            {
                bool ret = false;
                OkexFutureOrderBriefInfo bi;
                ret = OkexFutureTrader.Instance.getOrderInfoByID(m_instrument, m_spotContract, id, out bi);
                if (!ret)
                {
                    ret = OkexFutureTrader.Instance.getOrderInfoByID(m_instrument, m_forwardContract, id, out bi);
                }

                if (ret)
                {
                    if (bi.status != OkexOrderStatusType.OS_NotTraded && bi.status != OkexOrderStatusType.OS_PartiallyTraded)
                    {
                        idsToRemove.Add(id);
                    }
                }
            }

            foreach(long id in idsToRemove)
            {
                m_openOrders.Remove(id);
                m_closeOrders.Remove(id);
            }
        }

        private void tryTransferToForward(double basisDiff)
        {
            double spotPosition = getPositionByContract(m_spotContract);
            double forwardPosition = getPositionByContract(m_forwardContract);
            double avgPosition = (spotPosition + forwardPosition) / 2.0;

            bool asc = true;
            if (m_tradeDirection == OkexFutureTradeDirectionType.FTD_Buy)
            {
                asc = false;
            }
            uint index = getIndexInBDArray(ref m_fwBasisDiffArr, basisDiff, asc);

            transfer(m_fwBasisDiffArr[index].spotContractPosition, m_fwBasisDiffArr[index].forwardContractPosition, 
                     m_spotContract, m_forwardContract);
        }

        private void transfer(long targetFromtPosition, long targetToPosition, 
                                OkexFutureContractType fromContract, OkexFutureContractType toContract)
        {            
            long curSpotPosition = getFreePositionByContract(fromContract);// - getOrderedPositionByContract(m_spotContract, true);
            long curTargetForwardPosition = getPositionByContract(toContract) + getOrderedPositionByContract(toContract, true);

            if(curSpotPosition < targetFromtPosition)
            {
                long spDiff = targetFromtPosition - curSpotPosition;
                long targetVol = spDiff;
                OkexFutureDepthData fromDD = OkexFutureTrader.Instance.getMarketDepthData(m_instrument, fromContract);
                OkexFutureDepthData toDD = OkexFutureTrader.Instance.getMarketDepthData(m_instrument, toContract);
                if(m_tradeDirection == OkexFutureTradeDirectionType.FTD_Sell)
                {
                    double bidVol = fromDD.bids[0].volume;
                    double askVol = toDD.asks[0].volume;
                    double vol = Math.Min(bidVol, askVol);
                    vol = Math.Min(vol, targetVol);

                    long closeOrderID = OkexFutureTrader.Instance.trade(m_instrument, fromContract, fromDD.bids[0].price, vol, OkexContractTradeType.TT_CloseBuy);
                    m_closeOrders.Add(closeOrderID);
                    long openOrderID = OkexFutureTrader.Instance.trade(m_instrument, toContract, toDD.asks[0].price, vol, OkexContractTradeType.TT_OpenSell);
                    m_openOrders.Add(openOrderID);
                }
                else
                {
                    double askVol = fromDD.asks[0].volume;
                    double bidVol = toDD.asks[0].volume;
                    double vol = Math.Min(askVol, bidVol);
                    vol = Math.Min(vol, targetVol);

                    long closeOrderID = OkexFutureTrader.Instance.trade(m_instrument, fromContract, fromDD.asks[0].price, vol, OkexContractTradeType.TT_CloseSell);
                    m_closeOrders.Add(closeOrderID);
                    long openOrderID = OkexFutureTrader.Instance.trade(m_instrument, toContract, toDD.bids[0].price, vol, OkexContractTradeType.TT_OpenBuy);
                    m_openOrders.Add(openOrderID);
                }
            }

        }

        private void tryTransferToSpot(double basisDiff)
        {
            double spotPosition = getPositionByContract(m_spotContract);
            double forwardPosition = getPositionByContract(m_forwardContract);
            double avgPosition = (spotPosition + forwardPosition) / 2.0;

            bool asc = true;
            if (m_tradeDirection == OkexFutureTradeDirectionType.FTD_Buy)
            {
                asc = false;
            }
            uint index = getIndexInBDArray(ref m_rvBasisDiffArr, basisDiff, asc);

            transfer(m_rvBasisDiffArr[index].forwardContractPosition, m_rvBasisDiffArr[index].spotContractPosition,
                     m_forwardContract, m_spotContract);
        }

        private bool isBasisDiffEnough(double bd)
        {
            if (bd > m_basis + m_safeRange || bd < m_basis - m_safeRange)
            {
                return true;
            }

            return false;
        }

        private int getBasisDiffDirection(double bd)
        {
            int ret = 0;

            if (bd > m_basis + m_safeRange)
            {
                ret = 1;
            }
            else if (bd < m_basis - m_safeRange)
            {
                ret = -1;
            }

            if (m_tradeDirection == OkexFutureTradeDirectionType.FTD_Buy)
            {
                ret = -ret;
            }

            return ret;
        }

        private void buildBasisDiffArray()
        {
            if (m_count == 0)
            {
                return;
            }

            if (m_tradeDirection == OkexFutureTradeDirectionType.FTD_Sell)
            {
                buildBasisDiffArray(ref m_fwBasisDiffArr, m_count, m_basis + m_safeRange, m_basis + m_limitRange);
                buildBasisDiffArray(ref m_rvBasisDiffArr, m_count, m_basis - m_safeRange, m_basis - m_limitRange);
            }
            else
            {
                buildBasisDiffArray(ref m_rvBasisDiffArr, m_count, m_basis + m_safeRange, m_basis + m_limitRange);
                buildBasisDiffArray(ref m_fwBasisDiffArr, m_count, m_basis - m_safeRange, m_basis - m_limitRange);
            }
        }

        private void buildBasisDiffArray(ref OkexBasisDiffPositionData[] arr, uint count, double startVal, double endVal)
        {
            double deltaVal = endVal - startVal;
            if (count > 1)
            {
                deltaVal /= count - 1;
            }

            for (int i = 0; i < count; i++)
            {
                arr[i].basisDiff = startVal + deltaVal * i;
            }
        }

        private long getPositionByContract(OkexFutureContractType contract)
        {
            //return OkexFutureTrader.Instance.getHoldAmount(m_instrument, contract);
        }

        //private long getFreePositionByContract(OkexFutureContractType contract)
        //{
        //    List<OkexContractInfo> contracts = AccountInfo.Instance.getContractsByType(m_instrument, contract);

        //    long availablePosition = 0.0;
        //    foreach (var info in contracts)
        //    {
        //        OkexFutureContractType fc = contractTypeMap[info.contract_type];
        //        if (fc == contract)
        //        {
        //            availablePosition += info.available;
        //        }
        //    }

        //    return availablePosition;
        //}

        private long getOrderedPositionByContract(OkexFutureContractType contract, bool inOpenOrder)
        {
            List<OkexFutureOrderBriefInfo> info;
            long orderedPosition = 0;
            bool ret = OkexFutureTrader.Instance.getCurOrdersInfo(m_instrument, contract, out info);
            if (ret)
            {
                foreach(var bi in info)
                {
                    bool countIn = false;
                    if(bi.status == OkexOrderStatusType.OS_NotTraded || bi.status == OkexOrderStatusType.OS_PartiallyTraded)                       
                    {
                        if(inOpenOrder)
                        {
                            if (m_openOrders.Contains(bi.orderID))
                            {
                                countIn = true;
                            }
                        }
                        else
                        {
                            if (m_closeOrders.Contains(bi.orderID))
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

            return orderedPosition;
        }

        private double getCurPrice(OkexFutureContractType contract)
        {
            OkexFutureMarketData md = OkexFutureTrader.Instance.getMarketData(m_instrument, contract);
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

        private double getCurBuyPrice(OkexFutureContractType contract)
        {
            OkexFutureMarketData md = OkexFutureTrader.Instance.getMarketData(m_instrument, contract);
            if (md != null)
            {
                return md.buy;
            }
            return 0.0;
        }

        private double getCurSellPrice(OkexFutureContractType contract)
        {
            OkexFutureMarketData md = OkexFutureTrader.Instance.getMarketData(m_instrument, contract);
            if (md != null)
            {
                return md.sell;
            }
            return 0.0;
        }

        private uint getIndexInBDArray(ref OkexBasisDiffPositionData[] bdArr, double bd, bool asc = true)
        {
            uint index = 0;
            uint i = 0;
            if (asc)
            {
                for (i = 0; i < m_count; i++)
                {
                    if (bd < bdArr[i].basisDiff)
                    {
                        break;
                    }
                }
                index = Math.Max(0, i - 1);
            }
            else
            {
                for (i = 0; i < m_count; i++)
                {
                    if (bd > bdArr[i].basisDiff)
                    {
                        break;
                    }
                }

            }

            index = Math.Max(0, i - 1);
            index = Math.Min(index, m_count - 1);

            return index;
        }
    }
}
