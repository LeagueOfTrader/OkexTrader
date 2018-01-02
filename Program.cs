using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkexTrader.Trade;
using System.Threading;
using OkexTrader.MarketData;
using OkexTrader.FutureTrade;

namespace OkexTrader
{    

    class Program
    {
        static void output(String content)
        {
            Console.WriteLine(content);
            System.Diagnostics.Debug.WriteLine(content);
        }

        static void Main(string[] args)
        {
            //MarketDataMgr.Instance.subscribeInstrument(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter);
            //MarketDataMgr.Instance.subscribeInstrument(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_NextWeek);
            //Thread stThread = new Thread(strategyThread);
            //Thread mdThread = new Thread(marketDataThread);
            //Thread tdThread = new Thread(tradeThread);

            //mdThread.Start();
            //tdThread.Start();
            //stThread.Start();
            //FutureTradeEntity fte = new FutureTradeEntity();
            //fte.trade(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter, 5000.0, 1, OkexContractTradeType.TT_OpenSell, 10);
        }

        const int MD_FREQ = 100;
        const int TD_FREQ = 10;
        const int ST_FREQ = 100;

        static void strategyThread()
        {
            while (true)
            {
                int beginTick = System.Environment.TickCount;
                //StrategyMgr.Instance.update();
                OkexFutureDepthData dd = MarketDataMgr.Instance.getDepthData(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter);
                if (dd != null)
                {
                    output("Depth Data, deltaTime: " + (dd.receiveTimestamp - dd.sendTimestamp) + ", bid1: " + dd.bids[0].price + ", ask1: " + dd.asks[0].price);
                }
                OkexFutureMarketData md = MarketDataMgr.Instance.getMarketData(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_NextWeek);
                if(md != null)
                {
                    output("Market Data, date: " + md.timestamp + " , receive: " + md.receiveTimestamp + " , last price: " + md.last);
                }
                int deltaTick = System.Environment.TickCount - beginTick;
                if(deltaTick < ST_FREQ)
                {
                    Thread.Sleep(ST_FREQ - deltaTick);
                }
            }
        }

        static void marketDataThread()
        {
            while (true)
            {
                int beginTick = System.Environment.TickCount;
                //MarketDataMgr.Instance.update();
                int deltaTick = System.Environment.TickCount - beginTick;
                if (deltaTick < MD_FREQ)
                {
                    Thread.Sleep(MD_FREQ - deltaTick);
                }
            }
        }

        static void tradeThread()
        {
            while (true)
            {
                int beginTick = System.Environment.TickCount;
                //FutureTradeMgr.Instance.update();                
                int deltaTick = System.Environment.TickCount - beginTick;
                if (deltaTick < TD_FREQ)
                {
                    Thread.Sleep(TD_FREQ - deltaTick);
                }
            }
        }


    }
}
