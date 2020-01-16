using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BKDAPI.Models
{
    public class FoodCart
    {
        public int User_id { get; set; }
        public int TotalItem { get; set; }
        public decimal TotalAmount{ get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalTax { get; set; }        
        public List<FoodItem> ProductList { get; set; }
    }

    public class FoodItem {
        public decimal ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> TaxPer { get; set; }
        public Nullable<decimal> TaxAmt { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> TotalTax { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public int Quantity { get; set; }
    }

    public class FoodItemDetail
    {
        public int ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
    }
}