using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.okcoin.rest.future
{
    /// <summary>
    /// 新版本期货 REST API实现
    /// </summary>
    interface IFutureRestApi
    {
        /// <summary>
        /// 期货行情
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <returns></returns>
        String future_ticker(String symbol, String contractType);
        /// <summary>
        /// 期货指数
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <returns></returns>
        String future_index(String symbol);
        /// <summary>
        /// 期货交易记录
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType"> 合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <returns></returns>
        String future_trades(String symbol, String contractType);
        /// <summary>
        /// 期货深度
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <returns></returns>
        String future_depth(String symbol, String contractType);
        /// <summary>
        /// 汇率查询
        /// </summary>
        /// <returns></returns>
        String exchange_rate();
        /// <summary>
        ///  取消订单
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <param name="orderId">订单ID</param>
        /// <returns></returns>
        String future_cancel(String symbol, String contractType, String orderId);
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
        String future_trade(String symbol, String contractType, String price, String amount, String type, String matchPrice);
        /// <summary>
        /// 批量下单
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contract_type">合约类型: this_week:当周   next_week:下周   quarter:季度</param>
        /// <param name="orders_data">JSON类型的字符串 例：[{price:5,amount:2,type:1,match_price:1},{price:2,amount:3,type:1,match_price:1}] 最大下单量为5，price,amount,type,match_price参数参考future_trade接口中的说明 </param>
        ///<param name="lever_rate">杠杆倍数 value:10\20 默认10</param>
        /// <returns></returns>
        String future_batch_trade(String symbol, String contract_type, String orders_data, String lever_rate);
        /// <summary>
        /// 期货账户信息
        /// </summary>
        /// <returns></returns>
        String future_userinfo();
        /// <summary>
        /// 期货逐仓账户信息
        /// </summary>
        /// <returns></returns>
        String future_userinfo_4fix();
        /// <summary>
        /// 用户持仓查询
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <returns></returns>
        String future_position(String symbol, String contractType);
        /// <summary>
        /// 用户逐仓持仓查询
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contractType">合约类型: this_week:当周   next_week:下周   month:当月   quarter:季度</param>
        /// <returns></returns>
        String future_position_4fix(String symbol, String contractType);
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
        String future_order_info(String symbol, String contractType, String orderId, String status, String currentPage, String pageLength);
        /// <summary>
        /// 获取交割预估价
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <returns></returns>
        String future_estimated_price(String symbol);
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
        String future_kline(String symbol, String type, String contract_type, String size, String since);
        /// <summary>
        /// 获取当前可用合约总持仓量
        /// </summary>
        /// <param name="symbol">btc_usd：比特币， ltc_usd：莱特币 （必填字段)</param>
        /// <param name="contract_type">合约类型。this_week：当周；next_week：下周；quarter：季度</param>
        /// <returns></returns>
        String future_hold_amount(String symbol, String contract_type);
        /// <summary>
        ///  获取期货交易历史
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="date">合约交割时间，格式yyyy-MM-dd</param>
        /// <param name="since">交易Id起始位置</param>
        /// <returns></returns>
        String future_trades_history(String symbol, String date, String since);
        /// <summary>
        /// 批量获取期货订单信息
        /// </summary>
        /// <param name="symbol">btc_usd:比特币    ltc_usd :莱特币</param>
        /// <param name="contract_type">合约类型: this_week:当周   next_week:下周   quarter:季度 </param>
        /// <param name="order_id">订单ID(多个订单ID中间以","分隔,一次最多允许查询50个订单)</param>
        /// <returns></returns>
        String future_orders_info(String symbol, String contract_type, String order_id);
        /// <summary>
        /// 获取期货爆仓单
        /// </summary>
        /// <param name="symbol">btc_usd：比特币， ltc_usd：莱特币</param>
        /// <param name="contract_type">合约类型。this_week：当周；next_week：下周；quarter：季度</param>
        /// <param name="status">状态 0：最近7天未成交 1:最近7天已成交</param>
        /// <param name="current_page">当前页数</param>
        /// <param name="page_length">每页获取条数，最多不超过50</param>
        /// <returns></returns>
        String future_explosive(String symbol, String contract_type, String status, String current_page, String page_length);
    }
}
