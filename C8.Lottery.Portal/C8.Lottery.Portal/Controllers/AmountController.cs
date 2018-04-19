using C8.Lottery.Model;
using C8.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace C8.Lottery.Portal.Controllers
{
    /// <summary>
    /// 现金类控制器（绑卡、提现等）
    /// </summary>
    public class AmountController : Controller
    {
        /// <summary>
        /// 绑定银行卡页面
        /// </summary>
        /// <returns></returns>
        [Authentication]
        public ActionResult BindingBankCard()
        {
            int userId = UserHelper.GetByUserId();
            string sql = "select * from bankinfo where userId = @userId";
            var list = Util.ReaderToList<BankInfo>(sql, new SqlParameter[] { new SqlParameter("@userId", userId) });

            ViewBag.CardList = list;

            return View();
        }

        public ActionResult CheckBankAccount(string BankAccount)
        {
            var url = "https://ccdcapi.alipay.com/validateAndCacheCardInfo.json?_input_charset=utf-8&cardNo=" + BankAccount + "&cardBinCheck=true";
            var dd = HttpCommon.HttpGet(url);
            return Content(dd);
        }

        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <returns></returns>
        public ActionResult AddBankCard(BankInfo model)
        {
            int userId = UserHelper.GetByUserId();

            model.SubTime = DateTime.Now;
            model.UserId = userId;

            try
            {
                var rows = Convert.ToInt32(SqlHelper.ExecuteScalar("select count(*) from bankinfo where UserId=@UserId",
                    new SqlParameter[] { new SqlParameter("@UserId", model.UserId) }));

                string regsql = "";
                if (rows > 0)
                {
                    regsql = @"update bankinfo set BankAccount=@BankAccount,BankName=@BankName,SubTime=@SubTime where UserId=@UserId";
                }
                else
                {
                    regsql = @"insert into bankinfo (UserId,TrueName,BankAccount,BankName,SubTime)
                                    values (@UserId,@TrueName,@BankAccount,@BankName,@SubTime)";
                }

                SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@UserId",model.UserId),
                    new SqlParameter("@TrueName",model.TrueName),
                    new SqlParameter("@BankAccount",model.BankAccount),
                    new SqlParameter("@BankName",model.BankName),
                    new SqlParameter("@SubTime",model.SubTime)
                 };

                var i = SqlHelper.ExecuteNonQuery(regsql, regsp);

                return Json(new { Status = i>0?1:0 });

            }
            catch (Exception ex)
            {
                return Json(new { Status = 0 });
            }
        }

        /// <summary>
        /// 绑定成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult BindingSuccess()
        {
            return View();
        }

        /// <summary>
        /// 提现页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExtractCash()
        {
            int userId = UserHelper.GetByUserId();
            string sql = "select * from bankinfo where userId = @userId";
            var list = Util.ReaderToList<BankInfo>(sql, new SqlParameter[] { new SqlParameter("@userId", userId) });

            ViewBag.CardNumber = list.Count();
            ViewBag.CardList = list;
            ViewBag.MinExtractCash = ConfigurationManager.AppSettings["minExtractCash"];
            ViewBag.MyCommission = GetMyCommission(userId);

            return View();
        }

        /// <summary>
        /// 根据用户Id获取我的可用佣金数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private int GetMyCommission(int userId)
        {
            var sql1 = @"select isnull(sum([Money]),0)as MyYj from ComeOutRecord c 
                        inner join BettingRecord b on c.OrderId = b.Id
                        where b.[UserId]=@UserId and Type in(4,9)";

            var sql2 = @"select isnull(sum([Money]),0) from ComeOutRecord 
                        where UserId = @UserId and ((Type = 2 and State in(1,3)) or Type in(3,5))";

            SqlParameter[] regsp = new SqlParameter[]
            {
                new SqlParameter("@UserId",userId)
            };

            //我获取到的佣金
            var obtainCommission = Convert.ToInt32(SqlHelper.ExecuteScalar(sql1, regsp));

            //我已提现的佣金
            var useCommission = Convert.ToInt32(SqlHelper.ExecuteScalar(sql2, regsp));

            return obtainCommission - useCommission;
        }

        public static object _lock = new object();

        /// <summary>
        /// 添加提现记录
        /// </summary>
        /// <returns></returns>
        public ActionResult AddExtractCash(int backId, int money)
        {
            var status = 0;
            try
            {
                lock (_lock)
                {                    
                    int userId = UserHelper.GetByUserId();

                    var minExtractCash = int.Parse(ConfigurationManager.AppSettings["minExtractCash"]);
                    var myCommission = GetMyCommission(userId);

                    //后台判断提现佣金是否正确
                    if (money > myCommission || money < minExtractCash)
                    {
                        status = 2;
                    }
                    else
                    {
                        //插入提现记录语句
                        string sql = @"insert into ComeOutRecord (UserId,OrderId,Money,Type,SubTime,PayType,State) 
                            values(@UserId,@OrderId,@Money,2,GETDATE(),3,1);";

                        //修改用户表中Coin字段
                        sql += "update userinfo set Coin = Coin-@Money where Id=@UserId;";

                        SqlParameter[] regsp = new SqlParameter[] {
                            new SqlParameter("@UserId",userId),
                            new SqlParameter("@OrderId",backId),
                            new SqlParameter("@Money",money)
                        };

                        SqlHelper.ExecuteTransaction(sql, regsp);
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                status = 0;
            }

            return Json(new { Status = status });
        }

        /// <summary>
        /// 提现成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExtractCashSuccess(int money)
        {
            ViewBag.Money = money;
            return View();
        }
    }
}
