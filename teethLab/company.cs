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
    
    public partial class company
    {
        public company()
        {
            this.moneys = new HashSet<money>();
            this.companyProducts = new HashSet<companyProduct>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public int credit { get; set; }
        public Nullable<System.DateTime> enterDate { get; set; }
    
        public virtual ICollection<money> moneys { get; set; }
        public virtual ICollection<companyProduct> companyProducts { get; set; }
    }
}
