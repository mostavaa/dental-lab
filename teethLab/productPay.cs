//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace teethLab
{
    using System;
    using System.Collections.Generic;
    
    public partial class productPay
    {
        public int id { get; set; }
        public int cost { get; set; }
        public System.DateTime payDate { get; set; }
        public int productId { get; set; }
    
        public virtual product product { get; set; }
    }
}
