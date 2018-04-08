using C8.Lottery.Model;
using C8.Lottery.Public;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace C8.Lottery.Portal.Controllers
{
    /// <summary>
    /// 聊天室控制器
    /// 卢晨
    /// 2018-03-29
    /// </summary>
    public class TalkingController : BaseController
    {
        
        /// <summary>
        /// 聊天室列表业
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 具体聊天页面
        /// </summary>
        /// <param name="id">聊天室Id</param>
        /// <returns></returns>
        public ActionResult ChatRoom(int id)
        {
            try
            {
                int userId = UserHelper.GetByUserId();
                UserInfo user = UserHelper.GetUser(userId);

                ViewBag.RommId = id;
                if (user == null)
                {
                    ViewBag.UserId = 9998;  
                    ViewBag.UserName = "测试用户";
                    ViewBag.PhotoImg = "/images/default_avater.png";
                    ViewBag.IsAdmin = true;
                }
                else
                {
                    ViewBag.UserId = user.Id;
                    ViewBag.UserName = user.UserName;
                    ViewBag.PhotoImg = string.IsNullOrEmpty(user.Headpath)? "/images/default_avater.png": user.Headpath;//user.;
                    ViewBag.IsAdmin = false; //
                }

                switch (id)
                {
                    case 1:
                        ViewBag.RommName = "综合";
                        break;
                    case 2:
                        ViewBag.RommName = "答题";
                        break;
                    case 3:
                        ViewBag.RommName = "双色球";
                        break;
                    case 4:
                        ViewBag.RommName = "福彩3D";
                        break;
                    case 5:
                        ViewBag.RommName = "排列三";
                        break;
                    case 6:
                        ViewBag.RommName = "排列五";
                        break;
                    case 7:
                        ViewBag.RommName = "六合彩";
                        break;
                    case 8:
                        ViewBag.RommName = "十一选五";
                        break;
                    case 9:
                        ViewBag.RommName = "快乐十分";
                        break;
                    case 10:
                        ViewBag.RommName = "时时彩";
                        break;
                    case 11:
                        ViewBag.RommName = "快三";
                        break;
                    case 12:
                        ViewBag.RommName = "幸运飞艇";
                        break;
                    case 13:
                        ViewBag.RommName = "北京赛车";
                        break;
                    case 14:
                        ViewBag.RommName = "七星彩";
                        break;
                    case 15:
                        ViewBag.RommName = "七乐彩";
                        break;
                    default:
                        ViewBag.RommName = "";
                        break;
                }

                //查询登录人在本房间是否被禁言
                string sql = "select count(*) from TalkBlackList where RoomId = @RoomId and UserId = @UserId and (IsEverlasting =1 or EndTime > GETDATE())";

                SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@RoomId",id),
                    new SqlParameter("@UserId",ViewBag.UserId)
                 };

                var i = Convert.ToInt32(SqlHelper.ExecuteScalar(sql, regsp));

                ViewBag.IsEverlasting = (i > 0)?1:0;
            }
            catch (Exception)
            {               
            }

            return View("ChatRoomWS");
        }

        /// <summary>
        /// 处理记录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ManagementList(int id)
        {
            ViewBag.RoomId = id;
            return View();
        }

        /// <summary>
        /// 发送图片时保存图片到服务器
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public ActionResult SaveImg(string img)
        {
            var xPath = "/Upload/TalkingImg/";
            var datePath = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "/";
            string path = Server.MapPath(xPath) + datePath;//设定上传的文件路径
            string fileName = Guid.NewGuid().ToString();

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                byte[] arr = Convert.FromBase64String(img);

                MemoryStream ms = new MemoryStream(arr);            
                Image image = Image.FromStream(ms);
                image.Save(path+fileName+".jpg");
                image.Dispose();
                ms.Close();

                MemoryStream ms2 = new MemoryStream(ConvertToThumbnail(arr,70,100,100));
                Image image2 = Image.FromStream(ms2);
                image2.Save(path + fileName + "_Min.jpg");
                image2.Dispose();
                ms2.Close();
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Base64StringToImage 转换失败\nException：" + ex.Message);
            }

            return Json(xPath + datePath + fileName + "_Min.jpg");
        }

        /// <summary>
        /// 图像缩略图处理
        /// </summary>
        /// <param name="bytes">图像源数据</param>
        /// <param name="compression">压缩质量 1-100</param>
        /// <param name="thumbWidth">缩略图的宽</param>
        /// <param name="thumbHeight">缩略图的高</param>
        /// <returns></returns>
        private byte[] ConvertToThumbnail(byte[] bytes, int compression = 100, int thumbWidth = 0, int thumbHeight = 0)
        {
            byte[] bs = null;

            try
            {
                if (bytes != null)
                {
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        using (Bitmap srcimg = new Bitmap(ms))
                        {
                            if (thumbWidth == 0 && thumbHeight == 0)
                            {
                                thumbWidth = srcimg.Width;
                                thumbHeight = srcimg.Height;
                            }
                            if (srcimg.Width > srcimg.Height)
                            {
                                thumbHeight = thumbHeight * srcimg.Height / srcimg.Width;
                            }
                            else
                            {
                                thumbWidth = thumbWidth * srcimg.Width / srcimg.Height;
                            }

                            using (Bitmap dstimg = new Bitmap(thumbWidth, thumbHeight))//图片压缩质量
                            {
                                //从Bitmap创建一个System.Drawing.Graphics对象，用来绘制高质量的缩小图。
                                using (Graphics gr = Graphics.FromImage(dstimg))
                                {
                                    //把原始图像绘制成上面所设置宽高的缩小图
                                    Rectangle rectDestination = new Rectangle(0, 0, thumbWidth, thumbHeight);
                                    gr.Clear(Color.WhiteSmoke);
                                    gr.CompositingQuality = CompositingQuality.HighQuality;
                                    gr.SmoothingMode = SmoothingMode.HighQuality;
                                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    gr.DrawImage(srcimg, rectDestination, 0, 0, srcimg.Width, srcimg.Height, GraphicsUnit.Pixel);

                                    EncoderParameters ep = new EncoderParameters(1);
                                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, compression);//设置压缩的比例1-100
                                    ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                                    ImageCodecInfo jpegICIinfo = arrayICI.FirstOrDefault(t => t.FormatID == System.Drawing.Imaging.ImageFormat.Png.Guid);
                                    using (MemoryStream dstms = new MemoryStream())
                                    {
                                        if (jpegICIinfo != null)
                                        {
                                            dstimg.Save(dstms, jpegICIinfo, ep);
                                        }
                                        else
                                        {
                                            dstimg.Save(dstms, System.Drawing.Imaging.ImageFormat.Png);//保存到内存里
                                        }
                                        bs = new Byte[dstms.Length];
                                        dstms.Position = 0;
                                        dstms.Read(bs, 0, bs.Length);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return bs;
        }

        /// <summary>
        /// 添加聊天记录
        /// </summary>
        /// <param name="model"></param>
        public void AddMessage(TalkNotes model)
        {
            model.SendTime = DateTime.Now;
            model.Status = 1;           

            try
            {
                string regsql = @"insert into TalkNotes (Content,UserId,UserName,PhotoImg,SendTime,RoomId,MsgTypeChild,Status,Guid,IsAdmin)
                                values (@Content,@UserId,@UserName,@PhotoImg,@SendTime,@RoomId,@MsgTypeChild,@Status,@Guid,@IsAdmin);";
                SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@Content",model.Content),
                    new SqlParameter("@UserId",model.UserId),
                    new SqlParameter("@UserName",model.UserName),
                    new SqlParameter("@PhotoImg",model.PhotoImg??""),
                    new SqlParameter("@SendTime",model.SendTime),
                    new SqlParameter("@RoomId",model.RoomId),
                    new SqlParameter("@MsgTypeChild",model.MsgTypeChild),
                    new SqlParameter("@Status",model.Status),
                    new SqlParameter("@Guid",model.Guid),
                    new SqlParameter("@IsAdmin",model.IsAdmin)
                 };

                SqlHelper.ExecuteScalar(regsql, regsp);
            }
            catch (Exception ex)
            {               
            }            
        }

        /// <summary>
        /// 根据前端Guid和房间号获取聊天记录
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult GetMessageList(int roomId, string guid = "")
        {
            string sql = @"select top 20 * from TalkNotes 
                            where RoomId = @RoomId and Status = 1 {0}                            
                            order by id desc ";

            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@RoomId", roomId),new SqlParameter("@Guid",guid) };

            if (!string.IsNullOrEmpty(guid))
            {
                sql = string.Format(sql, " and id<(select id from TalkNotes where RoomId = @RoomId and Status = 1 and Guid = @Guid) ");
            }
            else
            {
                sql = string.Format(sql, " ");
            }

            try
            {
                var list = Util.ReaderToList<TalkNotes>(sql, sp);
                list.ForEach(e => e.SendTimeStr = e.SendTime.ToString("HH:mm"));
                return Json(new { Status = 1, DataList = list });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, DataList = new List<TalkNotes>() });
            }
        }

        /// <summary>
        /// 管理员删除消息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult DelMessage(string guid,int userId,string userName)
        {
            string sql = @" update TalkNotes set Status = 0 where Guid=@Guid ";

            try
            {
                int i = SqlHelper.ExecuteNonQuery(sql, new SqlParameter[] { new SqlParameter("@Guid", guid) });

                var processingRecords = new ProcessingRecords()
                {
                    ProcessToId = userId,
                    ProcessToName = userName,
                    Type = 1
                };

                AddProcessingRecords(processingRecords);

                if (i > 0)
                {
                    return Json(new { status = 1 });
                }
                else
                {
                    return Json(new { status = 0 });
                }
            }
            catch (Exception)
            {
                return Json(new { status = 0 });
            }
        }

        /// <summary>
        /// 加入黑名单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddBlackList(TalkBlackList model,string userName)
        {         
            model.BanTime = DateTime.Now;
            model.IsEverlasting = true;

            try
            {
                string regsql = @"insert into TalkBlackList (UserId,RoomId,BanTime,IsEverlasting) 
                                    values (@UserId,@RoomId,@BanTime,@IsEverlasting); ";
                SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@UserId",model.UserId),
                    new SqlParameter("@RoomId",model.RoomId),
                    new SqlParameter("@BanTime",model.BanTime),
                    new SqlParameter("@IsEverlasting",model.IsEverlasting)                  
                 };

                SqlHelper.ExecuteScalar(regsql, regsp);

                var processingRecords = new ProcessingRecords()
                {
                    ProcessToId = model.UserId,
                    ProcessToName = userName,
                    Type = 2
                };

                AddProcessingRecords(processingRecords);

                return Json(new { status = 1 });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0 });
            }
        }

        /// <summary>
        /// 插入处理记录
        /// </summary>
        /// <param name="model"></param>
        private void AddProcessingRecords(ProcessingRecords model)
        {
            int userId = UserHelper.GetByUserId();
            UserInfo user = UserHelper.GetUser(userId);

            if (user == null)
            {
                user = new UserInfo()
                {
                    UserName = "测试用户",
                    Id = 0
                };
            }           

            model.ProcessDate = DateTime.Now;
            model.ProcessTime = DateTime.Now;
            model.ProcessId = (int)user.Id;
            model.ProcessName = user.UserName;

            try
            {
                string regsql = @"insert into ProcessingRecords (ProcessId,ProcessName,Type,ProcessToId,ProcessToName,ProcessDate,ProcessTime)
                                values (@ProcessId,@ProcessName,@Type,@ProcessToId,@ProcessToName,@ProcessDate,@ProcessTime)";
                SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@ProcessId",model.ProcessId),
                    new SqlParameter("@ProcessName",model.ProcessName),
                    new SqlParameter("@Type",model.Type),
                    new SqlParameter("@ProcessToId",model.ProcessToId),
                    new SqlParameter("@ProcessToName",model.ProcessToName),
                    new SqlParameter("@ProcessDate",model.ProcessDate),
                    new SqlParameter("@ProcessTime",model.ProcessTime)
                 };

                SqlHelper.ExecuteScalar(regsql, regsp);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetProcessingRecords(int id = 0)
        {
            try
            {
                string sql = "select top(20) * from ProcessingRecords {0} order by ProcessTime desc";

                if (id == 0)
                {
                    sql = string.Format(sql, "");
                }
                else
                {
                    sql = string.Format(sql, " where Id <@Id ");
                }

                var list = Util.ReaderToList<ProcessingRecords>(sql, new SqlParameter[] { new SqlParameter("@Id", id) });

                List<dynamic> dyList = new List<dynamic>();

                list.ForEach(e =>
                {
                    dyList.Add(new
                    {
                        Id = e.Id,
                        Date = e.ProcessDate.ToString("yyyy-MM-dd"),
                        Message = "管理员对用户\"" + e.ProcessToName + "\"进行" + (e.Type == 1 ? "删除消息" : "拉黑") + "处理",
                        Time = e.ProcessTime.ToString("HH:mm")
                    });
                });

                return Json(new { Status = 1, DataList = dyList });
            }
            catch (Exception)
            {
                return Json(new { Status = 0, DataList = new { } });
            }
           
        }

        /// <summary>
        /// 获取屏蔽字
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSensitiveWordsList()
        {
            //屏蔽字一般不会变动，为减少数据库操作，加入2小时缓存
            var str = "";
            if (CacheHelper.GetCache("GetSensitiveWordsList") == null)
            {
                string sql = " select content from SensitiveWords ";
                str = Convert.ToString(SqlHelper.ExecuteScalar(sql));

                CacheHelper.AddCache("GetSensitiveWordsList", str,DateTime.Now.AddHours(2));
            }
            else
            {
                str = CacheHelper.GetCache("GetSensitiveWordsList").ToString();
            }

            if (string.IsNullOrEmpty(str))
            {
                return Json(str.Split(','));
            }
            else
            {
                return Json(new { });
            }
        }
    }
}
