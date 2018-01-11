using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.okcoin.rest;
using OkexTrader.Util;

namespace com.okcoin.rest.future
{
    /// <summary>
    /// 新版本期货 REST API实现
    /// </summary>
    class FutureRestApiV1 : IFutureRestApi
    {
        /// <summary>
        /// OKCoin申请的secretKey
        /// </summary>
        private String secret_key;
        /// <summary>
        /// OKCoin申请的apiKey
        /// </summary>
        private String api_key;
        /// <summary>
        /// 请求URL 国际站https://www.okcoin.com ; 国内站https://www.okcoin.cn
        /// </summary>
        private String url_prex;

        public FutureRestApiV1(String url_prex, String api_key, String secret_key)
        {
            this.api_key = api_key;
            this.secret_key = secret_key;
            this.url_prex = url_prex;
        }

        public FutureRestApiV1(String url_prex)
        {
            this.url_prex = url_prex;

        }

        /// <summary>
        /// 期货行情URL
        /// </summary>
        private const String FUTURE_TICKER_URL = "/api/v1/future_ticker.do";
        /// <summary>
        /// 期货指数查询URL
        /// </summary>
        private const String FUTURE_INDEX_URL = "/api/v1/future_index.do";
        /// <summary>
        /// 期货交易记录查询URL
        /// </summary>
        private const String FUTURE_TRADES_URL = "/api/v1/future_trades.do";
        /// <summary>
        /// 期货市场深度查询URL
        /// </summary>
        private const String FUTURE_DEPTH_URL = "/api/v1/future_depth.do";
        /// <summary>
        /// 美元-人民币汇率查询URL
        /// </summary>
        private const String FUTURE_EXCHANGE_RATE_URL = "/api/v1/exchange_rate.do";

        /// <summary>
        /// 期货取消订单URL
        /// </summary>
        private const String FUTURE_CANCEL_URL = "/api/v1/future_cancel.do";

        /// <summary>
        /// 期货下单URL
        /// </summary>
        private const String FUTURE_TRADE_URL = "/api/v1/future_trade.do";

        /// <summary>
        /// 期货账户信息URL
        /// </summary>
        private const String FUTURE_USERINFO_URL = "/api/v1/future_userinfo.do";

        /// <summary>
        /// 逐仓期货账户信息URL
        /// </summary>
        private const String FUTURE_USERINFO_4FIX_URL = "/api/v1/future_userinfo_4fix.do";

        /// <summary>
        /// 期货持仓查询URL
        /// </summary>
        private const String FUTURE_POSITION_URL = "/api/v1/future_position.do";

        /// <summary>
        /// 期货逐仓持仓查询URL
        /// </summary>
        private const String FUTURE_POSITION_4FIX_URL = "/api/v1/future_position_4fix.do";

        /// <summary>
        /// 期货逐仓持仓查询URL
        /// </summary>
        private const String FUTURE_ORDER_INFO_URL = "/api/v1/future_order_info.do";
        /// <summary>
        /// 批量下单的URL
        /// </summary>
        private const String FUTURE_BATCH_TRADE_URL = "/api/v1/future_batch_trade.do";
        /// <summary>
        /// 获取交割预估价的URL
        /// </summary>
        private const String FUTURE_ESTIMATED_PRICE_URL = "/api/v1/future_estimated_price.do";
        /// <summary>
        /// 获取期货合约的K线数据的URL
        /// </summary>
        private const String FUTURE_KLINE_URL = "/api/v1/future_kline.do";
        /// <summary>
        /// 获取当前可用合约总持仓量的URL
        /// </summary>
        private const String FUTURE_HOLD_AMOUNT_URL = "/api/v1/future_hold_amount.do";
        /// <summary>
        /// 获取期货交易历史的URL
        /// </summary>
        private const String FUTURE_TRADES_HISTORY_URL = "api/v1/future_trades_history.do";
        /// <summary>
        /// 批量获取期货订单信息的URL
        /// </summary>
        private const String FUTURE_ORDERS_INFO_URL = "/api/v1/future_orders_info.do";
        /// <summary>
        ///  获取期货爆仓单的URL
        /// </summary>
        private const String FUTURE_EXPLOSIVE_URL = "/api/v1/future_explosive.do";

