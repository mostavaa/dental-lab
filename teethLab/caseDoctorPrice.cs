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
    
    public partial class caseDoctorPrice
    {
        public int id { get; set; }
        public Nullable<int> doctorId { get; set; }
        public Nullable<int> caseId { get; set; }
        public int price { get; set; }
    
        public virtual CurrentCas CurrentCas { get; set; }
        public virtual doctor doctor { get; set; }
    }
}
