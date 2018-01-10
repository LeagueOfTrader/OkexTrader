using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkexTrader.Trade;
using System.Threading;
using OkexTrader.MarketData;
using OkexTrader.FutureTrade;
using OkexTrader.Strategy;

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
            MarketDataMgr.Instance.subscribeInstrument(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter);
            MarketDataMgr.Instance.subscribeInstrument(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_NextWeek);
            StockDataMgr.Instance.subscribeInstrument(OkexCoinType.CT_BCH, OkexCoinType.CT_USDT);

            //StrategyMgr.Instance.init();
            Thread stThread = new Thread(strategyThread);
            //Thread mdThread = new Thread(marketDataThread);
            //Thread tdThread = new Thread(tradeThread);

            //mdThread.Start();
            //tdThread.Start();
            stThread.Start();

            //OkexBasicStrategy s = new OkexBasicStrategy();
            //s.init();
            //s.trade(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter, 5000.0, 1, OkexContractTradeType.TT_OpenSell, 10);
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
                //OkexFutureDepthData dd = MarketDataMgr.Instance.getDepthDataWithTimeLimit(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_Quarter, 80);
                //if (dd != null)
                //{
                //    output("Depth Data, deltaTime: " + (dd.receiveTimestamp - dd.sendTimestamp) + ", bid1: " + dd.bids[0].price + ", ask1: " + dd.asks[0].price);
                //}
                //OkexFutureMarketData md = MarketDataMgr.Instance.getMarketDataWithTimeLimit(OkexFutureInstrumentType.FI_LTC, OkexFutureContractType.FC_NextWeek, 50);
                //if (md != null)
                //{
                //    output("Market Data, date: " + md.timestamp + " , receive: " + md.receiveTimestamp + " , last price: " + md.last);
                //}


                OkexStockDepthData dd = StockDataMgr.Instance.getDepthData(OkexCoinType.CT_BCH, OkexCoinType.CT_USDT);
                if (dd != null)
                {
                    output("Depth Data, deltaTime: " + (dd.receiveTimestamp - dd.sendTimestamp) + ", bid1: " + dd.bids[0].price + ", ask1: " + dd.asks[0].price);
                }
                OkexStockMarketData md = StockDataMgr.Instance.getMarketData(OkexCoinType.CT_BCH, OkexCoinType.CT_USDT);
                if (md != null)
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
