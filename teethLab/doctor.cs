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
    
    public partial class doctor
    {
        public doctor()
        {
            this.caseDoctorPrices = new HashSet<caseDoctorPrice>();
            this.CaseTransactions = new HashSet<CaseTransaction>();
            this.moneys = new HashSet<money>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public int Credit { get; set; }
        public string phones { get; set; }
    
        public virtual ICollection<caseDoctorPrice> caseDoctorPrices { get; set; }
        public virtual ICollection<CaseTransaction> CaseTransactions { get; set; }
        public virtual ICollection<money> moneys { get; set; }
    }
}
