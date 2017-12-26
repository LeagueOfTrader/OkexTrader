using com.okcoin.rest.stock;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.Trade
{
    public enum OkexStockInstrumentType
    {
        SI_BTC,
        SI_LTC,
        SI_ETH,
        SI_ETC,
        SI_BCH
    }

    class OkexStockMarketData
    {
        private double buyPrice;
        public double buy
        {
            get { return buyPrice; }
            set { buyPrice = value; }
        }
        private double highPrice;
        public double high
        {
            get { return highPrice; }
            set { highPrice = value; }
        }
        private double sellPrice;
        public double sell
        {
            get { return sellPrice; }
            set { sellPrice = value; }
        }
        private double lowPrice;
        public double low
        {
            get { return lowPrice; }
            set { lowPrice = value; }
        }
        private double lastPrice;
        public double last
        {
            get { return lastPrice; }
            set { lastPrice = value; }
        }
        private double volume;
        public double vol
        {
            get { return volume; }
            set { volume = value; }
        }
    }

    class OkexStockTrader
    {
        StockRestApi getRequest;// = new StockRestApi(url_prex);
        StockRestApi postRequest;// = new StockRestApi(url_prex, api_key, secret_key);

        string[] instrumentsToUsdt = { "btc_usdt", "ltc_usdt", "eth_usdt",  "etc_usdt", "bch_usdt" };

        public OkexStockTrader()
        {
            getRequest = new StockRestApi(OkexParam.url_prex);
            postRequest = new StockRestApi(OkexParam.url_prex, OkexParam.api_key, OkexParam.secret_key);
        }


        public OkexStockMarketData getStockMarketDataToUsdt(OkexStockInstrumentType instrument)
        {
            string str = getRequest.ticker(instrumentsToUsdt[(int)instrument]);
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            OkexStockMarketData md = JsonConvert.DeserializeObject<OkexStockMarketData>(jo["ticker"].ToString());
            return md;
        }
    }
}
