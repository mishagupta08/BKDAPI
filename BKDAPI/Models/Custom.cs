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
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
        public int ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> TaxPer { get; set; }
        public Nullable<decimal> TaxAmt { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> TotalTax { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public List<User> Cook { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string OrderStatus { get; set; }
    }
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
        public int TotalOrdQty { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTaxAmt { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
    }
    public class User
    {
        public decimal UId { get; set; }
        public decimal UserId { get; set; }
        public string UserName { get; set; }
        public string Passw { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public decimal GroupId { get; set; }
        public string ActiveStatus { get; set; }
    }
    public class Assign
    {
        public int Id { get; set; }
        public int AssignUserId { get; set; }
        public string AssignType { get; set; }
    }

}