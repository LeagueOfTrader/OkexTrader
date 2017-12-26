//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using com.okcoin.rest.future;
//using com.okcoin.rest.stock;
//namespace com.okcoin.rest
//{
//    class Client
//    {
//        static void Main(String[] args)
//        {
//            String api_key = "";  //OKCoin申请的apiKey
//            String secret_key = "";  //OKCoin申请的secretKey
//            String url_prex = "https://www.okcoin.com"; //国内站账号配置 为 https://www.okcoin.cn
//            //期货操作
//            FutureRestApiV1 getRequest = new FutureRestApiV1(url_prex);
//            FutureRestApiV1 postRequest = new FutureRestApiV1(url_prex, api_key, secret_key);
//            //期货行情信息
//            //Console.WriteLine(getRequest.future_ticker("ltc_usd", "this_week"));
//            //期货深度信息
//            //Console.WriteLine(getRequest.future_depth("ltc_usd", "this_week"));
//            //期货交易记录信息
//            //Console.WriteLine(getRequest.future_trades("ltc_usd","this_week"));
//            //期货指数信息
//            //Console.WriteLine(getRequest.future_index("ltc_usd"));
//            // 获取美元人民币汇率
//            //Console.WriteLine(getRequest.exchange_rate());
//            //获取交割预估价
//            //Console.WriteLine(getRequest.future_estimated_price("ltc_usd"));
//            // 获取期货合约的K线数据
//            //Console.WriteLine(getRequest.future_kline("ltc_usd", "1min", "this_week", "1", "1417536000000"));
//            //获取当前可用合约总持仓量
//            //Console.WriteLine(getRequest.future_hold_amount("ltc_usd","this_week"));
//            // 获取期货账户信息 （全仓）
//            //Console.WriteLine(postRequest.future_userinfo());
//            // 获取用户持仓获取OKCoin期货账户信息 （全仓）
//            //Console.WriteLine(postRequest.future_position("ltc_usd","this_week"));
//            //期货下单(862413180)
//            //Console.WriteLine(postRequest.future_trade("ltc_usd", "this_week", "1", "1", "1", "0"));
//            //获取期货交易历史
//            //Console.WriteLine(postRequest.future_trades_history("ltc_usd", "2015-09-02", "1"));
//            //批量下单(返回两个order_id(862492945,862492949)
//            //Console.WriteLine(postRequest.future_batch_trade("ltc_usd", "this_week", "[{price:1,amount:1,type:1,match_price:1},{price:1,amount:1,type:1,match_price:1}]", "10"));
//            // 取消期货订单
//            //Console.WriteLine(postRequest.future_cancel("ltc_usd", "this_week", "order_id"));
//            //获取期货订单信息
//            //Console.WriteLine(postRequest.future_order_info("ltc_usd", "this_week", "862413180", "2", "1", "2"));
//            //批量获取期货订单信息
//            // Console.WriteLine(postRequest.future_orders_info("ltc_usd", "this_week", "order_id"));
//            //获取逐仓期货账户信息
//            //Console.WriteLine(postRequest.future_userinfo_4fix());
//            // 逐仓用户持仓查询
//            //Console.WriteLine(postRequest.future_position_4fix("ltc_usd","this_week"));
//            // 获取期货爆仓单
//            //Console.WriteLine(postRequest.future_explosive("ltc_usd","this_week","1","1","2"));



//            //现货操作
//            StockRestApi getRequest1 = new StockRestApi(url_prex);
//            StockRestApi postRequest1 = new StockRestApi(url_prex, api_key, secret_key);
//            //获取现货行情
//            //Console.WriteLine(getRequest1.ticker("ltc_usd"));
//            //获取现货市场深度
//            //Console.WriteLine(getRequest1.depth("ltc_usd","2"));
//            //获取最近600交易信息
//            //Console.WriteLine(getRequest1.trades("ltc_usd","20"));
//             //获取比特币或莱特币的K线数据
//            //Console.WriteLine(getRequest1.kline("ltc_usd", "1min", "2", "1417536000000"));
//            // 获取用户信息
//            //Console.WriteLine(postRequest1.userinfo());
//            //下单交易(order_id":32490296)
//            //Console.WriteLine(postRequest1.trade("ltc_usd","buy","0.001","1"));
//            // 获取历史交易信息
//            //Console.WriteLine(postRequest1.trade_history("ltc_usd","2"));
//            //批量下单
//            //Console.WriteLine(postRequest1.batch_trade("ltc_usd", "buy", "[{price:3,amount:5,type:'sell'},{price:1,amount:1,type:'buy'},{price:1,amount:1}] "));
//            //撤销订单
//            //Console.WriteLine(postRequest1.cancel_order("ltc_usd","order_id"));
//            //获取用户的订单信息
//            //Console.WriteLine(postRequest1.order_info("ltc_usd","-1"));
//            // 批量获取用户订单
//            //Console.WriteLine(postRequest1.orders_info("0","ltc_usd","order_id"));
//            //获取历史订单信息，只返回最近七天的信息
//            //Console.WriteLine(postRequest1.order_history("ltc_usd","0","1","2"));
//            // 提币BTC/LTC
//            //Console.WriteLine(postRequest1.withdraw("ltc_usd", "0.001", "trade_pwd", "withdraw_address", "withdraw_amount "));
//            // 取消提币BTC/LTC
//            //Console.WriteLine(postRequest1.cancel_withdraw("ltc_usd", "withdraw_id"));
//            //查询手续费
//            //Console.WriteLine(postRequest1.order_fee("order_id","ltc_usd"));
//            //获取放款深度前10
//            //Console.WriteLine(postRequest1.lend_depth("ltc_usd"));
//            // 查询用户借款信息
//            //Console.WriteLine(postRequest1.borrows_info("ltc_usd"));
//            //申请借款(borrow_id":22789)
//            //Console.WriteLine(postRequest1.borrow_money("ltc_usd","three","1","0.001"));
//            //取消借款申请
//            //Console.WriteLine(postRequest1.cancel_borrow("ltc_usd", "22789"));
//            //获取借款订单记录
//            //Console.WriteLine(postRequest1.borrow_order_info("22789"));
//            //用户还全款
//            //Console.WriteLine(postRequest1.repayment("22789"));
//            // 未还款列表
//            //Console.WriteLine(postRequest1.unrepayments_info("ltc_usd", "1", "2"));
//            //获取用户提现/充值记录
//            //Console.WriteLine(postRequest1.account_records("ltc_usd","1","1","2"));
           
            
//        }
//    }
//}
