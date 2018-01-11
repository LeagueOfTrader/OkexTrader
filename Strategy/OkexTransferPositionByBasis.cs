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

    class OkexTransferPositionByBasis : OkexBasicStrategy
    {
        public class OkexBasisDiffPositionData
        {
            public double basisDiff = 0.0;
            public long spotContractPosition = 0;
            public long forwardContractPosition = 0;
        }        
               

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
        private long m_totalPosition = 0;
        private double m_ratio = 0.0;
        private double m_diff = 0.0;
        private OkexBasisDiffPositionData[] m_fwBasisDiffArr = null;
        private OkexBasisDiffPositionData[] m_rvBasisDiffArr = null;

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
            m_totalPosition = getPositionByContract(m_instrument, m_spotContract, m_tradeDirection) 
                                + getPositionByContract(m_instrument, m_forwardContract, m_tradeDirection);

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
        public void setTotalPosition(long total)
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
            long avgPosition = m_totalPosition / 2;
            double accumVal = m_ratio;
            for (int i = 0; i < m_count; i++)
            {
                m_fwBasisDiffArr[i].spotContractPosition = (long)((1.0 - accumVal) * (double)avgPosition);
                m_fwBasisDiffArr[i].forwardContractPosition = m_totalPosition - m_fwBasisDiffArr[i].spotContractPosition;

                m_rvBasisDiffArr[i].forwardContractPosition = (long)((1.0 - accumVal) * (double)avgPosition);
                m_rvBasisDiffArr[i].spotContractPosition = m_totalPosition - m_rvBasisDiffArr[i].forwardContractPosition;

                accumVal += m_ratio;
            }
        }

        private void initWithDiff() // 每格档固定值
        {
            long avgPosition = m_totalPosition / 2;
            long accumVal = (long)m_diff;
            for (int i = 0; i < m_count; i++)
            {
                m_fwBasisDiffArr[i].spotContractPosition = avgPosition - accumVal;
                m_fwBasisDiffArr[i].forwardContractPosition = m_totalPosition - m_fwBasisDiffArr[i].spotContractPosition;
               
                m_rvBasisDiffArr[i].forwardContractPosition = avgPosition - accumVal;
                m_rvBasisDiffArr[i].spotContractPosition = m_totalPosition - m_rvBasisDiffArr[i].forwardContractPosition;

                accumVal += (long)m_diff;
            }
        }

        public override void update()
        {
            double spotPrice = getCurPrice(m_instrument, m_spotContract);
            double forwardPrice = getCurPrice(m_instrument, m_forwardContract);

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
        }

        private void tryTransferToForward(double basisDiff)
        {
            double spotPosition = getPositionByContract(m_instrument, m_spotContract, m_tradeDirection);
            double forwardPosition = getPositionByContract(m_instrument, m_forwardContract, m_tradeDirection);
            double avgPosition = (spotPosition + forwardPosition) / 2.0;

            bool asc = true;
            if (m_tradeDirection == OkexFutureTradeDirectionType.FTD_Buy)
            {
                asc = false;
            }
            uint index = getIndexInBDArray(ref m_fwBasisDiffArr, basisDiff, asc);

            transferToTarget(m_fwBasisDiffArr[index].spotContractPosition, m_fwBasisDiffArr[index].forwardContractPosition, 
                            m_spotContract, m_forwardContract);
        }

        private void transferToTarget(long targetFromPosition, long targetToPosition, 
                                OkexFutureContractType fromContract, OkexFutureContractType toContract)
        {            
            long curFromPosition = getAvailablePositionByContract(m_instrument, fromContract, m_tradeDirection);// - getOrderedPositionByContract(m_spotContract, true);
            //long curTargetForwardPosition = getPositionByContract(m_instrument, toContract, m_tradeDirection) 
            //                                + getOrderedPositionByContract(m_instrument, toContract, m_tradeDirection, true);

            if(curFromPosition > targetFromPosition)
            {
                long targetVol = curFromPosition - targetFromPosition;
                OkexFutureDepthData fromDD = OkexFutureTrader.Instance.getMarketDepthData(m_instrument, fromContract);
                OkexFutureDepthData toDD = OkexFutureTrader.Instance.getMarketDepthData(m_instrument, toContract);
                if(m_tradeDirection == OkexFutureTradeDirectionType.FTD_Sell)
                {
                    long bidVol = fromDD.bids[0].volume;
                    long askVol = toDD.asks[0].volume;
                    long vol = Math.Min(bidVol, askVol);
                    vol = Math.Min(vol, targetVol);

                    //trade(m_instrument, fromContract, fromDD.bids[0].price, vol, OkexContractTradeType.TT_CloseBuy);
                    //trade(m_instrument, toContract, toDD.asks[0].price, vol, OkexContractTradeType.TT_OpenSell);
                }
                else
                {
                    long askVol = fromDD.asks[0].volume;
                    long bidVol = toDD.asks[0].volume;
                    long vol = Math.Min(askVol, bidVol);
                    vol = Math.Min(vol, targetVol);

                    //trade(m_instrument, fromContract, fromDD.asks[0].price, vol, OkexContractTradeType.TT_CloseSell);
                    //trade(m_instrument, toContract, toDD.bids[0].price, vol, OkexContractTradeType.TT_OpenBuy);
                }
            }

        }

        private void tryTransferToSpot(double basisDiff)
        {
            double spotPosition = getPositionByContract(m_instrument, m_spotContract, m_tradeDirection);
            double forwardPosition = getPositionByContract(m_instrument, m_forwardContract, m_tradeDirection);
            double avgPosition = (spotPosition + forwardPosition) / 2.0;

            bool asc = true;
            if (m_tradeDirection == OkexFutureTradeDirectionType.FTD_Buy)
            {
                asc = false;
            }
            uint index = getIndexInBDArray(ref m_rvBasisDiffArr, basisDiff, asc);

            transferToTarget(m_rvBasisDiffArr[index].forwardContractPosition, m_rvBasisDiffArr[index].spotContractPosition,
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
                arr[i] = new OkexBasisDiffPositionData();
                arr[i].basisDiff = startVal + deltaVal * i;
            }
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
