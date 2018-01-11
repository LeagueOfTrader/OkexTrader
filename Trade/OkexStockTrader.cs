using com.okcoin.rest.stock;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OkexTrader.Common;
using OkexTrader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.Trade
{

    class OkexStockTrader : Singleton<OkexStockTrader>
    {
        StockRestApi getRequest;// = new StockRestApi(url_prex);
        StockRestApi postRequest;// = new StockRestApi(url_prex, api_key, secret_key);

        public OkexStockTrader()
        {
            getRequest = new StockRestApi(OkexParam.url_prex);
            postRequest = new StockRestApi(OkexParam.url_prex, OkexParam.api_key, OkexParam.secret_key);
        }


        public OkexStockMarketData getStockMarketData(OkexCoinType commodityCoin, OkexCoinType currencyCoin)
        {
            string c0 = OkexDefValueConvert.getCoinName(commodityCoin);
            string c1 = OkexDefValueConvert.getCoinName(currencyCoin);
            string str = getRequest.ticker(c0 + "_" + c1);
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            OkexStockMarketData md = JsonConvert.DeserializeObject<OkexStockMarketData>(jo["ticker"].ToString());
            md.timestamp = long.Parse((string)jo["date"]);
            md.receiveTimestamp = DateUtil.getCurTimestamp();
            return md;
        }

        public OkexStockDepthData getStockDepthData(OkexCoinType commodityCoin, OkexCoinType currencyCoin, uint size = 10)
        {
            string c0 = OkexDefValueConvert.getCoinName(commodityCoin);
            string c1 = OkexDefValueConvert.getCoinName(currencyCoin);
            OkexStockDepthData dd = new OkexStockDepthData();
            dd.sendTimestamp = DateUtil.getCurTimestamp();
            string str = getRequest.depth(c0 + "_" + c1, size.ToString());
            
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            JArray bidArr = JArray.Parse(jo["bids"].ToString());
            JArray askArr = JArray.Parse(jo["asks"].ToString());
            int count = Math.Min(bidArr.Count, 10);
            for (int i = 0; i < count; i++)
            {
                JArray ordArr = JArray.Parse(bidArr[i].ToString());
                double p = (double)ordArr[0];
                double v = (double)ordArr[1];
                dd.bids[i].price = p;
                dd.bids[i].volume = v;
            }

            count = Math.Min(askArr.Count, 10);
            int last = askArr.Count - 1;
            for (int i = 0; i < count; i++)
            {
                JArray ordArr = JArray.Parse(askArr[last - i].ToString());
                double p = (double)ordArr[0];
                double v = (long)ordArr[1];
                dd.asks[i].price = p;
                dd.asks[i].volume = v;
            }
            return dd;
        }

        public bool getOrderInfoByID(OkexCoinType commodity, OkexCoinType currency, long orderID, out OkexStockOrderBriefInfo info)
        {
            string c0 = OkexDefValueConvert.getCoinName(commodity);
            string c1 = OkexDefValueConvert.getCoinName(currency);
            string symbol = c0 + "_" + c1;
            string str = postRequest.order_info(symbol, orderID.ToString());
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool ret = (bool)jo["result"];

            info = new OkexStockOrderBriefInfo();
            if (ret)
            {
                JArray arr = JArray.Parse(jo["orders"].ToString());
                foreach (var item in arr)
                {
                    info.amount = (double)item["amount"];
                    info.price = (double)item["price"];
                    info.createDate = (string)item["create_date"];
                    info.avgDealPrice = (double)item["avg_price"];
                    info.dealAmount = (double)item["deal_amount"];
                    string strType = (string)item["type"];
                    info.tradeType = OkexDefValueConvert.parseStockTradeType(strType);

                    int nStatus = int.Parse((string)item["status"]);
                    if(nStatus == 3)
                    {
                        nStatus = 4;
                    }
                    info.status = (OkexOrderStatusType)nStatus;
                    info.orderID = (long)item["order_id"];

                    info.commodity = commodity;
                    info.currency = currency;

                    break;
                }
            }
            return ret;
        }

        public void tradeAsync(OkexCoinType commodity, OkexCoinType currency,
                        OkexStockTradeType tradeType, double price, double amount, 
                        HttpAsyncReq.ResponseCallback callback)
        {
            string c0 = OkexDefValueConvert.getCoinName(commodity);
            string c1 = OkexDefValueConvert.getCoinName(currency);
            string symbol = c0 + "_" + c1;
            string strType = OkexDefValueConvert.getStockTradeTypeStr(tradeType);
            postRequest.tradeAsync(symbol, strType, price.ToString(), amount.ToString(), callback);
        }

        public void cancelAysnc(OkexCoinType commodity, OkexCoinType currency,
                        string orderID,
                        HttpAsyncReq.ResponseCallback callback)
        {
            string c0 = OkexDefValueConvert.getCoinName(commodity);
            string c1 = OkexDefValueConvert.getCoinName(currency);
            string symbol = c0 + "_" + c1;
            postRequest.cancelOrderAsync(symbol, orderID, callback);
        }
    }
}
