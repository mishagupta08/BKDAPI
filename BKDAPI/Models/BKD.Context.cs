﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BKDAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BKDHEntities : DbContext
    {
        public BKDHEntities()
            : base("name=BKDHEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Inv_M_UserMaster> Inv_M_UserMaster { get; set; }
        public virtual DbSet<M_ProductMaster> M_ProductMaster { get; set; }
        public virtual DbSet<trnFoodOrderDetail> trnFoodOrderDetails { get; set; }
        public virtual DbSet<trnFoodOrderMain> trnFoodOrderMains { get; set; }
        public virtual DbSet<trnFoodCart> trnFoodCarts { get; set; }
    }
}
