using C8.Lottery.Model;
using C8.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using C8.Lottery.Model.Enum;

namespace C8.Lottery.Portal
{
    public static class UserHelper
    {

        public static UserInfo LoginUser
        {
            get
            {
                string guid = HttpContext.Current.Request["UserId"];
                object userId = CacheHelper.GetCache(guid);
                int userId2 = Convert.ToInt32(userId);
                return Util.GetEntityById<UserInfo>(userId2);
            }
        }

        /// <summary>
        /// 获取用户ID 
        /// </summary>
        /// <returns></returns>
        public static int GetByUserId()
        {
            string guid = HttpContext.Current.Request["UserId"];
            object userId = CacheHelper.GetCache(guid);
            int userId2 = Convert.ToInt32(userId);
            return userId2;
        }

        /// <summary>
        /// 获取当前登录用户的完整信息，包含用户头像，粉丝等信息
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetUser()
        {
            int userId = GetByUserId();
            return GetUser(userId);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetUser(int userId)
        {

            string usersql = @"select (select count(1)from Follow where UserId=u.Id and Status=1)as follow,(select count(1)from Follow where Followed_UserId=u.Id and Status=1)as fans,(select count(1) from UserCoupon where UserId=u.Id and State=1 and getdate()<EndTime)as usercoupon, r.RPath as Headpath,u.* from UserInfo  u 
                              left  JOIN (select RPath,FkId from ResourceMapping where Type = @Type)  r 
                              on u.Id=r.FkId  where u.Id=@userId ";

            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            UserInfo user = new UserInfo();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@userId", userId), new SqlParameter("@Type", (int)ResourceTypeEnum.用户头像) };
                List<UserInfo> list = Util.ReaderToList<UserInfo>(usersql, sp);
                if (list != null)
                {
                    user = list.FirstOrDefault(x => x.Id == userId);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return user;
        }

        /// <summary>
        /// 获取用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static UserState GetUserState(int userId)
        {
            string usersql = @"select top(1) * from UserState where UserId = @UserId ";

            var userState = new UserState();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@UserId", userId)};
                List<UserState> list = Util.ReaderToList<UserState>(usersql, sp);
                userState = list.FirstOrDefault();
                if (userState == null)
                {
                    userState = new UserState();
                }

                //获取积分大于100分且排在第一未的彩种
                usersql = @"select top(1) tab.Name from (
	                        select sum(br.Score) as Score , lt.Name from BettingRecord br
	                        left join LotteryType2 lt on br.lType= lt.lType
	                        where br.UserId = @UserId
	                        group by lt.Name
                        ) as tab 
                        where tab.Score>=10
                        order by score desc";

                userState.MasterLottery = Convert.ToString(SqlHelper.ExecuteScalar(usersql, sp)).Replace("(PC蛋蛋)", "");

                return userState;
            }
            catch (Exception)
            {
                return userState;
            }            
        }

        /// <summary>
        /// 更新缓存用户信息
        /// </summary>
        /// <returns></returns>
        public static void UpdateUser(UserInfo u)
        {
            string sessionId = HttpContext.Current.Request["sessionId"];
            MemClientFactory.WriteCache(sessionId, u, 30);

        }
    }
}