﻿using C8.Lottery.Model;
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
            Add(type, linkCode);
        }

        public void AddIp(string linkCode, string type, string ip)
        {
            Add(type, linkCode, ip);
        }

        public void AddPv(string linkCode, string type)
        {
            Add(type, linkCode);
        }


        private void Add(string type, string linkCode, string ip = "")
        {
            FriendLink model = GetFriendLink(linkCode);
            if (model != null && model.Id > 0)
            {
                if (type == "ip")
                {
                    var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                    string cachekeyip = string.Format(RedisKeyConst.FriendshipLinks_LinkIP, linkCode, ip); //"friendshipLinks:IP: " + linkCode + ":" + ip;
                    // var ipsObj = CacheHelper.GetCache<string>("FriendshipLinksControllerIpList" + linkCode);
                    //var ips = ipsObj == null ? "" : ipsObj.ToString();
                    //if (string.IsNullOrEmpty(ips) || ips.IndexOf("," + ip + ",") == -1)
                    //if (!CacheHelper.IsSet("IP:" + linkCode + ":FriendshipLinksControllerIp" + ip))
                    if (!CacheHelper.IsSet(cachekeyip))
                    {
                        //if (string.IsNullOrEmpty(ips))
                        //{
                        //    CacheHelper.SetCache("FriendshipLinksControllerIpList" + linkCode, "," + ip + ",", endDate);
                        //}
                        //else
                        //{
                        //    CacheHelper.SetCache("FriendshipLinksControllerIpList" + linkCode, ips + ip + ",", endDate);
                        //}
                        CacheHelper.SetCache(cachekeyip, "1", endDate);

                        #region 向缓存中增加IP数
                        string cacheklinkcode = string.Format(RedisKeyConst.FriendshipLinks_LinkIPs, linkCode); //"friendshipLinks:link_ip:" + linkCode;
                        //var obj = CacheHelper.GetCache<int>("FriendshipLinksControllerIp" + linkCode);
                        var obj = CacheHelper.GetCache<int>(cacheklinkcode);
                        if (obj == default(int))
                        {
                            //CacheHelper.AddCache("FriendshipLinksControllerIp" + linkCode, 1);
                            CacheHelper.AddCache(cacheklinkcode, 1);
                        }
                        else
                        {
                            //var ipNum = Convert.ToInt32(CacheHelper.GetCache<int>("FriendshipLinksControllerIp" + linkCode));
                            //CacheHelper.SetCache("FriendshipLinksControllerIp" + linkCode, ipNum + 1);
                            var ipNum = Convert.ToInt32(CacheHelper.GetCache<int>(cacheklinkcode));
                            CacheHelper.SetCache(cacheklinkcode, ipNum + 1);
                        }
                        #endregion
                    }
                }
                if (type == "uv")
                {
                    #region 向缓存中增加PV数
                    string cacheklinkcode = string.Format(RedisKeyConst.FriendshipLinks_LinkUVs, linkCode); //"friendshipLinks:link_uv:" + linkCode;
                    //var obj = CacheHelper.GetCache<int>("FriendshipLinksControllerUv" + linkCode);
                    var obj = CacheHelper.GetCache<int>(cacheklinkcode);
                    if (obj == default(int))
                    {
                        //CacheHelper.AddCache("FriendshipLinksControllerUv" + linkCode, 1);
                        CacheHelper.AddCache(cacheklinkcode, 1);
                    }
                    else
                    {
                        //var uvNum = Convert.ToInt32(CacheHelper.GetCache<int>("FriendshipLinksControllerUv" + linkCode));
                        //CacheHelper.SetCache("FriendshipLinksControllerUv" + linkCode, uvNum + 1);
                        var uvNum = Convert.ToInt32(CacheHelper.GetCache<int>(cacheklinkcode));
                        CacheHelper.SetCache(cacheklinkcode, uvNum + 1);
                    }
                    #endregion
                }
                if (type == "pv")
                {
                    #region 向缓存中增加PV数
                    string cacheklinkcode = string.Format(RedisKeyConst.FriendshipLinks_LinkPVs, linkCode); //"friendshipLinks:link_pv:" + linkCode;
                    //var obj = CacheHelper.GetCache<int>("FriendshipLinksControllerPv" + linkCode);
                    var obj = CacheHelper.GetCache<int>(cacheklinkcode);
                    if (obj == default(int))
                    {
                        //CacheHelper.AddCache("FriendshipLinksControllerPv" + linkCode, 1);
                        CacheHelper.AddCache(cacheklinkcode, 1);
                    }
                    else
                    {
                        //var pvNum = Convert.ToInt32(CacheHelper.GetCache<int>("FriendshipLinksControllerPv" + linkCode));
                        //CacheHelper.SetCache("FriendshipLinksControllerPv" + linkCode, pvNum + 1);
                        var pvNum = Convert.ToInt32(CacheHelper.GetCache<int>(cacheklinkcode));
                        CacheHelper.SetCache(cacheklinkcode, pvNum + 1);
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 获取友情链接对象
        /// </summary>
        /// <param name="linkCode"></param>
        /// <returns></returns>
        private FriendLink GetFriendLink(string linkCode)
        {
            List<FriendLink> list = null;
            string cachekey = RedisKeyConst.FriendshipLinks_List; //"friendshipLinks:list:all";
            //var friendLinkList = CacheHelper.GetCache<List<FriendLink>>("FriendshipLinksControllerFriendLinkList");
            var friendLinkList = CacheHelper.GetCache<List<FriendLink>>(cachekey);
            if (friendLinkList == null)
            {
                list = Util.ReaderToList<FriendLink>("select * from dbo.FriendLink where [Type]=1 and state = 0 ");

                //CacheHelper.SetCache("FriendshipLinksControllerFriendLinkList", list, DateTime.Now.AddHours(2));
                CacheHelper.SetCache(cachekey, list, DateTime.Now.AddHours(2));
            }
            else
            {
                list = friendLinkList as List<FriendLink>;
            }

            return list.FirstOrDefault(e => e.Code == linkCode);
        }
    }
}
