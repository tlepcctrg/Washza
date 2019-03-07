using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WashZa.Models
{
    public class WashLaundry
    {
        public string Id { get; set; }
        public Nullable<int> Active { get; set; }
        public string userid { get; set; }
        public string payid { get; set; }
        public Nullable<decimal> amount { get; set; }
        public bool flag_payment { get; set; }
        public bool flag_send { get; set; }
        public string First_Name { get; set; }
        public  string Last_Name { get; set; }
        public string Address { get; set; }
    }
}