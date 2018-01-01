using OkexTrader.Common;
using OkexTrader.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.FutureTrade
{
    class OkexTradeCommand
    {
        public OkexFutureInstrumentType instrument;
        public OkexFutureContractType contract;
        public double price;
        public double amount;
        public OkexTradeType tradeType;
        public uint leverRate;
        public bool matchPrice;
    }

    class FutureTradeMgr : Singleton<FutureTradeMgr>
    {
        private List<OkexTradeCommand> m_tradeCmd = new List<OkexTradeCommand>(); // todo: be optimized by pool

        public void applyTrade(OkexFutureInstrumentType instrument, OkexFutureContractType contract, double price, double amount, OkexTradeType tradeType,
                            uint leverRate = 10, bool matchPrice = false)
        {
            OkexTradeCommand cmd = new OkexTradeCommand();
            cmd.instrument = instrument;
            cmd.contract = contract;
            cmd.price = price;
            cmd.amount = amount;
            cmd.tradeType = tradeType;
            cmd.leverRate = leverRate;
            cmd.matchPrice = matchPrice;

            m_tradeCmd.Add(cmd);
        }

        private void executeTrade(OkexTradeCommand cmd)
        {
            OkexFutureTrader.Instance.trade(cmd.instrument, cmd.contract, cmd.price, cmd.amount, cmd.tradeType, cmd.leverRate, cmd.matchPrice);
        }

        public void update()
        {
            if(m_tradeCmd.Count > 0)
            {
                foreach(var cmd in m_tradeCmd)
                {
                    executeTrade(cmd);
                }

                m_tradeCmd.Clear();
            }
        }
    }
}
