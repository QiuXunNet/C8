using C8.Lottery.Public;
using C8.Lottery.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace C8.Lottery.Portal.Controllers
{
     public class PersonalController : FilterController
    {
        //
        // GET: /Personal/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 修改密码 KCP
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyPassword()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldpw">旧密码</param>
        /// <param name="newpw">新密码</param>
        /// <returns></returns>
        public ActionResult ModifyPWD(string oldpwd,string newpwd)
        {
            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            if (Session["UserInfo"] != null)
            {
                UserInfo user = (UserInfo)Session["UserInfo"];
                oldpwd = Tool.GetMD5(oldpwd);
                if (oldpwd != user.Password)
                {
                    jsonmsg.Success = false;
                    jsonmsg.Msg = "旧密码不对,请重新输入";
                }
                else
                {
                    newpwd = Tool.GetMD5(newpwd);
                    try
                    {
                        string usersql = "update UserInfo set [Password]=@Password where Mobile =@Mobile ";
                        SqlParameter[] sp = new SqlParameter[] {
                            new SqlParameter("@Password",newpwd),
                            new SqlParameter("@Mobile",user.Mobile)

                        };
                        int date = SqlHelper.ExecuteNonQuery(usersql, sp);
                        if(date > 0)
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
                    catch (Exception e)
                    {
                        jsonmsg.Success = false;
                        jsonmsg.Msg = e.Message;
                        throw;
                    }
                }
            }
            else
            {
                jsonmsg.Success = false;
                jsonmsg.Msg = "登录超时，请重新登录";
            }
            return Json(jsonmsg);
        }
       

        /// <summary>
        /// 设置昵称
        /// </summary>
        /// <returns></returns>
        public ActionResult SetNickName()
        {

            return View();
        }

    }
}
