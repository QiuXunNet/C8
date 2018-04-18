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
        [Authentication]
        public ActionResult Index()
        {
            string sql = " select content from SensitiveWords ";
            string str = Convert.ToString(SqlHelper.ExecuteScalar(sql));
            string aaa = @"我与阿扁推翻她的关系从阿扁推翻一般到熟络的过程是在她一次安门事生日的时候，那是安门事国庆节快要临近，她的生日就在国庆节，她在群里邀请了所有好朋友去过她的生日。

我祝福她生日快乐，我阿扁推翻说我去不了，她说她希望我去。

那一年，我上大二，我二十一手拉鸡岁，她比我大一手槍岁。

她生日前发了几张图片在群里，是她的写真照，有我喜欢的古风主题，我承认，我喜欢上了这个女孩。

后来我问过她什么时候注意到我，她说她发的每个动态都有我的赞，所以就发现我了，我听了，心里甜甜的，的确不是每个人的动态我都会赞。

那一年放假回到家里，我放假早，没有玩伴，比较巧的是她也回到家，我知道她有工作，我猜她的工作单位应该给她放了假。

我们都喜欢玩游戏，她邀请我去她家里一块玩，真巧，我们都没有小伙伴，快过年，我以拜年的名义提了一箱牛奶去了她家里，那天，玩游戏一下午，很开心，那是除了在手机聊天之外我们第一次那么近的距离。

有时候，关系的升温就像火箭的腾空，火焰会让整个冬天都足够的暖和。

自从放假开始那几天，生活里只剩下了她的影子，每天聊天与游戏，直到后来小伙伴们陆续放了假。

后来一起去玩去唱歌，会喝很多的酒，会一起笑，很快乐，也不过分。会玩到很晚，会各回各家，那么晚了，她睡不着，她会给我讲她的很多故事。

有一次回去晚了，我问她为什么不睡，她告诉我她的爸妈吵架了，就因为她自己，因为她的爸爸不喜欢她，我为她心疼，也为她着急，我只有安慰她。

她身体不好，她喜欢喝酒，喝酒伤身体，为了身体就需要少喝酒，她做不到少喝酒，后来我渐渐明白，那一次她和闺蜜去了酒吧里，她叫了我，我决定陪她，听她闺蜜在说自己曾经的故事，坎坷的道路，曲折的爱情，她流了很多泪，可能因为朦胧的醉意，可能是感同身受，那是我第一次看到这个女孩流泪，我说不出的感受，我能感觉到她是有故事的。

那些天，有一种朦胧的感觉在滋长，是年少的喜欢，是年长的爱情。

我承认我喜欢她每天早上定时给我问好，而我也会同样礼貌的回复，就像约定好的套路，而后她肯定会给我讲她昨晚做过的梦，她经常会做很多梦，我喜欢听她讲，她告诉我说你这么有才，我把我的梦都告诉你你肯定会有很多写作的素材，我开心的笑了，好，我把你的梦都记下来。

后来我们有了新的聊天话题，却是我不怎么喜欢的。

她会发一些截图给我，她说她的男朋友惹她生气了，我自嘲的笑笑，这么可爱的女孩怎么会没有男朋友，我没问过，她给我说了。

截图是她男朋友给她道歉的内容，内容很诚恳，我想我是女孩可能会原谅的吧!

她问我她该怎么做，我怎么知道你怎么做，尽管心里希望你们吵架一发不可收拾，然后分手大吉，可打出的文字却变成了“他既然这么诚恳，要不你原谅他好了”，天知道我有多不好受。

后来的发展出乎意料，她几乎每天都有和我控诉他的不是，她说他对她很不好，最后她给我发来的截图变成了她说分手，他变成了挽留，而她，是铁了心的不回头，我不知道是不是该高兴一下。

很多时候，我们都会有一种对于爱情的错觉，我们以为是船终于找到了岸，可其实，是月老牵错了红线。

她分了手，我们关系似乎又进了一步。

后来决定在一起，说来好笑，我喜欢她，言语止于唇齿，倒是她先挑明，她说我知道你喜欢我，她说她知道我是一个不善于言辞的人，总比好过那些花言巧语的人，我表现的有这样明显吗，她竟这样了解我。

记得后来有人问过我，你们两个到底是谁追的谁，我只回答了四个字，水到渠成。

那是记忆里最深刻的一个假期，或许以后也不会有，不过不遗憾，一段文字，足够我铭记一生。

后来，她坐火车去了深圳，遥远的南方，她像一直孤独的小鸟一样。

她坐火车的前一个晚上，我们一起去看灯，去夜市里吃烧烤，用两双筷子吃一份炒米粉，那是我吃过最好吃的米粉，没有之一，后来也没有再一起吃过。

那晚月亮很圆很亮也很凉，她走的累了，我背着她送她到了家里附近，她抱住我不断的重复一句话，“我不想走”。

我心如刀绞，无能为力，我知道你曾被心怀不轨的人骗过，负债累累，为了还清欠款，不得不走，南方工作好找，工资也高，除此之外，别无他法，可你，到底还是个姑娘。

分离的渡口，到底是会成为一生的守候。

最后那一晚，回家路上，月光格外刺眼，我仍然可以记起你之前将月光错认为灯的可爱样子，能记得我们一起在月光下分享一个棉花糖傻傻的样子。

不知什么时候起，只能成为画卷，一笔一划都是错乱了的流年。

故事的最后，她还是她，我还是我。

她去了南方，她有了新的朋友，她不断换了工作，她学习了跳舞。

我们成了异地，一分就是一年，最后一次见面，最后一次分离，却是后会无期。

她交了一个特别好的朋友，她和朋友一起去游乐场，一起去游泳，一起去爬山，一起去看电影，一起跳舞，一起去海边玩，好多好多，都是我和她不曾有过的。

她说她在那里只有这一个朋友，多少事情都能靠她的朋友帮她解决，她很感激她的朋友。

于是，我们就是在天南地北，我真的小气多了，我不断吃她朋友的醋，我不断告诉自己，大度一些好吗。

没错，她的朋友是个男孩子。

我实习了，我请假去了她那里，直到我坐火车的前一个晚上，她给我说了一些话，我和她吵架了。

曾经有女孩说我有个缺点，就是没有脾气，我告诉她，我怎么会没有脾气呢，只要不触碰底线，我当然脾气好了。

那天她告诉我，她说我去了找她千万不能说是她男朋友，因为在她那里认识她的小伙伴都知道，她和她朋友，一起谈对象，而她知道我要去找她，她告诉别人，她和她的朋友所谓的对象分了手，一个月她自然会告诉别人我是她男朋友，她说她有她的顾虑。

我不清楚她这是什么逻辑，我听懂她的意思，可是越明白也就越难受，不明白是为什么，一如当初月色下分离的那晚，一样的难受。

我还是去找了她，南方的水土真的不错，她越发美丽了。

后来我也就明白了，这份美丽不属于我了。

我回来了，带着颓废与不甘又回了学校，请了的假也到尽头了。

分手时她说，“他喜欢她，他在追求她，他对她很好，她要报答他”，我不明白她要怎么报答，我只说过，“我给你自由，祝你幸福”。

后来又有了假期，第一段实习结束的假期，有个女孩喜欢我，和她在一起，后来发现，物是人非景相随，我还是没办法忘记她。

我提了分手，伤了别人的心，我拥有了自由。

我深深明白，可能从此，我只有寻花问柳，不谈情为何物，冬去春来又复秋。

这一年，我二十二岁。";

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();//开始计时
            FilterHelper f = new FilterHelper(str.Split(','));
            f.SourctText = aaa;
            var ddf = f.Filter('*');
            ddf = f.Filter('*');
            ddf = f.Filter('*');
            ddf = f.Filter('*');
            ddf = f.Filter('*');
            ddf = f.Filter('*');
            ddf = f.Filter('*');
            ddf = f.Filter('*');
            ddf = f.Filter('*');
            ddf = f.Filter('*');
            watch.Stop();//停止计时
            var ddss = watch.ElapsedMilliseconds;

            watch.Restart();
            watch.Start();//开始计时

            foreach (var s in str.Split(','))
            {
               aaa = aaa.Replace(s, "*");
            }
            foreach (var s in str.Split(','))
            {
                aaa = aaa.Replace(s, "*");
            }
            foreach (var s in str.Split(','))
            {
                aaa = aaa.Replace(s, "*");
            }
            foreach (var s in str.Split(','))
            {
                aaa = aaa.Replace(s, "*");
            }
            foreach (var s in str.Split(','))
            {
                aaa = aaa.Replace(s, "*");
            }
            foreach (var s in str.Split(','))
            {
                aaa = aaa.Replace(s, "*");
            }
            foreach (var s in str.Split(','))
            {
                aaa = aaa.Replace(s, "*");
            }
            foreach (var s in str.Split(','))
            {
                aaa = aaa.Replace(s, "*");
            }
            foreach (var s in str.Split(','))
            {
                aaa = aaa.Replace(s, "*");
            }
            foreach (var s in str.Split(','))
            {
                aaa = aaa.Replace(s, "*");
            }
            watch.Stop();//停止计时
            var ddss2 = watch.ElapsedMilliseconds;
            return View();
        }

        /// <summary>
        /// 获取房间列表
        /// </summary>
        public ActionResult GetChatRoomList()
        {
            string sql = "select * from ChatRoom order by Code ";
            var list = Util.ReaderToList<ChatRoom>(sql);

            return Json(list);
        }

        /// <summary>
        /// 具体聊天页面
        /// </summary>
        /// <param name="id">聊天室Id</param>
        /// <returns></returns>
        [Authentication]
        public ActionResult ChatRoom(int roomId,string roomName)
        {
            try
            {
                int userId = UserHelper.GetByUserId();
                UserInfo user = UserHelper.GetUser(userId);
                var userState = UserHelper.GetUserState(userId);

                ViewBag.RoomId = roomId;
                ViewBag.RoomName = roomName;

                if (user == null)
                {
                    ViewBag.UserId = 9998;
                    ViewBag.UserName = "测试用户";
                    ViewBag.PhotoImg = "/images/default_avater.png";
                    ViewBag.IsAdmin = false;
                }
                else
                {
                    ViewBag.UserId = user.Id;
                    ViewBag.UserName = user.Name;
                    ViewBag.PhotoImg = string.IsNullOrEmpty(user.Headpath) ? "/images/default_avater.png" : user.Headpath;//user.;
                    ViewBag.IsAdmin = (userState.IsChatAD??0)==0?false:true; //
                }

                //查询登录人在本房间是否被禁言
                string sql = "select UserId from TalkBlackList where RoomId = @RoomId and (IsEverlasting =1 or EndTime > GETDATE())";

                SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@RoomId",roomId)
                 };

                var blackListStr = ","+string.Join(",", Util.ReaderToList<TalkBlackList>(sql, regsp).Select(e=>e.UserId))+",";
                
                ViewBag.BlackListStr = blackListStr;
                ViewBag.IsBlack = userState.ChatBlack;//1拉黑  0 正常
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
        [Authentication]
        public ActionResult ManagementList(int roomId, string roomName)
        {
            int userId = UserHelper.GetByUserId();
            UserInfo user = UserHelper.GetUser(userId);
            var userState = UserHelper.GetUserState(userId);

            if (user == null)
            {
                ViewBag.IsAdmin = false;
            }
            else
            {
                ViewBag.IsAdmin = (userState.IsChatAD ?? 0) == 0 ? false : true;
            }

            ViewBag.RoomId = roomId;
            ViewBag.RoomName = roomName;
            return View();
        }

        /// <summary>
        /// 拉黑列表页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authentication]
        public ActionResult BlackList(int roomId, string roomName)
        {
            ViewBag.RoomId = roomId;
            ViewBag.RoomName = roomName;

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
                image.Save(path + fileName + ".jpg");
                image.Dispose();
                ms.Close();

                MemoryStream ms2 = new MemoryStream(ConvertToThumbnail(arr, 70, 100, 100));
                Image image2 = Image.FromStream(ms2);
                image2.Save(path + fileName + "_Min.jpg");
                image2.Dispose();
                ms2.Close();

                return Json(new { Status=1,imgUrl= "http://"+ HttpContext.Request.Url.Host+":"+HttpContext.Request.Url.Port+xPath + datePath + fileName + "_Min.jpg" } );
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0});
            }
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
        [ValidateInput(false)]
        public ActionResult AddMessage(TalkNotes model)
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

                return Json(new { Status = 1 });

            }
            catch (Exception ex)
            {
                return Json(new { Status = 0 });
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

            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@RoomId", roomId), new SqlParameter("@Guid", guid) };

            if (!string.IsNullOrEmpty(guid))
            {
                sql = string.Format(sql, " and id<(select top(1) id from TalkNotes where RoomId = @RoomId and Status = 1 and Guid = @Guid) ");
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
        /// <returns></returns>
        public ActionResult DelMessage(string guid, int userId, string userName,int roomId)
        {
            string sql = @" update TalkNotes set Status = 0 where Guid=@Guid ";

            try
            {
                int i = SqlHelper.ExecuteNonQuery(sql, new SqlParameter[] { new SqlParameter("@Guid", guid) });

                var processingRecords = new ProcessingRecords()
                {
                    ProcessToId = userId,
                    ProcessToName = userName,
                    Type = 1,
                    RoomId = roomId
                };

                AddProcessingRecords(processingRecords);

                if (i > 0)
                {
                    return Json(new { Status = 1 });
                }
                else
                {
                    return Json(new { Status = 0 });
                }
            }
            catch (Exception)
            {
                return Json(new { status = 0 });
            }
        }

        /// <summary>
        /// 管理员删除某人全部消息
        /// </summary>
        /// <returns></returns>
        public ActionResult DelMessageAll(int userId, string userName, int roomId)
        {
            string sql = @" update TalkNotes set Status = 0 where RoomId = @RoomId and UserId = @UserId   ";

            try
            {
                int i = SqlHelper.ExecuteNonQuery(sql, new SqlParameter[] { new SqlParameter("@RoomId", roomId), new SqlParameter("@UserId", userId) });

                var processingRecords = new ProcessingRecords()
                {
                    ProcessToId = userId,
                    ProcessToName = userName,
                    Type = 1,
                    RoomId = roomId
                };

                AddProcessingRecords(processingRecords);

                if (i > 0)
                {
                    return Json(new { Status = 1 });
                }
                else
                {
                    return Json(new { Status = 0 });
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
        public ActionResult AddBlackList(TalkBlackList model, string userName)
        {
            model.BanTime = DateTime.Now;
            model.IsEverlasting = true;

            try
            {
                string regsql = @" delete TalkBlackList where UserId = @UserId and RoomId = @RoomId ;
                                    insert into TalkBlackList (UserId,RoomId,BanTime,IsEverlasting) 
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
                    Type = 2,
                    RoomId = model.RoomId
                };

                AddProcessingRecords(processingRecords);

                return Json(new { Status = 1 });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0 });
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
                string regsql = @"insert into ProcessingRecords (ProcessId,ProcessName,Type,ProcessToId,ProcessToName,ProcessDate,ProcessTime,RoomId)
                                values (@ProcessId,@ProcessName,@Type,@ProcessToId,@ProcessToName,@ProcessDate,@ProcessTime,@RoomId)";
                SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@ProcessId",model.ProcessId),
                    new SqlParameter("@ProcessName",model.ProcessName),
                    new SqlParameter("@Type",model.Type),
                    new SqlParameter("@ProcessToId",model.ProcessToId),
                    new SqlParameter("@ProcessToName",model.ProcessToName),
                    new SqlParameter("@ProcessDate",model.ProcessDate),
                    new SqlParameter("@ProcessTime",model.ProcessTime),
                    new SqlParameter("@RoomId",model.RoomId)
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
        public ActionResult GetProcessingRecords(string roomId,int id = 0)
        {
            try
            {
                string sql = "select top(20) * from ProcessingRecords where RoomId =@RoomId {0} order by ProcessTime desc";

                if (id == 0)
                {
                    sql = string.Format(sql, "");
                }
                else
                {
                    sql = string.Format(sql, " and Id <@Id ");
                }

                var list = Util.ReaderToList<ProcessingRecords>(sql, new SqlParameter[] { new SqlParameter("@Id", id),new SqlParameter("@RoomId",roomId) });

                List<dynamic> dyList = new List<dynamic>();

                list.ForEach(e =>
                {
                    var msg = "";
                    switch (e.Type)
                    {
                        case 1:
                            msg = "删除消息";
                            break;
                        case 2:
                            msg = "拉黑";
                            break;
                        case 3:
                            msg = "解除拉黑";
                            break;
                    }

                    dyList.Add(new
                    {
                        Id = e.Id,
                        Date = e.ProcessDate.ToString("yyyy-MM-dd"),
                        Message = "管理员对用户\"" + e.ProcessToName + "\"进行" + msg + "处理",
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
        /// 获取拉黑列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetBlackList(string roomId, int id = 0)
        {
            try
            {
                string sql = @"select top(20) tbl.Id,tbl.UserId,u.UserName,rm.RPath PhotoImg,tbl.RoomId from TalkBlackList tbl
                                left join UserInfo u on tbl.UserId = u.Id
                                left join ResourceMapping rm on u.Id = rm.FkId and rm.Type =2  
                                where tbl.RoomId =@RoomId {0} and (tbl.IsEverlasting = 1 or EndTime >GETDATE())
                                order by tbl.Id desc";

                if (id == 0)
                {
                    sql = string.Format(sql, "");
                }
                else
                {
                    sql = string.Format(sql, " and tbl.Id <@Id ");
                }

                var list = Util.ReaderToList<BlackListView>(sql, new SqlParameter[] { new SqlParameter("@Id", id), new SqlParameter("@RoomId", roomId) });

                return Json(new { Status = 1, DataList = list });
            }
            catch (Exception)
            {
                return Json(new { Status = 0, DataList = new { } });
            }
        }

        /// <summary>
        /// 解禁
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public ActionResult RemoveBlackList(int userId, int roomId,string userName)
        {
            string sql = "delete TalkBlackList where UserId = @UserId and RoomId = @RoomId";

            try
            {
                SqlHelper.ExecuteScalar(sql, new SqlParameter[] { new SqlParameter("@UserId",userId),new SqlParameter("@RoomId",roomId)});

                var processingRecords = new ProcessingRecords()
                {
                    ProcessToId = userId,
                    ProcessToName = userName,
                    Type = 3,
                    RoomId = roomId
                };

                AddProcessingRecords(processingRecords);

                return Json(new { Status = 1});
            }
            catch (Exception)
            {
                return Json(new { Status = 0 });
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

                CacheHelper.AddCache("GetSensitiveWordsList", str, DateTime.Now.AddHours(2));
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
