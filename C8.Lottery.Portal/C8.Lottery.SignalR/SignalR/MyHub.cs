using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace C8.Lottery.SignalR
{
    public class MyHub : Hub
    {
        private static UserContext _db = new UserContext();

        public void Hello()
        {
            Clients.All.hello();
        }

        /// <summary>
        /// 重写Hub连接事件
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            var roomName = Context.QueryString["roomName"];
            var userName = Context.QueryString["userName"];
            var userId = Context.QueryString["userId"];
            var photoImg = Context.QueryString["photoImg"];

            var room = _db.Rooms.FirstOrDefault(a => a.RoomName == roomName);
            if (room == null)
            {
                room = new ConversationRoom { RoomName = roomName };
                _db.Rooms.Add(room);
            }
            var user = room.Users.FirstOrDefault(a => a.UserId == userId);
            if (user == null)
            {
                user = new User
                {
                    UserName = userName,
                    UserId = userId,
                    PhotoImg = photoImg
                };
                _db.Users.Add(user);
                user.Rooms.Add(room);
                room.Users.Add(user);
                Groups.Add(Context.ConnectionId, room.RoomName);
            }
          //  Clients.Group(roomName).LoadMemberList(room.Users.Count());
          //  Clients.Group(roomName).ShowMessage(userName+"  初次链接");

            return base.OnConnected();
        }

        /// <summary>
        /// 重写Hub连接断开的事件
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            OffLine();
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// 断开重连
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()    
        {
            var roomName = Context.QueryString["roomName"];
            var userName = Context.QueryString["userName"];
            var userId = Context.QueryString["userId"];
            var photoImg = Context.QueryString["photoImg"];

            var room = _db.Rooms.FirstOrDefault(a => a.RoomName == roomName);
            if (room == null)
            {
                room = new ConversationRoom { RoomName = roomName };
                _db.Rooms.Add(room);
            }
            var user = room.Users.FirstOrDefault(a => a.UserId == userId);
            if (user == null)
            {
                user = new User
                {
                    UserName = userName,
                    UserId = userId,
                    PhotoImg = photoImg
                };
                _db.Users.Add(user);
                user.Rooms.Add(room);
                room.Users.Add(user);
                Groups.Add(Context.ConnectionId, room.RoomName);
            }

            Clients.Group(roomName).LoadMemberList(room.Users.Count());
          //  Clients.Group(roomName).ShowMessage(userName + "  断线重连");

            return base.OnReconnected();
        }

        /// <summary>
        /// 返回聊天室人数
        /// </summary>
        public void LoadMemberList()
        {
            var roomName = Context.QueryString["roomName"];
            var room = _db.Rooms.FirstOrDefault(a => a.RoomName == roomName);
            Clients.Group(roomName).LoadMemberList(room.Users.Count());
        }

        /// <summary>
        /// 断开方法
        /// </summary>
        public void OffLine()
        {
            var userId = Context.QueryString["userId"];
            var user = _db.Users.FirstOrDefault(u => u.UserId == userId);

            //判断用户是否存在,存在则删除
            if (user != null)
            {
                _db.Users.Remove(user);
                // 循环用户的房间,删除用户
                foreach (var item in user.Rooms)
                {
                    var room = _db.Rooms.Find(a => a.RoomName == item.RoomName);
                    if (room != null)
                    {
                        room.Users.Remove(user);
                        //如果房间人数为0,则删除房间
                        if (room.Users.Count <= 0)
                        {
                            _db.Rooms.Remove(room);
                        }
                        Groups.Remove(Context.ConnectionId, room.RoomName);
                       // Clients.Group(room.RoomName).ShowMessage(user.UserName + "  退出链接");
                        Clients.Group(room.RoomName, new string[0]).LoadMemberList(room.Users.Count());
                    }
                }
            }
        }

        /// <summary>
        /// 给分组内所有的用户发送消息
        /// </summary>
        /// <param name="message">信息</param>
        public void SendMessage(string message)
        {
            var room = Context.QueryString["roomName"];
            var userName = Context.QueryString["userName"];
            var userId = Context.QueryString["userId"];
            var photoImg = Context.QueryString["photoImg"];
            if (!_list.Any(a => a.UserId == userId && a.RoomName == room))
            {
                var data = new
                {
                    UserName = userName,
                    SendPeople = userId,
                    Content = message,
                    PhotoImg = photoImg,
                    SendTimeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                Clients.Group(room).sendMessage(data);
            }
        }

        /// <summary>
        /// 获取消息记录的Guid并推送删除
        /// </summary>
        /// <param name="guid"></param>
        public void DelMessage(string guid)
        {
            var room = Context.QueryString["roomName"];            
            var userId = Context.QueryString["userId"];           
            if (!_list.Any(a => a.UserId == userId && a.RoomName == room))
            {                
                Clients.Group(room).delMessage(guid);
               // Clients.User()
            }
        }

        /// <summary>
        /// 用于保存已被禁言的用户
        /// </summary>
        private static List<MyClass> _list = new List<MyClass>();

       /// <summary>
       /// 拉黑禁言
       /// </summary>
        public void Blacklisted(string userId)
        {
            var roomName = Context.QueryString["roomName"];
            _list.Add(new MyClass { RoomName = roomName, UserId = userId });              
            
           Clients.Group(roomName).blacklisted(userId);
        }

        /// <summary>
        /// 禁言用户类
        /// </summary>
        public class MyClass
        {
            public string RoomName { get; set; }
            public string UserId { get; set; }
        }
    }
}