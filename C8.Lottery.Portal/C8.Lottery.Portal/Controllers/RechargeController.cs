using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using C8.Lottery.Model;
using C8.Lottery.Public;
using Senparc.Weixin.HttpUtility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace C8.Lottery.Portal.Controllers
{
    /// <summary>
    /// 充值控制器
    /// </summary>
    public class RechargeController : BaseController
    {
        // private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static object _lock = new object();
        [Authentication]
        public ActionResult Index()
        {
            return View();
        }

        public string GetRandom()
        {
            lock (_lock)
            {
                return "T" + DateTime.Now.Ticks;
            }
        }

        #region 微信支付代码
        string appid = "wx226d38e96ed8f01e";
        string mchid = "1375852802";
        string key = "1de60212dceafe2b4fdb621bd6f04288";

        public ActionResult GetWxUrl(int amount)
        {
            var no = GetRandom();// Guid.NewGuid().ToString("N");
            var redirect_url = HttpUtility.UrlEncode(HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + "/Personal/TransactionRecord");
            var total = (amount * 100).ToString();

            //LogHelper.WriteLog("微信： redirect_url="+redirect_url);
            //LogHelper.WriteLog("微信： Ip=" + Tool.GetIP());


            Senparc.Weixin.MP.TenPayLibV3.RequestHandler packageReqHandler = new Senparc.Weixin.MP.TenPayLibV3.RequestHandler(null);
            packageReqHandler.SetParameter("appid", appid);//APPID
            packageReqHandler.SetParameter("mch_id", mchid);//商户号
            packageReqHandler.SetParameter("nonce_str", Senparc.Weixin.MP.TenPayLibV3.TenPayV3Util.GetNoncestr());
            packageReqHandler.SetParameter("body", "金币充值");
            packageReqHandler.SetParameter("out_trade_no", no);//订单号
            packageReqHandler.SetParameter("total_fee", total); //金额,以分为单位
            packageReqHandler.SetParameter("spbill_create_ip",Tool.GetIP());//IP
            packageReqHandler.SetParameter("notify_url", HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + "/Recharge/WxNotify"); //回调地址
            packageReqHandler.SetParameter("trade_type", "MWEB");//这个不可以改。固定为Mweb
            packageReqHandler.SetParameter("sign", packageReqHandler.CreateMd5Sign("key", key));
            string data = packageReqHandler.ParseXML();
            var urlFormat = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            var formDataBytes = data == null ? new byte[0] : Encoding.UTF8.GetBytes(data);
            MemoryStream ms = new MemoryStream();
            ms.Write(formDataBytes, 0, formDataBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);

            if (AddComeOutRecord(no, amount, 1))
            {
                var result = RequestUtility.HttpPost(urlFormat, null, ms);

                var res = System.Xml.Linq.XDocument.Parse(result);
                //log.Info(res);
                //log.Info("订单号:" + no);
                string mweb_url = res.Element("xml").Element("mweb_url").Value + "&redirect_url=" + redirect_url;

                return Content(mweb_url);
            }
            else
            {
                return Content("0");
            }
        }

        public ActionResult WxNotify()
        {
            Senparc.Weixin.MP.TenPayLibV3.ResponseHandler payNotifyRepHandler = new Senparc.Weixin.MP.TenPayLibV3.ResponseHandler(null);
            payNotifyRepHandler.SetKey(key);

            string return_code = payNotifyRepHandler.GetParameter("return_code");
            string return_msg = payNotifyRepHandler.GetParameter("return_msg");
            string xml = string.Format(@"<xml><return_code><![CDATA[{0}]]></return_code><return_msg><![CDATA[{1}]]></return_msg></xml>", return_code, return_msg);

            //   log.Info(xml);
            if (return_code.ToUpper() != "SUCCESS")
            {
                return Content(xml, "text/xml");
            }

            string out_trade_no = payNotifyRepHandler.GetParameter("out_trade_no");

            //log.Info("微信回调订单号：-" + out_trade_no);
            //微信服务器可能会多次推送到本接口，这里需要根据out_trade_no去查询订单是否处理，如果处理直接返回：return Content(xml, "text/xml"); 不跑下面代码
            //if (false)
            //{
            //验证请求是否从微信发过来（安全）
            if (payNotifyRepHandler.IsTenpaySign())
            {
                if (AlertComeOutRecord(out_trade_no, 1))
                {
                    return Content(xml, "text/xml");
                }
                else
                {                   
                    //如果订单修改失败，需要微信再次发送请求
                    xml = string.Format(@"<xml><return_code><![CDATA[FAIL]]></return_code><return_msg><![CDATA[]]></return_msg></xml>");
                    return Content(xml, "text/xml");
                }
            }
            else {
                LogHelper.WriteLog("---------222222222");
                return Content("");
            }           
        }


        #endregion

        #region 支付宝支付代码

        static string serverUrl = "https://openapi.alipay.com/gateway.do";
        static string app_id = "2018041302550006";   //开发者的应用ID
        static string format = "JSON";
        static string charset = "utf-8";
        static string sign_type = "RSA2";  //签名格式
        static string version = "1.0";
        //商户私钥
        static string merchant_private_key = "MIIEpQIBAAKCAQEA0wSiER8RQygVK1/1j5nYAzDJwI6Tx9NL3j7YfecRZvfXCJ8oTysByONG/loL0vdl/Ih0oeIwDcNbUr1F4AgjnQYRz8l16UVu8AwYShqf29MGfSAQLdvzqG3/oqkJpNwaBMALtyq1qwklga5fEgH8/l+pFc+LN2aTaxtWFShaDQRpMuVGsE3CwzALK+XWZxm49XcbBikH04F65Kx0NmC4FziBT/HS/kCC5atJ2JiUoa2VM8j00CBM0On/vHodjU3HVmmBg1LBVoTi9+FbaJT+Hdqy4H0In1V+2K9OVREe8ZrURT6S0zAeTgrjAUTKt9PMYrpkCaDkVqHzIhOB2gruvwIDAQABAoIBAQDBSVciE7D+MLLjXixR8vs4QPIsXOzkdpjh4/LtsD/yb0YacZ68lYo29mfLB7QY8+AJJvyeY87cbHs0GIbupMXqSOr7x28n0x/A5XNCPYz8EBm7dykauIRBXTBxUCCzT6DNhRO2HXr2RZSDarNOjV+tqPX6Mnc0sdKKoymAi8ugayUaqn8dTajoqg+drk6L1IgZCMK/CmVYPYJIQ+VrvRgwQ7bYiC1BHCme7I/n43rxGOQaXB9wRK5/LSD9O0tR9QktvhbsmtwW+e5TvoAGpy9p0LQMhLSdj5CocnM0G11dIboECrYc+SYD841EJF4J2pm18VIPp3rvoYZxRXrFleSxAoGBAPkGkmnawaHk/IhHyX0ZuHVMwBf8l4ZK3ducpBLKRdtLQvW2XOuoQhNiuKXg7KOW9npvN/GwKC50V8l7RblbEoZngyJ8rjm00sRF0UtSbWOdyII5WF0/mwJGm8yiwq8GuSyHj0xAdeJopH53ybuejBfN8VeBzv4H1G9dUuLnbCxVAoGBANjtj1oKsE7LrjNkiIN1+3P+wKIy/TwqA5bLwUILnzVW92IC2gHQLN+NoksKngkgH7m8Xnp0Eue3anUNfeAgdJKar94bKiBNest4ZOk288w0E5kRr2X/UtlVLIYcuJ4o9bx+QDn+ftMlWk90WOQI14QQxswZCa1fHe9b/60VmoLDAoGBAJqxSWx2VsiB3Zmutmx++MXtGnsMDvh+M1lD8ew2OLTkCMFoOkqtp/Yw4jExCu8ITS57Pk5ltmA9J3dim0psV5KkZKKcvwHb4P3JvRzEJG24SyESDGFIrLr6L7gr9zIQxCD0SMD+Xfx6MozZTri84Zu788r/OR02sfFIEMAhMGJNAoGAc4GK4xbt6gbqKtNNHTKlQY5UZAlibbaxUooLzW8CxxQXhUifbHe8bQytbeepXpKMUgnLBMjpiBhRxyH39G9TovxayJkORUT8LXtdwBBSoFjaVpbkHhtlsfN4UbDZXN3Sext+d2LbhPJOtB/vdPyARQHp2KM8U+RhvCHwceke7KECgYEAsdikO13ps6ML6buBTOZ3/3lqJVmNkUo9YG6gpxzhq/RQ2dXPqRqvJtDr/qmjP1+kigOi9xr8ZifjBEUI1ymTv/Th3X3Rq+o6Hrdfr1TpqE8ADlnJQYc6noV0qdwkd8lsnToLCrpUNLmU0EKWaIbiRv3+E55H80A3oyKGufqIlBo=";
        //支付宝公钥
        static string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAydy5QG+mxrN77GIiLLEis/cVYleYn8hg5CesKKT73WkswNSr4ohTfV/BVvHdURec395Ex2T230mKu/QSbbY7Ahi3k+EWufIX6TYz/Q28grV4BrtF1kIcNvQUHgE/zEGL+nVQhfEH/uBjZQ84ue4S8ywnVBovndB2rJOExr1SBS5iI1yzYxdvpHRGTHHMIF0MInW96dRU1t3XqGut6e4YvsZj8x3tEprNBSF5MIQ+BRz9KrdplIRpzR/sPXenxbgGjBTj5Fus/a625Oofb23F6W8qPG86m9RgPo14BWmAi5SoVa7FAYubN2p3OBpnEhxkDqSq5LmtVHkXUKOjxaIQxwIDAQAB";

        /// <summary>
        /// 获取支付宝唤起APP的代码
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="no"></param>
        /// <returns></returns>
        public ActionResult GetZfbUrl(int amount)
        {
            var no = GetRandom();// Guid.NewGuid().ToString("N");
            IAopClient client = new DefaultAopClient(serverUrl, app_id, merchant_private_key, format, version, sign_type, alipay_public_key, charset, false);
            AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
            string address = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host; //获取访问域名
                                                                       // log.Info("address:" + address);
            request.SetReturnUrl(address + "/Personal/TransactionRecord");//同步请求
            request.SetNotifyUrl(address + "/Recharge/AsyncPay");//异步请求
            request.BizContent = "{" +
                                    "\"body\":\"金币充值\"," +
                                    "\"subject\":\"金币充值\"," +
                                    "\"out_trade_no\":\"" + no + "\"," +
                                    "\"timeout_express\":\"5m\"," +
                                    "\"total_amount\":" + amount + "," +
                                    "\"product_code\":\"QUICK_WAP_WAY\"" +
                                "  }";//这里填写一些发送给支付宝的一些参数

            // log.Info(request.BizContent);

            if (AddComeOutRecord(no, amount, 2))
            {
                AlipayTradeWapPayResponse response = client.pageExecute(request);
                return Content(response.Body);//这里会发送一个表单输出到页面中
            }
            else
            {
                return Content("0");
            }
        }

        /// <summary>
        /// 验证通知数据的正确性
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="total_amount"></param>
        /// <param name="seller_id"></param>
        /// <returns></returns> 
        private SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="inputPara"></param>
        /// <returns></returns>
        public bool Verify(SortedDictionary<string, string> inputPara)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            Boolean verifyResult = AlipaySignature.RSACheckV1(inputPara, alipay_public_key, charset, sign_type, false);
            return verifyResult;
        }

        /// <summary>
        /// 支付宝异步回调方法
        /// </summary>
        public void AsyncPay()
        {
            LogHelper.WriteLog("支付宝异步回调页面");
            SortedDictionary<string, string> sPara = GetRequestPost();//将post请求过来的参数传化为SortedDictionary
                                                                      // log.Info("sPara.Count:" + sPara.Count);
            if (sPara.Count > 0)
            {
                bool bo = Verify(sPara);
                LogHelper.WriteLog("Verify(sPara):" + bo);
                if (bo)//验签if (VerifyResult)
                {
                    try
                    {
                        //商户订单号
                        string out_trade_no = Request.Form["out_trade_no"];
                        //支付宝交易号
                        string trade_no = Request.Form["trade_no"];
                        //支付金额
                        decimal total_amount = decimal.Parse(Request.Form["total_amount"]);//.ConvertType(Decimal.Zero);
                        //实收金额
                        decimal receipt_amount = decimal.Parse(Request.Form["receipt_amount"]);//.ConvertType(Decimal.Zero);
                        //交易状态
                        string trade_status = Request.Form["trade_status"];
                        //卖家支付宝账号
                        string seller_id = Request.Form["seller_id"];

                        //商品描述
                        string body = Request.Form["body"];
                        //交易创建时间
                        DateTime gmt_create = DateTime.Parse(Request.Form["gmt_create"]);
                        //交易付款时间
                        DateTime gmt_payment = DateTime.Parse(Request.Form["gmt_payment"]);
                        string appid = Request.Form["app_id"];
                        // WriteError("验证参数开始");

                        //log.Info("商户订单号:" + out_trade_no);
                        //log.Info("支付宝交易号:" + trade_no);
                        //log.Info("支付金额:" + total_amount);
                        //log.Info("实收金额:" + receipt_amount);
                        //log.Info("交易状态:" + trade_status);
                        //log.Info("卖家支付宝账号:" + seller_id);
                        //log.Info("商品描述:" + body);
                        //log.Info("交易创建时间:" + gmt_create);
                        //log.Info("交易付款时间:" + gmt_payment);
                        LogHelper.WriteLog("trade_status--" + trade_status);
                        if (trade_status == "TRADE_FINISHED") //支持退款订单，如果超过可退款日期，支付宝发送一条请求并走这个代码
                        {
                            //  log.Info("该订单不可退款");
                        }
                        else if (trade_status == "TRADE_SUCCESS")
                        {

                            if (AlertComeOutRecord(out_trade_no, 2))
                            {
                                Response.Write("success");  //请不要修改或删除
                            }
                            else
                            {
                                Response.Write("fail");  //如果订单修改失败，则要求支付宝再次发送请求
                            }
                        }
                        else
                        {

                        }

                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                        Response.Write("success");  //请不要修改或删除

                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }

                    catch (Exception ex)
                    {

                        //  log.Error(ex.Message);
                    }
                }
                else//验证失败
                {
                    // log.Info("验证失败");
                    Response.Write("fail");
                }
            }
            else
            {
                // log.Info("无通知参数");
                Response.Write("无通知参数");
            }
        }

        #endregion

        /// <summary>
        /// 添加订单信息
        /// </summary>
        /// <param name="no">订单号</param>
        /// <param name="money">订单金额(单位：元)</param>
        /// <param name="payType">支付类型</param>
        private bool AddComeOutRecord(string no, int money, int payType)
        {
            int userId = UserHelper.GetByUserId();
            string sql = @"insert into ComeOutRecord (UserId,OrderId,Money,Type,SubTime,PayType) 
                            values(@UserId,@OrderId,@Money,1,GETDATE(),@PayType);select @@identity;";

            var moneyToCoin = Convert.ToInt32(ConfigurationManager.AppSettings["MoneyToCoin"]);

            SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@UserId",userId),
                    new SqlParameter("@OrderId",no),
                    new SqlParameter("@Money",money*moneyToCoin),
                    new SqlParameter("@PayType",payType)
                 };

            var i = Convert.ToInt32(SqlHelper.ExecuteScalar(sql, regsp));

            return i > 0;
        }

        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="no"></param>
        /// <param name="type"></param>
        private bool AlertComeOutRecord(string no, int payType)
        {
            try
            {
                //判断支付中的订单是否存在,如果不存在.则说明已经改变状态了
                string sql = "select * from ComeOutRecord where OrderId=@OrderId and PayType=@PayType and State=1";
                var list = Util.ReaderToList<ComeOutRecord>(sql, new SqlParameter[] { new SqlParameter("@OrderId", no), new SqlParameter("@PayType", payType) });
                if (!list.Any())
                {
                    return true;
                }
                sql = "";
                var money = list.FirstOrDefault().Money;
                var userId = list.FirstOrDefault().UserId;
                var addCoin = 0;  //需要增加的金币数
                var moneyToCoin = Convert.ToInt32(ConfigurationManager.AppSettings["MoneyToCoin"]);

                //每日任务完成充值100元任务
                if (money/moneyToCoin >= 100)
                {
                    var makeMoneyTaskList = Util.ReaderToList<MakeMoneyTask>("select top(1) * from MakeMoneyTask where Code=100");
                    if (makeMoneyTaskList != null && makeMoneyTaskList.Any())
                    {
                        try
                        {
                            var obj = SqlHelper.ExecuteScalar("select top(1) completedCount from usertask where UserId = @UserId and taskId = 100",
                                new SqlParameter[] { new SqlParameter("@UserId", userId) });

                            if (obj == null || (Convert.ToInt32(obj)+1) == makeMoneyTaskList.FirstOrDefault().Count)//判断今日是否完任务
                            {                               
                                addCoin = makeMoneyTaskList.FirstOrDefault().Coin;
                                sql += "insert into ComeOutRecord (UserId,OrderId,Money,Type,SubTime) values(@UserId,100,@Money,8,GETDATE());";//插入领取任务记录
                            }
                            if (obj == null) //如果usertask表没有数据，则插入
                            {
                                sql += "insert into usertask (UserId,TaskId,CompletedCount) values (@UserId,100,1);";
                            }
                            else
                            {
                                sql += "update usertask set completedCount = completedCount +1 where UserId = @UserId and taskId = 100;";
                            }
                        }
                        catch (Exception) { }
                    }
                }

                sql += @"update ComeOutRecord set State = 3 where OrderId=@OrderId and PayType=@PayType;
                        update UserInfo set Coin = Coin + " + (money + addCoin) + " where Id =@UserId;";

                SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@OrderId",no),
                    new SqlParameter("@PayType",payType),
                    new SqlParameter("@UserId",userId),
                    new SqlParameter("@Money",addCoin)
                 };

                SqlHelper.ExecuteTransaction(sql, regsp);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("错误 -- " + ex.Message);
                return false;
            }
        }
    }
}
