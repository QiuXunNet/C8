using C8.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace C8.Lottery.Portal.Controllers
{
    public class PvUvIpController : BaseController
    {
        
        string interfaceUrl = ConfigurationManager.AppSettings["InterfaceUrl"];
        /// <summary>
        /// 部分页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var linkCode = Session["LinkCode"];
            ViewBag.LinkCode = linkCode == null ? "" : linkCode;
            return View();
        }

        public void AddUv(string linkCode, string type)
        {
            var url = interfaceUrl + "/FriendshipLinks/Add?linkCode="+linkCode+"&type="+ type;
            HttpCommon.HttpGet(url);            
        }

        public void AddIp(string linkCode, string type,string ip)
        {
            var url = interfaceUrl + "/FriendshipLinks/Add?linkCode=" + linkCode + "&ip=" + ip + "&type=" + type;
            HttpCommon.HttpGet(url);
        }

        public void AddPv(string linkCode, string type)
        {
            var url = interfaceUrl + "/FriendshipLinks/Add?linkCode=" + linkCode + "&type=" + type;
            HttpCommon.HttpGet(url);
        }
    }
}
