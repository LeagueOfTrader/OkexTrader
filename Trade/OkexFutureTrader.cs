using com.okcoin.rest.future;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OkexTrader.Common;
using OkexTrader.Util;

namespace OkexTrader.Trade
{
    

    class OkexFutureTrader : Singleton<OkexFutureTrader>
    {     
        FutureRestApiV1 getRequest;
        FutureRestApiV1 postRequest;

        //string[] contractTypeName = { "this_week", "next_week", "quarter" };
        //string[] instrumentQuotationName = { "btc_usd", "ltc_usd", "eth_usd", "etc_usd" , "bch_usd" };
        //string[] coinName = { "btc", "ltc", "eth", "etc", "bch" };

        //string[] kLineTypeName = {"1min", "3min","5min", "15min", "30min", "1hour", "2hour", "4hour", "6hour", "12hour", "day", "3day", "week"};

        public OkexFutureTrader()
        {
            getRequest = new FutureRestApiV1(OkexParam.url_prex);
            postRequest = new FutureRestApiV1(OkexParam.url_prex, OkexParam.api_key, OkexParam.secret_key);
        }

        // 行情
        public OkexFutureMarketData getMarketData(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            string str = getRequest.future_ticker(OkexDefValueConvert.getInstrumentStr(instrument), OkexDefValueConvert.getContractTypeStr(contract));
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            //if (!futureData.ContainsKey(instrument))
            //{
            //    OkexFutureData fd = new OkexFutureData();
            //    futureData.Add(instrument, fd);
            //}

            //if (!futureData[instrument].marketData.ContainsKey(contract))
            //{
            //    futureData[instrument].marketData.Add
            //}
            OkexFutureMarketData md = new OkexFutureMarketData();
            md = (OkexFutureMarketData)JsonConvert.DeserializeObject(jo["ticker"].ToString(), typeof(OkexFutureMarketData));
            md.timestamp = long.Parse((string)jo["date"]);
            md.receiveTimestamp = DateUtil.getCurTimestamp(); //System.Environment.TickCount;
            return md;
        }

        // 盘口信息
        public OkexFutureDepthData getMarketDepthData(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            OkexFutureDepthData dd = new OkexFutureDepthData();
            dd.sendTimestamp = DateUtil.getCurTimestamp();//System.Environment.TickCount;
            string str = getRequest.future_depth(OkexDefValueConvert.getInstrumentStr(instrument), OkexDefValueConvert.getContractTypeStr(contract));
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            JArray bidArr = JArray.Parse(jo["bids"].ToString());
            JArray askArr = JArray.Parse(jo["asks"].ToString());
            for (int i = 0; i < 5; i++)
            {
                JArray ordArr = JArray.Parse(bidArr[i].ToString());
                double p = (double)ordArr[0];
                long v = (long)ordArr[1];
                dd.bids[i].price = p;
                dd.bids[i].volume = v;

                ordArr = JArray.Parse(askArr[i].ToString());
                p = (double)ordArr[0];
                v = (long)ordArr[1];
                dd.asks[i].price = p;
                dd.asks[i].volume = v;
            }
            return dd;
        }

        // 成交信息
        public List<OkexFutureTradeInfo> getTradesInfo(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            List<OkexFutureTradeInfo> trades = new List<OkexFutureTradeInfo>();
            string str = getRequest.future_trades(OkexDefValueConvert.getInstrumentStr(instrument), OkexDefValueConvert.getContractTypeStr(contract));
            JArray arr = JArray.Parse(str);
            
            foreach(var item in arr)
            {
                OkexFutureTradeInfo ti = (OkexFutureTradeInfo)JsonConvert.DeserializeObject(item.ToString(), typeof(OkexFutureTradeInfo));
                trades.Add(ti);
            }

            return trades;
        }

        // 期货指数
        public double getFutureIndex(OkexFutureInstrumentType instrument)
        {
            string str = getRequest.future_index(OkexDefValueConvert.getInstrumentStr(instrument));
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            double futureIdx = (double)jo["future_index"];
            return futureIdx;
        }

        // 美元汇率
        public double getExchangeRate()
        {
            string str = getRequest.exchange_rate();
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            double er = (double)jo["rate"];
            return er;
        }

        // always return 0
        public double getEstimatePrice(OkexFutureInstrumentType instrument)
        {
            string str = getRequest.future_estimated_price(OkexDefValueConvert.getInstrumentStr(instrument));

            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            double ep = (double)jo["forecast_price"];
            return ep;
        }

