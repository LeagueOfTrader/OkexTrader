using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.okcoin.rest.stock
{
    /// <summary>
    /// 现货行情，交易 REST API
    /// </summary>
    interface IStockRestApi
    {
        /// <summary>
        /// 行情
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <returns></returns>
        String ticker(String symbol);
        /// <summary>
        /// 现货市场深度
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="size">size:1-200</param>
        /// <returns></returns>
        String depth(String symbol, String size);
        /// <summary>
        /// 现货历史交易信息
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="since">不加since参数时，返回最近的60笔交易</param>
        /// <returns></returns>
        String trades(String symbol, String since);
        /// <summary>
        /// 现货的K线数据
        /// </summary>
        /// <param name="symbol">btc_usd：比特币， ltc_usd：莱特币 </param>
        /// <param name="type">
        /// 1min : 1分钟 
        /// 3min : 3分钟 
        /// 5min : 5分钟 
        /// 15min : 15分钟 
        /// 30min : 30分钟
        /// 1day : 1日
        /// 3day : 3日
        ///1week : 1周
        ///1hour : 1小时
        ///2hour : 2小时
        ///4hour : 4小时
        ///6hour : 6小时
        ///12hour : 12小时</param>
        /// <param name="size">指定获取数据的条数</param>
        /// <param name="since">时间戳（eg：1417536000000）。 返回该时间戳以后的数据 </param>
        /// <returns></returns>
        String kline(String symbol, String type, String size, String since);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        String userinfo();
        /// <summary>
        /// 下单交易
        /// </summary>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币</param>
        /// <param name="type">买卖类型： 限价单（buy/sell） 市价单（buy_market/sell_market）</param>
        /// <param name="price">下单价格 [限价买单(必填)： 大于等于0，小于等于1000000 |  市价买单(必填)： BTC :最少买入0.01个BTC 的金额(金额>0.01*卖一价) / LTC :最少买入0.1个LTC 的金额(金额>0.1*卖一价)]</param>
        /// <param name="amount"> 交易数量 [限价卖单（必填）：BTC 数量大于等于0.01 / LTC 数量大于等于0.1 | 市价卖单（必填）： BTC :最少卖出数量大于等于0.01 / LTC :最少卖出数量大于等于0.1]</param>
        /// <returns></returns>
        String trade(String symbol, String type, String price, String amount);
        /// <summary>
        /// 批量下单
        /// </summary>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币</param>
        /// <param name="type">买卖类型： 限价单（buy/sell） 市价单（buy_market/sell_market）</param>
        /// <param name="orders_data">JSON类型的字符串 例：[{price:3,amount:5},{price:3,amount:3}]   最大下单量为5，price和amount参数参考trade接口中的说明</param>
        /// <returns></returns>
        String batch_trade(String symbol, String type, String orders_data);
        /// <summary>
        /// 撤销订单
        /// </summary>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币</param>
        /// <param name="order_id">订单ID(多个订单ID中间以","分隔,一次最多允许撤消3个订单)</param>
        /// <returns></returns>
        String cancel_order(String symbol, String order_id);
        /// <summary>
        /// 获取用户的订单信息
        /// </summary>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币</param>
        /// <param name="order_id">订单ID(-1查询全部订单，否则查询相应单号的订单)</param>
        /// <returns></returns>
        String order_info(String symbol, String order_id);
        /// <summary>
        /// 批量获取用户订单
        /// </summary>
        /// <param name="type">查询类型 0:未成交，未成交 1:完全成交，已撤销</param>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币</param>
        /// <param name="order_id">订单ID(多个订单ID中间以","分隔,一次最多允许查询50个订单)</param>
        /// <returns></returns>
        String orders_info(String type, String symbol,
            String order_id);
        /// <summary>
        /// 获取历史订单信息，只返回最近七天的信息
        /// </summary>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币</param>
        /// <param name="status">委托状态: 0：未成交 1：已完成(最近七天的数据)</param>
        /// <param name="current_page">当前页数</param>
        /// <param name="page_length">每页数据条数，最多不超过200</param>
        /// <returns></returns>
        String order_history(String symbol, String status,
            String current_page, String page_length);
        /// <summary>
        /// 提币BTC/LTC
        /// </summary>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币 </param>
        /// <param name="chargefee">网络手续费 BTC默认范围 [0.0001，0.01] LTC默认范围 [0.001，0.2],手续费越高，网络确认越快，OKCoin内部提币设置0 </param>
        /// <param name="trade_pwd">交易密码 </param>
        /// <param name="withdraw_address">提币认证地址 </param>
        /// <param name="withdraw_amount">提币数量 BTC>=0.01 LTC>=0.1 </param>
        /// <returns></returns>
        String withdraw(String symbol, String chargefee, String trade_pwd, String withdraw_address, String withdraw_amount);
        /// <summary>
        /// 取消提币BTC/LTC
        /// </summary>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币 </param>
        /// <param name="withdraw_id">提币申请Id </param>
        /// <returns></returns>
        String cancel_withdraw(String symbol, String withdraw_id);
        /// <summary>
        /// 查询手续费
        /// </summary>
        /// <param name="order_id">订单ID </param>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币 </param>
        /// <returns></returns>
        String order_fee(String order_id, String symbol);
        /// <summary>
        ///  获取放款深度前10
        /// </summary>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币 usd: 美元</param>
        /// <returns></returns>
        String lend_depth(String symbol);
        /// <summary>
        ///  查询用户借款信息
        /// </summary>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币 usd: 美元 </param>
        /// <returns></returns>
        String borrows_info(String symbol);
        /// <summary>
        /// 申请借款
        /// </summary>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币 usd: 美元 </param>
        /// <param name="days">借款天数， three，seven，fifteen，thirty，sixty，ninety </param>
        /// <param name="amount">借入数量 </param>
        /// <param name="rate">借款利率 [0.0001, 0.01] </param>
        /// <returns></returns>
        String borrow_money(String symbol, String days, String amount, String rate);
        /// <summary>
        /// 取消借款申请
        /// </summary>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币 usd: 美元 </param>
        /// <param name="borrow_id">借款单ID</param>
        /// <returns></returns>
        String cancel_borrow(String symbol, String borrow_id);
        /// <summary>
        ///  获取借款订单记录
        /// </summary>
        /// <param name="borrow_id">借款单ID</param>
        /// <returns></returns>
        String borrow_order_info(String borrow_id);
        /// <summary>
        /// 用户还全款
        /// </summary>
        /// <param name="borrow_id">借款单ID</param>
        /// <returns></returns>
        String repayment(String borrow_id);
        /// <summary>
        /// 未还款列表
        /// </summary>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币 usd: 美元 </param>
        /// <param name="current_page">当前页数</param>
        /// <param name="page_length">每页数据条数，最多不超过50条</param>
        /// <returns></returns>
        String unrepayments_info(String symbol, String current_page, String page_length);
        /// <summary>
        /// 获取用户提现/充值记录
        /// </summary>
        /// <param name="symbol">btc_usd: 比特币 ltc_usd: 莱特币 usd: 美元</param>
        /// <param name="type">0：充值 1 ：提现 </param>
        /// <param name="current_page">当前页数</param>
        /// <param name="page_length">每页数据条数，最多不超过50条</param>
        /// <returns></returns>
        String account_records(String symbol, String type, String current_page, String page_length);
        /// <summary>
        ///  获取历史交易信息
        /// </summary>
        /// <param name="symbol">btc_usd:比特币 ltc_usd :莱特币 </param>
        /// <param name="since">从某一tid开始访问600条数据(必填项) </param>
        /// <returns></returns>
        String trade_history(String symbol, String since);
    }
}
