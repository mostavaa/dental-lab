﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class teethLabEntities : DbContext
    {
        public teethLabEntities()
            : base("name=teethLabEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<balance> balances { get; set; }
        public DbSet<caseDoctorPrice> caseDoctorPrices { get; set; }
        public DbSet<CaseTransaction> CaseTransactions { get; set; }
        public DbSet<company> companies { get; set; }
        public DbSet<CurrentCas> CurrentCases { get; set; }
        public DbSet<doctor> doctors { get; set; }
        public DbSet<employee> employees { get; set; }
        public DbSet<employeeMoney> employeeMoneys { get; set; }
        public DbSet<money> moneys { get; set; }
        public DbSet<product> products { get; set; }
        public DbSet<ProductUnit> ProductUnits { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<user> users { get; set; }
        public DbSet<companyProduct> companyProducts { get; set; }
    }
}