        // K线
        public List<OkexKLineData> getKLineData(OkexFutureInstrumentType instrument, OkexFutureContractType contract, OkexKLineType klType)
        {
            List<OkexKLineData> kLines = new List<OkexKLineData>();
            string str = getRequest.future_kline(OkexDefValueConvert.getInstrumentStr(instrument), 
                                                    OkexDefValueConvert.getKLineTypeStr(klType), 
                                                    OkexDefValueConvert.getContractTypeStr(contract), "", "");
            JArray arr = JArray.Parse(str);

            foreach (var item in arr)
            {
                JArray klArr = JArray.Parse(item.ToString());
                OkexKLineData kld = new OkexKLineData();
                kld.timestamp = (long)klArr[0];
                kld.open = (double)klArr[1];
                kld.high = (double)klArr[2];
                kld.low = (double)klArr[3];
                kld.close = (double)klArr[4];
                kld.volume = (long)klArr[5];
                kld.refValue = (double)klArr[6];
                kLines.Add(kld);
            }

            return kLines;
        }

        // 获取当前可用合约总持仓量
        public long getMarketHoldAmount(OkexFutureInstrumentType instrument, OkexFutureContractType contract)
        {
            string str = getRequest.future_hold_amount(OkexDefValueConvert.getInstrumentStr(instrument), OkexDefValueConvert.getContractTypeStr(contract));
            JArray arr = JArray.Parse(str);
            JObject jo = (JObject)JsonConvert.DeserializeObject(arr[0].ToString());
            long amount = (long)jo["amount"];
            return amount;
        }

        // 账户信息
        public bool getUserInfo(out Dictionary<OkexFutureInstrumentType, OkexAccountInfo> info)
        {
            string str = postRequest.future_userinfo_4fix();
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool result = (bool)jo["result"];

            info = new Dictionary<OkexFutureInstrumentType, OkexAccountInfo>();
            
            if (result == true)
            {
                for (int i = 0; i <= (int)OkexFutureInstrumentType.FI_BCH; i++)
                {
                    OkexFutureInstrumentType fi = (OkexFutureInstrumentType)i;
                    string instrumentName = OkexDefValueConvert.getCoinName(fi);
                    OkexAccountInfo ai = new OkexAccountInfo();
                    ai.balance = (double)jo["info"][instrumentName]["balance"];
                    ai.rights = (double)jo["info"][instrumentName]["rights"];
                    JArray arr = JArray.Parse(jo["info"][instrumentName]["contracts"].ToString());
                    foreach (var item in arr)
                    {
                        OkexContractInfo ci = (OkexContractInfo)JsonConvert.DeserializeObject(item.ToString(), typeof(OkexContractInfo));
                        ai.contractsInfo.Add(ci);
                    }
                    info.Add(fi, ai);
                }
            }
            return result;
        }

        // 仓位信息
        public bool getFuturePosition(OkexFutureInstrumentType instrument, OkexFutureContractType contract, out List<OkexPositionInfo> info)
        {
            string str = postRequest.future_position_4fix(OkexDefValueConvert.getInstrumentStr(instrument), OkexDefValueConvert.getContractTypeStr(contract));
            info = new List<OkexPositionInfo>();

            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool result = (bool)jo["result"];
            if (result)
            {
                JArray arr = JArray.Parse(jo["holding"].ToString());
                foreach(var item in arr)
                {
                    OkexPositionInfo pi = JsonConvert.DeserializeObject<OkexPositionInfo>(item.ToString());
                    info.Add(pi);
                }
            }

            return result;
        }

        // extension for position holding query
        public long getHoldPositionAmount(OkexFutureInstrumentType instrument, OkexFutureContractType contract,
                                    OkexFutureTradeDirectionType direction)
        {
            long positionAmount = 0;
            List<OkexPositionInfo> info;
            bool hold = getFuturePosition(instrument, contract, out info);
            if (hold)
            {
                foreach (var pi in info)
                {
                    OkexFutureContractType ct = OkexDefValueConvert.parseContractType(pi.contract_type);
                    if (ct == contract)
                    {
                        OkexPositionBriefInfo bi = new OkexPositionBriefInfo(pi, instrument, contract, direction);
                        if (bi.amount > 0)
                        {
                            positionAmount += bi.amount;
                            break;
                        }
                    }
                }
            }

            return positionAmount;
        }

        public OkexPositionBriefInfo getHoldPosition(OkexFutureInstrumentType instrument, OkexFutureContractType contract, 
                                    OkexFutureTradeDirectionType direction, uint leverRate)
        {
            OkexPositionBriefInfo briefInfo = null;
            List<OkexPositionInfo> info;
            bool hold = getFuturePosition(instrument, contract, out info);
            if (hold)
            {
                foreach(var pi in info)
                {
                    OkexFutureContractType ct = OkexDefValueConvert.parseContractType(pi.contract_type);
                    if (ct == contract && leverRate == pi.lever_rate)
                    {
                        OkexPositionBriefInfo bi = new OkexPositionBriefInfo(pi, instrument, contract, direction);
                        if(bi.amount > 0)
                        {
                            briefInfo = bi;
                            break;
                        }
                    }
                }
            }

            return briefInfo;
        }

