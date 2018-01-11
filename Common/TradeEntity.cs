using OkexTrader.Trade;
using OkexTrader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OkexTrader.Common
{
    public enum TradeQueryResult
    {
        TQR_None,
        TQR_Failed,
        TQR_Timeout,
        TQR_Unfinished,
        TQR_Finished
    }

    abstract class TradeEntity : HttpAsyncResponser
    {
        protected long m_queryInterval = 1000;
        protected long m_orderID = 0;
        protected Timer m_resultTimer = null;
        protected Timer m_queryTimer = null;
        protected TradeQueryResult m_status = TradeQueryResult.TQR_None;
        protected bool m_valid = true;
        protected object m_lock = new object();

        protected const long TRADE_QUERY_TIMEOUT = 3000;        

        public TradeEntity(long queryInterval = 1000)
        {
            m_queryInterval = queryInterval;

            m_queryTimer = new Timer(queryInterval);
            m_resultTimer = new Timer(TRADE_QUERY_TIMEOUT);
            m_queryTimer.Elapsed += new ElapsedEventHandler(query);
            m_resultTimer.Elapsed += new ElapsedEventHandler(timeout);
        }

        public TradeQueryResult getStatus()
        {
            return m_status;
        }

        protected void query(object sender, ElapsedEventArgs e)
        {
            query();
        }

        protected void timeout(object sender, ElapsedEventArgs e)
        {
            timeout();
        }

        protected void start()
        {
            if (m_queryTimer != null)
            {
                m_queryTimer.Start();
            }
            if (m_resultTimer != null)
            {
                m_resultTimer.Start();
            }
        }

        protected void stop()
        {
            lock (m_lock)
            {
                m_valid = false;
                if (m_queryTimer != null)
                {
                    m_queryTimer.Stop();
                }
                if (m_resultTimer != null)
                {
                    m_resultTimer.Stop();
                }
            }
        }

        protected override void onResponsed(String str)
        {
            onTradeOrdered(str);
        }

        protected static TradeQueryResult getResultType(OkexOrderStatusType status)
        {
            TradeQueryResult ret = TradeQueryResult.TQR_Unfinished;
            if (status == OkexOrderStatusType.OS_AllTraded
               || status == OkexOrderStatusType.OS_Canceled)
            {
                ret = TradeQueryResult.TQR_Finished;
            }

            return ret;
        }

        abstract protected void onTradeOrdered(String str);
        abstract protected void query();
        abstract protected void timeout();
    }
}