        private const String FUTURE_DEVOLVE_URL = "/api/v1/future_devolve.do";

        /// <summary>
        /// 获取OKCoin最新市场期货行情数据 
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币 </param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <returns></returns>
        public String future_ticker(String symbol, String contractType)
        {
            String result = "";
            try
            {
                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                String param = "";
                if (!StringUtil.isEmpty(symbol))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "symbol=" + symbol;
                }
                if (!StringUtil.isEmpty(contractType))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "contract_type=" + contractType;

                }
                result = httpUtil.requestHttpGet(url_prex, FUTURE_TICKER_URL, param);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 期货指数
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <returns></returns>
        public String future_index(String symbol)
        {
            String result = "";
            try
            {
                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                String param = "";
                if (!StringUtil.isEmpty(symbol))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "symbol=" + symbol;
                }
                result = httpUtil.requestHttpGet(url_prex, FUTURE_INDEX_URL, param);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 期货交易记录
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType"> 合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <returns></returns>
        public String future_trades(String symbol, String contractType)
        {
            String result = "";
            try
            {
                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                String param = "";
                if (!StringUtil.isEmpty(symbol))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "symbol=" + symbol;
                }
                if (!StringUtil.isEmpty(contractType))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "contract_type=" + contractType;

                }
                result = httpUtil.requestHttpGet(url_prex, FUTURE_TRADES_URL, param);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 期货深度
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <returns></returns>
        public String future_depth(String symbol, String contractType)
        {
            String result = "";

            try
            {
                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                String param = "";
                if (!StringUtil.isEmpty(symbol))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "symbol=" + symbol;
                }
                if (!StringUtil.isEmpty(contractType))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "contract_type=" + contractType;

                }
                result = httpUtil.requestHttpGet(url_prex, FUTURE_DEPTH_URL, param);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 汇率查询
        /// </summary>
        /// <returns></returns>
        public String exchange_rate()
        {
            String result = "";
            try
            {
                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                result = httpUtil.requestHttpGet(url_prex, FUTURE_EXCHANGE_RATE_URL, null);
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }
        /// <summary>
        ///  取消订单
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <param name="orderId">订单ID</param>
        /// <returns></returns>
        public String future_cancel(String symbol, String contractType,
            String orderId)
        {
            String result = "";
            try
            {  // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();

                if (!StringUtil.isEmpty(contractType))
                {
                    paras.Add("contract_type", contractType);
                }
                if (!StringUtil.isEmpty(orderId))
                {
                    paras.Add("order_id", orderId);
                }
                if (!StringUtil.isEmpty(api_key))
                {
                    paras.Add("api_key", api_key);
                }
                if (!StringUtil.isEmpty(symbol))
                {
                    paras.Add("symbol", symbol);
                }
                String sign = MD5Util.buildMysignV1(paras, secret_key);

                paras.Add("sign", sign);
                // 发送post请求

                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                result = httpUtil.requestHttpPost(url_prex, FUTURE_CANCEL_URL, paras);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 批量下单
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contract_type">合约类型: this_week:当周   next_week:下周   quarter:季度</param>
        /// <param name="orders_data">JSON类型的字符串 例：[{price:5,amount:2,type:1,match_price:1},{price:2,amount:3,type:1,match_price:1}] 最大下单量为5，price,amount,type,match_price参数参考future_trade接口中的说明 </param>
        ///<param name="lever_rate">杠杆倍数 value:10\20 默认10</param>
        /// <returns></returns>
        public String future_batch_trade(String symbol, String contract_type, String orders_data, String lever_rate)
        {
            String result = "";
            try
            {
                // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();
                paras.Add("api_key", api_key);
                if (!StringUtil.isEmpty(symbol))
                {
                    paras.Add("symbol", symbol);
                }
                if (!StringUtil.isEmpty(contract_type))
                {
                    paras.Add("contract_type", contract_type);
                }
                if (!StringUtil.isEmpty(orders_data))
                {
                    paras.Add("orders_data", orders_data);
                }
                if (!StringUtil.isEmpty(lever_rate))
                {
                    paras.Add("lever_rate", lever_rate);
                }
                String sign = MD5Util.buildMysignV1(paras, secret_key);

                paras.Add("sign", sign);
                // 发送post请求

                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                result = httpUtil.requestHttpPost(url_prex, FUTURE_BATCH_TRADE_URL, paras);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 期货下单
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <param name="price">价格</param>
        /// <param name="amount">委托数量</param>
        /// <param name="type">1:开多   2:开空   3:平多   4:平空</param>
        /// <param name="matchPrice">是否为对手价 0:不是    1:是   ,当取值为1时,price无效</param>
        /// <returns></returns>
        public String future_trade(String symbol, String contractType,
            String price, String amount, String type, String matchPrice)
        {
            String result = "";
            try
            {  // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();
                if (!StringUtil.isEmpty(symbol))
                {
                    paras.Add("symbol", symbol);
                }
                if (!StringUtil.isEmpty(contractType))
                {
                    paras.Add("contract_type", contractType);
                }
                if (!StringUtil.isEmpty(api_key))
                {
                    paras.Add("api_key", api_key);
                }
                if (!StringUtil.isEmpty(price))
                {
                    paras.Add("price", price);
                }
                if (!StringUtil.isEmpty(amount))
                {
                    paras.Add("amount", amount);
                }
                if (!StringUtil.isEmpty(type))
                {
                    paras.Add("type", type);
                }
                if (!StringUtil.isEmpty(matchPrice))
                {
                    paras.Add("match_price", matchPrice);
                }
                String sign = MD5Util.buildMysignV1(paras, secret_key);
                paras.Add("sign", sign);
                // 发送post请求

                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                result = httpUtil.requestHttpPost(url_prex, FUTURE_TRADE_URL, paras);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 期货账户信息
        /// </summary>
        /// <returns></returns>
        public String future_userinfo()
        {
            String result = "";
            try
            { // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();

                paras.Add("api_key", api_key);

                String sign = MD5Util.buildMysignV1(paras, this.secret_key);
                paras.Add("sign", sign);


                // 发送post请求

                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                result = httpUtil.requestHttpPost(url_prex, FUTURE_USERINFO_URL,
                       paras);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 期货逐仓账户信息
        /// </summary>
        /// <returns></returns>
        public String future_userinfo_4fix()
        {
            String result = "";
            try
            { // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();
                paras.Add("api_key", api_key);
                String sign = MD5Util.buildMysignV1(paras, this.secret_key);
                paras.Add("sign", sign);
                // 发送post请求
                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                result = httpUtil.requestHttpPost(url_prex, FUTURE_USERINFO_4FIX_URL,
                       paras);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 用户持仓查询
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <returns></returns>
        public String future_position(String symbol, String contractType)
        {
            String result = "";
            try
            { // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();
                if (!StringUtil.isEmpty(symbol))
                {
                    paras.Add("symbol", symbol);
                }
                if (!StringUtil.isEmpty(contractType))
                {
                    paras.Add("contract_type", contractType);
                }
                paras.Add("api_key", api_key);
                String sign = MD5Util.buildMysignV1(paras, secret_key);
                paras.Add("sign", sign);
                // 发送post请求

                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                result = httpUtil.requestHttpPost(url_prex, FUTURE_POSITION_URL,
                       paras);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;

        }
        /// <summary>
        /// 用户逐仓持仓查询
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <returns></returns>
        public String future_position_4fix(String symbol, String contractType)
        {
            String result = "";
            try
            {
                // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();
                if (!StringUtil.isEmpty(symbol))
                {
                    paras.Add("symbol", symbol);
                }
                if (!StringUtil.isEmpty(contractType))
                {
                    paras.Add("contract_type", contractType);
                }
                paras.Add("api_key", api_key);
                String sign = MD5Util.buildMysignV1(paras, secret_key);
                paras.Add("sign", sign);
                // 发送post请求
                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                result = httpUtil.requestHttpPost(url_prex, FUTURE_POSITION_4FIX_URL,
                       paras);
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }
        /// <summary>
        /// 获取用户订单信息
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <param name="orderId">订单ID(-1查询全部未成交订单，否则查询相应单号的订单)</param>
        /// <param name="status">查询状态：1:未完成(最近七天的数据)  2:已完成(最近七天的数据)</param>
        /// <param name="currentPage">当前页数</param>
        /// <param name="pageLength">每页获取条数，最多不超过50</param>
        /// <returns></returns>
        public String future_order_info(String symbol, String contractType,
            String orderId, String status, String currentPage = null, String pageLength = null)
        {
            String result = "";
            try
            {
                // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();
                if (!StringUtil.isEmpty(contractType))
                {
                    paras.Add("contract_type", contractType);
                }
                if (!StringUtil.isEmpty(currentPage))
                {
                    paras.Add("current_page", currentPage);
                }
                if (!StringUtil.isEmpty(orderId))
                {
                    paras.Add("order_id", orderId);
                }
                if (!StringUtil.isEmpty(api_key))
                {
                    paras.Add("api_key", api_key);
                }
                if (!StringUtil.isEmpty(pageLength))
                {
                    paras.Add("page_length", pageLength);
                }
                if (!StringUtil.isEmpty(symbol))
                {
                    paras.Add("symbol", symbol);
                }
                if (!StringUtil.isEmpty(status))
                {
                    paras.Add("status", status);
                }
                String sign = MD5Util.buildMysignV1(paras, secret_key);
                paras.Add("sign", sign);
                // 发送post请求

                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                result = httpUtil.requestHttpPost(url_prex, FUTURE_ORDER_INFO_URL,
                       paras);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 获取交割预估价
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <returns></returns>
        public String future_estimated_price(String symbol)
        {
            String result = "";
            try
            {
                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                String param = "";
                if (!StringUtil.isEmpty(symbol))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "symbol=" + symbol;
                }
                result = httpUtil.requestHttpGet(url_prex, FUTURE_ESTIMATED_PRICE_URL, param);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 获取期货合约的K线数据
        /// </summary>
        /// <param name="symbol">btc_usd：比特币， ltc_usd：莱特币 </param>
        /// <param name="type">
        /// 1min : 1分钟
        ///3min : 3分钟
        ///5min : 5分钟
        ///15min : 15分钟
        /// 30min : 30分钟
        ///1day : 1日
        ///3day : 3日
        ///1week : 1周
        ///1hour : 1小时
        ///2hour : 2小时
        ///4hour : 4小时
        ///6hour : 6小时
        ///12hour : 12小时
        /// </param>
        /// <param name="contract_type">合约类型。this_week：当周；next_week：下周；quarter：季度</param>
        /// <param name="size">指定获取数据的条数</param>
        /// <param name="since">时间戳（eg：1417536000000）。 返回该时间戳以后的数据</param>
        /// <returns></returns>
        public String future_kline(String symbol, String type, String contract_type, String size, String since)
        {
            String result = "";
            try
            {
                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                String param = "";
                if (!StringUtil.isEmpty(symbol))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "symbol=" + symbol;
                }
                if (!StringUtil.isEmpty(type))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "type=" + type;
                }
                if (!StringUtil.isEmpty(contract_type))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "contract_type=" + contract_type;
                }
                if (!StringUtil.isEmpty(size))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "size=" + size;
                }
                if (!StringUtil.isEmpty(since))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "since=" + since;
                }
                result = httpUtil.requestHttpGet(url_prex, FUTURE_KLINE_URL, param);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 获取当前可用合约总持仓量
        /// </summary>
        /// <param name="symbol">btc_usd：比特币， ltc_usd：莱特币 （必填字段)</param>
        /// <param name="contract_type">合约类型。this_week：当周；next_week：下周；quarter：季度</param>
        /// <returns></returns>
        public String future_hold_amount(String symbol, String contract_type)
        {
            String result = "";
            try
            {
                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                String param = "";
                if (!StringUtil.isEmpty(symbol))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "symbol=" + symbol;
                }
                if (!StringUtil.isEmpty(contract_type))
                {
                    if (!param.Equals(""))
                    {
                        param += "&";
                    }
                    param += "contract_type=" + contract_type;
                }
                result = httpUtil.requestHttpGet(url_prex, FUTURE_HOLD_AMOUNT_URL, param);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        ///  获取期货交易历史
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="date">合约交割时间，格式yyyy-MM-dd</param>
        /// <param name="since">交易Id起始位置</param>
        /// <returns></returns>
        public String future_trades_history(String symbol, String date, String since)
        {
            String result = "";
            try
            {
                // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();
                if (!StringUtil.isEmpty(symbol))
                {
                    paras.Add("symbol", symbol);
                }
                if (!StringUtil.isEmpty(date))
                {
                    paras.Add("date", date);
                }
                if (!StringUtil.isEmpty(since))
                {
                    paras.Add("since", since);
                }
                if (!StringUtil.isEmpty(api_key))
                {
                    paras.Add("api_key", api_key);
                }
                String sign = MD5Util.buildMysignV1(paras, secret_key);
                paras.Add("sign", sign);
                // 发送post请求

                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                result = httpUtil.requestHttpPost(url_prex, FUTURE_TRADES_HISTORY_URL,
                       paras);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 批量获取期货订单信息
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contract_type">合约类型: this_week:当周   next_week:下周   quarter:季度 </param>
        /// <param name="order_id">订单ID(多个订单ID中间以","分隔,一次最多允许查询50个订单)</param>
        /// <returns></returns>
        public String future_orders_info(String symbol, String contract_type, String order_id)
        {
            String result = "";
            try
            {
                // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();
                if (!StringUtil.isEmpty(symbol))
                {
                    paras.Add("symbol", symbol);
                }
                if (!StringUtil.isEmpty(contract_type))
                {
                    paras.Add("contract_type", contract_type);
                }
                if (!StringUtil.isEmpty(order_id))
                {
                    paras.Add("order_id", order_id);
                }
                if (!StringUtil.isEmpty(api_key))
                {
                    paras.Add("api_key", api_key);
                }
                String sign = MD5Util.buildMysignV1(paras, secret_key);
                paras.Add("sign", sign);
                // 发送post请求

                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                result = httpUtil.requestHttpPost(url_prex, FUTURE_ORDERS_INFO_URL,
                       paras);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 获取期货爆仓单
        /// </summary>
        /// <param name="symbol">btc_usd：比特币， ltc_usd：莱特币</param>
        /// <param name="contract_type">合约类型。this_week：当周；next_week：下周；quarter：季度</param>
        /// <param name="status">状态 0：最近7天未成交 1:最近7天已成交</param>
        /// <param name="current_page">当前页数</param>
        /// <param name="page_length">每页获取条数，最多不超过50</param>
        /// <returns></returns>
        public String future_explosive(String symbol, String contract_type, String status, String current_page, String page_length)
        {
            String result = "";
            try
            {
                // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();
                if (!StringUtil.isEmpty(symbol))
                {
                    paras.Add("symbol", symbol);
                }
                if (!StringUtil.isEmpty(contract_type))
                {
                    paras.Add("contract_type", contract_type);
                }
                if (!StringUtil.isEmpty(status))
                {
                    paras.Add("status", status);
                }
                if (!StringUtil.isEmpty(api_key))
                {
                    paras.Add("api_key", api_key);
                }
                if (!StringUtil.isEmpty(current_page))
                {
                    paras.Add("current_page", current_page);
                }
                if (!StringUtil.isEmpty(page_length))
                {
                    paras.Add("page_length", page_length);
                }
                String sign = MD5Util.buildMysignV1(paras, secret_key);
                paras.Add("sign", sign);
                // 发送post请求

                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                result = httpUtil.requestHttpPost(url_prex, FUTURE_EXPLOSIVE_URL,
                       paras);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        /// <summary>
        /// 期货下单
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <param name="price">价格</param>
        /// <param name="amount">委托数量</param>
        /// <param name="type">1:开多   2:开空   3:平多   4:平空</param>
        /// <param name="matchPrice">是否为对手价 0:不是    1:是   ,当取值为1时,price无效</param>
        /// <param name="leverRate">杠杆率，10或20</param>
        /// <returns></returns>
        public String future_trade_ex(String symbol, String contractType,
            String price, String amount, String type, String matchPrice, String leverRate)
        {
            String result = "";
            try
            {  // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();
                if (!StringUtil.isEmpty(symbol))
                {
                    paras.Add("symbol", symbol);
                }
                if (!StringUtil.isEmpty(contractType))
                {
                    paras.Add("contract_type", contractType);
                }
                if (!StringUtil.isEmpty(api_key))
                {
                    paras.Add("api_key", api_key);
                }
                if (!StringUtil.isEmpty(price))
                {
                    paras.Add("price", price);
                }
                if (!StringUtil.isEmpty(amount))
                {
                    paras.Add("amount", amount);
                }
                if (!StringUtil.isEmpty(type))
                {
                    paras.Add("type", type);
                }
                if (!StringUtil.isEmpty(matchPrice))
                {
                    paras.Add("match_price", matchPrice);
                }
                if (!StringUtil.isEmpty(leverRate))
                {
                    paras.Add("lever_rate", leverRate);
                }
                String sign = MD5Util.buildMysignV1(paras, secret_key);
                paras.Add("sign", sign);
                // 发送post请求

                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                result = httpUtil.requestHttpPost(url_prex, FUTURE_TRADE_URL, paras);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        /// <summary>
        /// 期货下单
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <param name="price">价格</param>
        /// <param name="amount">委托数量</param>
        /// <param name="type">1:开多   2:开空   3:平多   4:平空</param>
        /// <param name="matchPrice">是否为对手价 0:不是    1:是   ,当取值为1时,price无效</param>
        /// <param name="leverRate">杠杆率，10或20</param>
        /// <returns></returns>
        public void future_async_trade_ex(String symbol, String contractType,
            String price, String amount, String type, String matchPrice, String leverRate, HttpAsyncReq.ResponseCallback callback)
        {
            try
            {  // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();
                if (!StringUtil.isEmpty(symbol))
                {
                    paras.Add("symbol", symbol);
                }
                if (!StringUtil.isEmpty(contractType))
                {
                    paras.Add("contract_type", contractType);
                }
                if (!StringUtil.isEmpty(api_key))
                {
                    paras.Add("api_key", api_key);
                }
                if (!StringUtil.isEmpty(price))
                {
                    paras.Add("price", price);
                }
                if (!StringUtil.isEmpty(amount))
                {
                    paras.Add("amount", amount);
                }
                if (!StringUtil.isEmpty(type))
                {
                    paras.Add("type", type);
                }
                if (!StringUtil.isEmpty(matchPrice))
                {
                    paras.Add("match_price", matchPrice);
                }
                if (!StringUtil.isEmpty(leverRate))
                {
                    paras.Add("lever_rate", leverRate);
                }
                String sign = MD5Util.buildMysignV1(paras, secret_key);
                paras.Add("sign", sign);
                // 发送post请求

                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                httpUtil.requestHttpPostAsync(url_prex, FUTURE_TRADE_URL, paras, callback);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        ///  取消订单
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <param name="orderId">订单ID</param>
        /// <returns></returns>
        public void future_cancel_async(String symbol, String contractType,
            String orderId, HttpAsyncReq.ResponseCallback callback)
        {
            try
            {  // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();

                if (!StringUtil.isEmpty(contractType))
                {
                    paras.Add("contract_type", contractType);
                }
                if (!StringUtil.isEmpty(orderId))
                {
                    paras.Add("order_id", orderId);
                }
                if (!StringUtil.isEmpty(api_key))
                {
                    paras.Add("api_key", api_key);
                }
                if (!StringUtil.isEmpty(symbol))
                {
                    paras.Add("symbol", symbol);
                }
                String sign = MD5Util.buildMysignV1(paras, secret_key);

                paras.Add("sign", sign);
                // 发送post请求

                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                httpUtil.requestHttpPostAsync(url_prex, FUTURE_CANCEL_URL, paras, callback);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void future_devolve_async(String symbol, String type,
            String amount, HttpAsyncReq.ResponseCallback callback)
        {
            try
            {  // 构造参数签名
                Dictionary<String, String> paras = new Dictionary<String, String>();

                if (!StringUtil.isEmpty(type))
                {
                    paras.Add("type", type);
                }
                if (!StringUtil.isEmpty(amount))
                {
                    paras.Add("amount", amount);
                }
                if (!StringUtil.isEmpty(api_key))
                {
                    paras.Add("api_key", api_key);
                }
                if (!StringUtil.isEmpty(symbol))
                {
                    paras.Add("symbol", symbol);
                }
                String sign = MD5Util.buildMysignV1(paras, secret_key);

                paras.Add("sign", sign);
                // 发送post请求

                HttpUtilManager httpUtil = HttpUtilManager.getInstance();
                httpUtil.requestHttpPostAsync(url_prex, FUTURE_DEVOLVE_URL, paras, callback);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