        // 交易 开仓平仓
        public long trade(OkexFutureInstrumentType instrument, OkexFutureContractType contract, double price, long amount, OkexContractTradeType tradeType, 
                            uint leverRate = 10, bool matchPrice = false)
        {
            string strMatchPrice = "";
            if (matchPrice)
            {
                strMatchPrice = "1";
            }
            else
            {
                strMatchPrice = "0";
            }

            if(leverRate != 10 && leverRate != 20)
            {
                leverRate = 10;
            }

            uint nType = (uint)tradeType;
            string str = postRequest.future_trade_ex(OkexDefValueConvert.getInstrumentStr(instrument), OkexDefValueConvert.getContractTypeStr(contract), price.ToString(), amount.ToString(), nType.ToString(),
                                        strMatchPrice, leverRate.ToString());
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool ret = (bool)jo["result"];
            if (!ret)
            {
                return 0;
            }

            long orderID = (long)jo["order_id"];
            return orderID;
        }

        public void tradeAsync(OkexFutureInstrumentType instrument, OkexFutureContractType contract, double price, long amount, OkexContractTradeType tradeType,
                           HttpAsyncReq.ResponseCallback callback, uint leverRate = 10, bool matchPrice = false)
        {
            string strMatchPrice = "";
            if (matchPrice)
            {
                strMatchPrice = "1";
            }
            else
            {
                strMatchPrice = "0";
            }

            if (leverRate != 10 && leverRate != 20)
            {
                leverRate = 10;
            }

            uint nType = (uint)tradeType;
            postRequest.future_async_trade_ex(OkexDefValueConvert.getInstrumentStr(instrument), OkexDefValueConvert.getContractTypeStr(contract), 
                                        price.ToString(), amount.ToString(), nType.ToString(),
                                        strMatchPrice, leverRate.ToString(), callback);
        }

        public bool cancel(OkexFutureInstrumentType instrument, OkexFutureContractType contract, long orderID)
        {
            string str = postRequest.future_cancel(OkexDefValueConvert.getInstrumentStr(instrument), OkexDefValueConvert.getContractTypeStr(contract), orderID.ToString());
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool ret = (bool)jo["result"];
            
            return ret;
        }

        public void cancelAsync(OkexFutureInstrumentType instrument, OkexFutureContractType contract, long orderID, HttpAsyncReq.ResponseCallback callback)
        {
            postRequest.future_cancel_async(OkexDefValueConvert.getInstrumentStr(instrument), OkexDefValueConvert.getContractTypeStr(contract), 
                                            orderID.ToString(), callback);
        }

        public bool getCurOrdersInfo(OkexFutureInstrumentType instrument, OkexFutureContractType contract, 
                                    out List<OkexFutureOrderBriefInfo> briefInfo, bool finished = false)
        {
            List<OkexFutureOrderBriefInfo> ordersBriefInfo = new List<OkexFutureOrderBriefInfo>();
            string strFinished = "1";
            if (finished)
            {
                strFinished = "2";
            }
            string str = postRequest.future_order_info(OkexDefValueConvert.getInstrumentStr(instrument), OkexDefValueConvert.getContractTypeStr(contract), "-1", strFinished, "0", "1");

            briefInfo = new List<OkexFutureOrderBriefInfo>();
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool ret = (bool)jo["result"];

            if (ret)
            {
                JArray arr = JArray.Parse(jo["orders"].ToString());
                foreach(var item in arr)
                {
                    OkexFutureOrderBriefInfo obi = new OkexFutureOrderBriefInfo();
                    obi.amount = (long)item["amount"];
                    obi.contractName = (string)item["contract_name"];
                    obi.leverRate = (int)item["lever_rate"];
                    obi.price = (double)item["price"];
                    obi.tradeType = (OkexContractTradeType)int.Parse((string)item["type"]);
                    obi.status = (OkexOrderStatusType)int.Parse((string)item["status"]);
                    obi.orderID = (long)item["order_id"];
                    briefInfo.Add(obi);
                }
            }

            return ret;
        }

        public bool getOrderInfoByID(OkexFutureInstrumentType instrument, OkexFutureContractType contract,
                                     long orderID, out OkexFutureOrderBriefInfo info)
        {
            List<OkexFutureOrderBriefInfo> ordersBriefInfo = new List<OkexFutureOrderBriefInfo>();
            string str = postRequest.future_order_info(OkexDefValueConvert.getInstrumentStr(instrument), OkexDefValueConvert.getContractTypeStr(contract), orderID.ToString(), "1", "0", "1");

            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            bool ret = (bool)jo["result"];

            info = new OkexFutureOrderBriefInfo();
            if (ret)
            {
                JArray arr = JArray.Parse(jo["orders"].ToString());
                foreach (var item in arr)
                {
                    info.amount = (long)item["amount"];
                    info.contractName = (string)item["contract_name"];
                    info.leverRate = (int)item["lever_rate"];
                    info.price = (double)item["price"];
                    info.tradeType = (OkexContractTradeType)int.Parse((string)item["type"]);
                    info.status = (OkexOrderStatusType)int.Parse((string)item["status"]);
                    info.orderID = (long)item["order_id"];
                    break;
                }
            }

            return ret;
        }
    }
}
