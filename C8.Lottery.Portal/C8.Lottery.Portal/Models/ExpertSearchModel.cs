using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.Lottery.Portal.Models
{
    [Serializable]
    public class ExpertSearchModel
    {
        public int UserId { get; set; }
        public int lType { get; set; }
        public int isFollow { get; set; }
        public string Name { get; set; }
        public string Avater { get; set; }
    }
}