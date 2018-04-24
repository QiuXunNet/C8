using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using C8.Lottery.Model;
namespace C8.Lottery.Portal.Models
{
    public class VoucherModel:UserCoupon
    {
        public string Name { get; set; }
    }
}