using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QL_BanDoAn.Models
{
    public class Cart
    {
        public string email { get; set; }
        public int id { get; set; }
        public bool status { get; set; }
        public string time { get; set; }
        public double totalPrice { get; set; }
        public List<Foods> listFood { get; set; }
    }
}