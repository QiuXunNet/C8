using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;
using System.Data.SqlClient;
using C8.Lottery.Portal;
using CryptSharp;

namespace C8.Lottery.Portal.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //1.最后一期开奖号码
            string sql = "";

            List<LotteryRecord> list = new List<LotteryRecord>();

            for (int i = 0; i < 65; i++)
            {
                sql = "select top(1)* from LotteryRecord where lType = " + (i + 1) + " order by Id desc";
                list.Add(Util.ReaderToModel<LotteryRecord>(sql));
            }

            ViewBag.openList = list;
            ViewBag.UserInfo = UserHelper.LoginUser;
            ViewBag.SiteSetting = GetSiteSetting();
            return View();
        }


        public ActionResult GetRemainOpenTimeByType(int lType)
        {
            string time = Util.GetOpenRemainingTimeWithHour(lType);
            string[] arr = time.Split('&');

            if (arr.Length == 3)
            {
                time = "<t class='hour'>" + arr[0] + "</t>:<t class='minute'>" + arr[1] + "</t>:<t class='second'>" + arr[2] + "</t>";
            }

            return Content(time);
        }



        /// <summary>
        /// 获取验证码KCP
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult GetCode(string mobile)
        {
            try
            {
                bool result;
                //随机生成短信验证码    
                string code = SmsSender.GetVCode();
                // 短信验证码存入session(session的默认失效时间30分钟) 
                Session.Add("code", code);
                Session["CodeTime"] = DateTime.Now;//存入时间一起存到Session里
                // 短信内容+随机生成的6位短信验证码              
                // 单个手机号发送短信  去掉注释可开启短信功能
                if (SmsSender.SendMsgByTXY("", mobile, code) == 0)
                {
                    result = true;// 成功    
                }
                else
                {
                    result = false;//失败    
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 注册 KCP  
        /// </summary>
        /// <returns></returns>

        public ActionResult Register(int? id)
        {

            ViewData["id"] = id == null ? 0 : id;
            return View();


        }


        [HttpPost]

        public ActionResult Create(FormCollection form)
        {



            int inviteid = Convert.ToInt32(form["inviteid"]);

            string mobile = form["mobile"];
            string password = form["password"];

            string vcode = form["vcode"];
            string usersql = "select * from UserInfo where Mobile =@Mobile";

            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            try
            {
                List<UserInfo> list = new List<UserInfo>();
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Mobile", mobile) };
                list = Util.ReaderToList<UserInfo>(usersql, sp);

                if (list.Count > 0)
                {
                    jsonmsg.Success = false;
                    jsonmsg.Msg = "该手机号已被注册";
                }
                else
                {
                    if (Session["code"] != null && Session["CodeTime"] != null)
                    {
                        string code = Session["code"].ToString();
                        DateTime time = (DateTime)Session["CodeTime"];

                        if (vcode != code || time.AddSeconds(60) < DateTime.Now)
                        {
                            jsonmsg.Success = false;
                            jsonmsg.Msg = "短信验证码输入不正确";

                        }
                        else
                        {
                            password = Tool.GetMD5(password);
                            string ip = Tool.GetIP();
                            string regsql = @"
  insert into UserInfo(UserName, Name, Password, Mobile, Coin, Money, Integral, SubTime, LastLoginTime, State,Pid,RegisterIP)
  values(@UserName, @Name, @Password, @Mobile, 0,0, 0, getdate(), getdate(), 0,@Pid,@RegisterIP);select @@identity ";
                            SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@UserName",mobile),
                     new SqlParameter("@Name",mobile),
                    new SqlParameter("@Password",password),
                    new SqlParameter("@Mobile",mobile),
                    new SqlParameter("@Pid",inviteid),
                    new SqlParameter("@RegisterIP",ip)

                 };
                            int data = Convert.ToInt32(SqlHelper.ExecuteScalar(regsql, regsp));
                            if (data > 0)
                            {


                                jsonmsg.Success = true;
                                jsonmsg.Msg = "ok";
                                string guid = Guid.NewGuid().ToString();
                                Response.Cookies["UserId"].Value = guid;
                                CacheHelper.SetCache(guid, data, DateTime.Now.AddMinutes(30));
                                if (inviteid > 0)
                                {
                                    UserInfo invite = GetByid(inviteid);
                                    if (invite != null)
                                    {
                                        int mynum = GetNum(3);
                                        AddCoin(data, mynum);//受邀自己得3级奖励
                                        AddCoinRecord(2, data, inviteid, mynum);//受邀得奖记录
                                        int upnum = GetNum(1);
                                        AddCoin(Convert.ToInt32(invite.Id), upnum);//上级得奖
                                        AddCoinRecord(1, inviteid, data, upnum);//上级得奖记录
                                        UserInfo super = GetByid(Convert.ToInt32(invite.Pid));//上上级
                                        if (super != null)
                                        {
                                            int supernum = GetNum(2);
                                            AddCoin(Convert.ToInt32(super.Id), supernum);//上上级得奖
                                        }

                                    }
                                }

                            }
                            else
                            {
                                jsonmsg.Success = false;
                                jsonmsg.Msg = "fail";

                            }

                        }
                    }
                    else
                    {
                        jsonmsg.Success = false;
                        jsonmsg.Msg = "请重新获取验证码";

                    }


                }
            }
            catch (Exception e)
            {
                jsonmsg.Success = false;
                jsonmsg.Msg = e.Message;
                throw;
            }

            return Json(jsonmsg);
        }
        /// <summary>
        /// 增加金币
        /// </summary>
        public void AddCoin(int id, int num)
        {
            try
            {
                string strsql = "update UserInfo set Coin=Coin+@num where Id =@Id";
                SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@num", num),
                    new SqlParameter("@Id", id),
                };
                SqlHelper.ExecuteNonQuery(strsql, sp);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 邀请获得金币记录 type:1邀请者  2受邀者
        /// </summary>
        public void AddCoinRecord(int type, int userId, int otherId, int amount)
        {
            try
            {
                string strsql = @"insert into CoinRecord(lType, UserId, OtherId, Type, Amount, SubTime)
                                  values(0, @UserId, @OtherId, @Type, @Amount, getdate()) ";
                SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@OtherId", otherId),
                    new SqlParameter("@Type", type),
                    new SqlParameter("@Amount", amount),
                };
                SqlHelper.ExecuteNonQuery(strsql, sp);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 随机抽取金币 根据金币等级
        /// </summary>
        /// <returns></returns>
        public int GetNum(int GradeId)
        {
            int Num = 0;
            List<CoinRate> list = new List<CoinRate>();
            List<int> listNum = new List<int>();
            try
            {
                string strsql = "select * from CoinRate where GradeId = @GradeId";
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@GradeId", GradeId) };
                list = Util.ReaderToList<CoinRate>(strsql, sp);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        for (int i = 0; i < item.Rate; i++)
                        {
                            listNum.Add(item.Num);
                        }

                    }
                }
                Random rm = new Random();
                int j = rm.Next(listNum.Count);
                Num = listNum[j];

            }
            catch (Exception)
            {

                throw;
            }
            return Num;
        }


        /// <summary>
        /// 根据id获取用户信息
        /// </summary>
        /// <param name="Pid"></param>
        /// <returns></returns>
        public UserInfo GetByid(int id)
        {
            UserInfo user = new UserInfo();
            try
            {
                string usersql = "select * from UserInfo where Id =@Id";
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Id", id) };
                user = Util.ReaderToModel<UserInfo>(usersql, sp);


            }
            catch (Exception)
            {

                throw;
            }
            return user;

        }





        /// <summary>
        /// 登录KCP
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Logins(string mobile, string password)
        {
            //string usersql = "select * from UserInfo where Mobile =@Mobile";
            string usersql = @"select * from UserInfo where Mobile=@Mobile ";
            UserInfo user = new UserInfo();
            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            try
            {

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Mobile", mobile) };
                List<UserInfo> list = Util.ReaderToList<UserInfo>(usersql, sp);
                if (list != null)
                {
                    user = list.FirstOrDefault(x => x.Mobile == mobile);
                }

                if (user != null)
                {
                    bool iscor = true;
                    if (password.Contains("$2y"))
                    {
                        iscor = Crypter.CheckPassword(password, user.Password);
                    }

                    if (Tool.GetMD5(password) != user.Password && !iscor)
                    {
                        jsonmsg.Success = false;
                        jsonmsg.Msg = "密码不正确";

                    }
                    else
                    {
                        if (user.State != 0)
                        {
                            jsonmsg.Success = false;
                            jsonmsg.Msg = "账号被禁、暂时无法登录";
                        }
                        else
                        {

                            string guid = Guid.NewGuid().ToString();
                            Response.Cookies["UserId"].Value = guid;
                            //Session[guid] = user.Id;

                            //MemClientFactory.WriteCache<string>(sessionId.ToString(), user.Id.ToString(), 30);
                            CacheHelper.SetCache(guid, user.Id, DateTime.Now.AddMinutes(30));


                            jsonmsg.Success = true;
                            jsonmsg.Msg = "ok";
                            string ip = Tool.GetIP();
                            string editsql = "update UserInfo set LastLoginTime=getdate(),LastLoginIP=@LastLoginIP where Mobile=@Mobile";//记录最后一次登录时间
                            SqlParameter[] editsp = new SqlParameter[] { new SqlParameter("@Mobile", mobile), new SqlParameter("@LastLoginIP", ip) };
                            SqlHelper.ExecuteNonQuery(editsql, editsp);
                        }

                    }
                }
                else
                {
                    jsonmsg.Success = false;
                    jsonmsg.Msg = "手机号不存在";

                }
            }
            catch (Exception e)
            {
                jsonmsg.Success = false;
                jsonmsg.Msg = e.Message;
                throw;
            }


            return Json(jsonmsg);
        }



        /// <summary>
        /// 忘记密码 KCP
        /// </summary>
        /// <returns></returns>
        public ActionResult Forget()
        {
            return View();

        }
        /// <summary>
        /// 验证 KCP
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ActionResult Validate(string mobile, string vcode)
        {
            string usersql = "select * from UserInfo where Mobile =@Mobile";

            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            try
            {
                List<UserInfo> list = new List<UserInfo>();
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Mobile", mobile) };
                list = Util.ReaderToList<UserInfo>(usersql, sp);

                if (list.Count < 0)
                {
                    jsonmsg.Msg = "该手机未注册、请先注册账号";
                    jsonmsg.Success = false;
                }
                else
                {
                    if (Session["code"] != null && Session["CodeTime"] != null)
                    {
                        string code = Session["code"].ToString();
                        DateTime time = (DateTime)Session["CodeTime"];


                        if (vcode != code || time.AddSeconds(60) < DateTime.Now)
                        {
                            jsonmsg.Success = false;
                            jsonmsg.Msg = "短信验证码输入不正确";

                        }
                        else
                        {
                            Session["Mobile"] = mobile;
                            jsonmsg.Success = true;
                            jsonmsg.Msg = "ok";
                        }
                    }
                    else
                    {
                        jsonmsg.Success = false;
                        jsonmsg.Msg = "请重新获取验证码";
                    }
                }
            }
            catch (Exception e)
            {
                jsonmsg.Success = false;
                jsonmsg.Msg = e.Message;
                throw;
            }
            return Json(jsonmsg);

        }



        /// <summary>
        /// 设置密码
        /// </summary>
        /// <returns></returns>
        public ActionResult SetPassword()
        {
            if (Session["Mobile"] == null)
            {
                return RedirectToAction("Forget");
            }
            else
            {
                return View();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public ActionResult SetPw(string password)
        {
            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            if (Session["Mobile"] != null)
            {

                try
                {
                    string mobile = Session["Mobile"].ToString();
                    password = Tool.GetMD5(password);
                    string usersql = "update UserInfo set[Password] = @Password where Mobile=@Mobile";
                    SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@Mobile",mobile),
                    new SqlParameter("@Password",password)
                };
                    int data = SqlHelper.ExecuteNonQuery(usersql, sp);
                    if (data > 0)
                    {
                        jsonmsg.Msg = "ok";
                        jsonmsg.Success = true;
                    }
                    else
                    {
                        jsonmsg.Success = false;
                        jsonmsg.Msg = "fail";

                    }
                }
                catch (Exception e)
                {
                    jsonmsg.Success = false;
                    jsonmsg.Msg = e.Message;
                    throw;
                }

            }
            return Json(jsonmsg);
        }



        /// <summary>
        /// 计划
        /// </summary>
        /// <returns></returns>
        public ActionResult Plan()
        {
            return View();
        }

        /// <summary>
        /// 开奖
        /// </summary>
        /// <returns></returns>
        public ActionResult Open()
        {

            string sql = "";

            List<LotteryRecord> list = new List<LotteryRecord>();

            for (int i = 0; i < 65; i++)
            {
                sql = "select top(1)* from LotteryRecord where lType = " + (i + 1) + " order by Id desc";
                list.Add(Util.ReaderToModel<LotteryRecord>(sql));
            }

            ViewBag.openList = list;
            return View();
        }





    }
}
