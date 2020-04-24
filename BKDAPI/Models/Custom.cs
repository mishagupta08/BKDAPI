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
        public string CookName { get; set; }
        public string SupervisorName { get; set; }
        public string DeliveryBy { get; set; }
        public string OrderStatus { get; set; }
    }

    public class Orders
    {
        public int OrderNo { get; set; }
        public string StallName { get; set; }
        public int TotalQuantity { get; set; }
        public decimal OrderAmount { get; set; }
        public string OrderDate { get; set; }
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
        public string FCode { get; set; }
        public string Name { get; set; }
        public string BranchCode { get; set; }
        public string GroupName { get; set; }
        public decimal GroupId { get; set; }
        public string ActiveStatus { get; set; }
    }
    public class CookAssign
    {
        public int ProductId { get; set; }
        public string CookId { get; set; }
    }

    public class OrderAssignment
    {
        public int OrderId { get; set; }
        public List<CookAssign> ProductCook { get; set; }
        public int SupervisorId { get; set; }
        public int DeliveryBoyId { get; set; }
    }

    public class KitchenUser
    {
        public decimal UId { get; set; }
        public decimal UserId { get; set; }
        public string UserName { get; set; }
        public string Passw { get; set; }
        public string Name { get; set; }
        public string BranchCode { get; set; }
        public string FCode { get; set; }
        public string ActiveStatus { get; set; }
    }

    public class WorkStatus
    {
        public string Status { get; set; }
        public int UserId { get; set; }
        public string UserType { get; set; }
        public int OrderId { get; set; }
        public List<int> ProductId { get; set; }
    }

    public class StockFilter
    {
        public string StallCounter { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class UsedStallProducts
    {
        public string Stall { get; set; }       
        public List<ConsumedProducts> ProductList { get; set; }
    }

    public class ConsumedProducts
    {
        public int ProductCode { get; set; }
        public string type { get; set; }
        public int Quantity { get; set; }
    }

    public class StockReportModel
    {
        public string IsStockUpdated { get; set; }
        public List<StockReport> StockDetail { get; set; }
}

    public class StockReport
    {
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }        
    }

    public class StockSummaryModel
    {
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal WasteStock { get; set; }
        public decimal OpStock { get; set; }
        public decimal InStock { get; set; }
        public decimal StockOut { get; set; }
        public decimal ClsStock { get; set; }        
    }
}