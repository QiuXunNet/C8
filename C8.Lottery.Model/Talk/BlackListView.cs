using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Model
{
    public class BlackListView
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string PhotoImg { get; set; }

        public string RoomId { get; set; }
    }
}
