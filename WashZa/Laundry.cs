//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WashZa
{
    using System;
    using System.Collections.Generic;
    
    public partial class Laundry
    {
        public string Id { get; set; }
        public Nullable<int> Active { get; set; }
        public string userid { get; set; }
        public string payid { get; set; }
        public Nullable<decimal> amount { get; set; }
        public string flag_payment { get; set; }
        public string flag_send { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
