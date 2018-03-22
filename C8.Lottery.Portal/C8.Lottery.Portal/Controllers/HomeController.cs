using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.Lottery.Model;
using C8.Lottery.Public;
using System.Data.SqlClient;

namespace C8.Lottery.Portal.Controllers
{
    public class HomeController : Controller
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

        public ActionResult Register()
        {


            return View();
        }

      
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            //UserInfo user = new UserInfo();
            //user.UserName = reguser.Mobile;
            //user.Mobile = reguser.Mobile;
            //user.Password = reguser.Password;
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
                            string regsql = @"
  insert into UserInfo(UserName, Name, Password, Mobile, Coin, Money, Integral, SubTime, LastLoginTime, State)
  values(@UserName, @Name, @Password, @Mobile, 0,0, 0, getdate(), getdate(), 0)";
                            SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@UserName",mobile),
                     new SqlParameter("@Name",mobile),
                    new SqlParameter("@Password",password),
                    new SqlParameter("@Mobile",mobile)

                 };
                            int data = SqlHelper.ExecuteNonQuery(regsql, regsp);
                            if (data > 0)
                            {
                                jsonmsg.Success = true;
                                jsonmsg.Msg = "ok";

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
        /// 登录KCP
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logins(string mobile,string password)
        {
               string usersql = "select * from UserInfo where Mobile =@Mobile";
           
                UserInfo user = new UserInfo();
                ReturnMessageJson jsonmsg = new ReturnMessageJson();
            try
            {

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Mobile", mobile) };
                user = Util.ReaderToModel<UserInfo>(usersql, sp);
                if (user != null)
                {
                    if (Tool.GetMD5(password) != user.Password)
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
                         
                            Session["UserInfo"] = user;
                            jsonmsg.Success = true;
                            jsonmsg.Msg = "ok";
                            string editsql = "update UserInfo set LastLoginTime=getdate() where Mobile=@Mobile";//记录最后一次登录时间
                            SqlParameter[] editsp = new SqlParameter[] { new SqlParameter("@Mobile", mobile) };
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
                jsonmsg.Msg =e.Message;
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
        public ActionResult Validate(string mobile,string vcode)
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
                    jsonmsg.Msg =e.Message;
                    throw;
                }
               
            }
            return Json(jsonmsg);
        }

      
    }
}
