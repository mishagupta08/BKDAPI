//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class trnFoodOrderDetail
    {
        public decimal Id { get; set; }
        public decimal OrderId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public int ProductCode { get; set; }
        public int Quantity { get; set; }
        public string KitchenID { get; set; }
        public string CookID { get; set; }
        public string PckID { get; set; }
        public string DelvID { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> TaxPer { get; set; }
        public Nullable<decimal> TaxAmt { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> TotalTax { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
    }
}
