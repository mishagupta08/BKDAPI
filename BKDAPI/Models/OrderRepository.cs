using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BKDAPI.Models
{
    public class OrderRepository
    {
        string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        public async Task<Response> PostOrder(Inv_M_UserMaster user)
        {
            Response objResponse = new Response();
            Order order = new Order();
            OrderDetail orderDetail = new OrderDetail();
            List<OrderDetail> lstOrderDetail = new List<OrderDetail>();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@UserID", user.UserId)
                };
                using (DataSet ds = SqlHelper.ExecuteDataset(CONNECTION_STRING, "sp_PostOrder", parameters))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //Lets go ahead and create the list of employees
                        order.OrderId = Convert.ToInt32(ds.Tables[0].Rows[0]["OrderId"]);
                        order.OrderDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["OrderDate"]).ToString("dd-MMM-yyyy");
                        order.TotalOrdQty = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalOrdQty"]);
                        order.TotalAmount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalAmount"]);
                        order.TotalTaxAmt = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalTaxAmt"]);
                        lstOrderDetail = new List<OrderDetail>();
                        //Now lets populate the employee details into the list of employees
                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            orderDetail = new OrderDetail();
                            orderDetail.OrderId = Convert.ToInt32(row["OrderId"]);
                            orderDetail.ProductCode = Convert.ToInt32(row["ProductCode"]);
                            orderDetail.Quantity = Convert.ToInt32(row["Quantity"]);
                            orderDetail.ProductName = Convert.ToString(row["ProductName"]);
                            orderDetail.ProductImage = Convert.ToString(row["ProductImage"]);
                            orderDetail.Price = Convert.ToInt32(row["Price"]);
                            orderDetail.TaxPer = Convert.ToInt32(row["TaxPer"]);
                            orderDetail.TotalPrice = Convert.ToInt32(row["TotalPrice"]);
                            orderDetail.TotalTax = Convert.ToInt32(row["TotalTax"]);
                            orderDetail.TotalAmount = Convert.ToInt32(row["TotalAmount"]);
                            lstOrderDetail.Add(orderDetail);
                        }
                    }
                    else
                    {
                        objResponse.Status = false;
                    }
                    order.OrderDetail = lstOrderDetail;
                }

                objResponse.Status = true;
                objResponse.ResponseValue = await Task.Run(() => new JavaScriptSerializer().Serialize(order));
            }
            catch (Exception ex)
            {
                objResponse.Status = false;
                objResponse.ResponseMessage = ex.Message;
            }
            return objResponse;
        }

        public async Task<Response> GetOrderList(int userId)
        {
            Response objResponse = new Response();           
            List<Orders> lstOrderDetail = new List<Orders>();           
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@UserID", userId)
                };
                using (DataSet ds = SqlHelper.ExecuteDataset(CONNECTION_STRING, "sp_GetOrderSummaryList", parameters))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstOrderDetail = new List<Orders>();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            var orderDetail = new Orders();                            
                            orderDetail.OrderNo = Convert.ToInt32(row["OrderId"]);
                            orderDetail.OrderDate = Convert.ToDateTime(row["OrderDate"]).ToString("dd-MMM-yyyy");                            
                            orderDetail.TotalQuantity = Convert.ToInt32(row["TotalOrdQty"]);
                            orderDetail.OrderAmount = Convert.ToDecimal(row["NetPayable"]);
                            orderDetail.StallName = Convert.ToString(row["StallName"]);
                            orderDetail.OrderStatus = Convert.ToString(row["Remarks"]);
                            lstOrderDetail.Add(orderDetail);                            
                        }
                        objResponse.Status = true;
                    }
                    else
                    {
                        objResponse.Status = false;
                    }                    
                }
                objResponse.ResponseValue = await Task.Run(() => new JavaScriptSerializer().Serialize(lstOrderDetail));
            }
            catch (Exception ex)
            {
                objResponse.Status = false;
                objResponse.ResponseMessage = ex.Message;
            }
            return objResponse;
        }

        public async Task<Response> GetOrderProducts(int OrderId,int UserId)
        {
            Response objResponse = new Response();
            Order order = new Order();
            OrderDetail orderDetail = new OrderDetail();
            User user = new User();
            List<OrderDetail> lstOrderDetail = new List<OrderDetail>();
            List<User> lstUser = new List<User>();
            try
            {
              

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                      new SqlParameter("@OrderId", OrderId),
                      new SqlParameter("@UserId", UserId)
                    };
                
                using (DataSet ds = SqlHelper.ExecuteDataset(CONNECTION_STRING, "sp_GetOrderProductList", parameters))
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        lstUser = new List<User>();
                        foreach (DataRow rowCook in ds.Tables[1].Rows)
                        {
                            user = new User();
                            user.UserId = Convert.ToInt32(rowCook["UserId"]);
                            user.UserName = Convert.ToString(rowCook["UserName"]);
                            user.Name = Convert.ToString(rowCook["Name"]);
                            user.GroupId = Convert.ToDecimal(rowCook["GroupId"]);
                            lstUser.Add(user);
                        }
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstOrderDetail = new List<OrderDetail>();
                        //Now lets populate the employee details into the list of employees
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            orderDetail = new OrderDetail();
                            orderDetail.Id = Convert.ToInt32(row["Id"]);
                            orderDetail.OrderId = Convert.ToInt32(row["OrderId"]);
                            orderDetail.OrderDate = Convert.ToDateTime(row["OrderDate"]).ToString("dd-MMM-yyyy");
                            orderDetail.ProductCode = Convert.ToInt32(row["ProductCode"]);
                            orderDetail.Quantity = Convert.ToInt32(row["Quantity"]);
                            orderDetail.ProductName = Convert.ToString(row["ProductName"]);
                            orderDetail.ProductImage = Convert.ToString(row["ProductImage"]);
                            orderDetail.Price = Convert.ToInt32(row["Price"]);
                            orderDetail.TaxPer = Convert.ToInt32(row["TaxPer"]);
                            orderDetail.TotalPrice = Convert.ToInt32(row["TotalPrice"]);
                            orderDetail.TotalTax = Convert.ToInt32(row["TotalTax"]);
                            orderDetail.TotalAmount = Convert.ToInt32(row["TotalAmount"]);
                            orderDetail.CookName = Convert.ToString(row["Cook"]);
                            orderDetail.SupervisorName = Convert.ToString(row["Supervisor"]);
                            orderDetail.DeliveryBy = Convert.ToString(row["DeliveryBy"]);
                            orderDetail.OrderStatus = Convert.ToString(row["Status"]);
                            
                            orderDetail.Cook = lstUser;
                            lstOrderDetail.Add(orderDetail);                            
                        }
                        objResponse.Status = true;
                    }
                    else
                    {
                        objResponse.Status = false;
                    }

                    //order.OrderDetail = lstOrderDetail;
                }
                objResponse.ResponseValue = await Task.Run(() => new JavaScriptSerializer().Serialize(lstOrderDetail));
            }
            catch (Exception ex)
            {
                objResponse.Status = false;
                objResponse.ResponseMessage = ex.Message;
            }
            return objResponse;
        }

        public async Task<Response> GetOrderStatus(int userId)
        {
            Response objResponse = new Response();
            Order order = new Order();
            OrderDetail orderDetail = new OrderDetail();
            User user = new User();
            List<OrderDetail> lstOrderDetail = new List<OrderDetail>();
            List<User> lstUser = new List<User>();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@UserID", userId)
                };
                using (DataSet ds = SqlHelper.ExecuteDataset(CONNECTION_STRING, "sp_GetOrderStatus", parameters))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //Lets go ahead and create the list of employees
                        //order.OrderId = Convert.ToInt32(ds.Tables[0].Rows[0]["OrderId"]);
                        //order.OrderDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["OrderDate"]).ToString("dd-MMM-yyyy");
                        //order.TotalOrdQty = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalOrdQty"]);
                        //order.TotalAmount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalAmount"]);
                        //order.TotalTaxAmt = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalTaxAmt"]);
                        lstOrderDetail = new List<OrderDetail>();
                        //Now lets populate the employee details into the list of employees
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            orderDetail = new OrderDetail();
                            orderDetail.Id = Convert.ToInt32(row["Id"]);
                            orderDetail.OrderId = Convert.ToInt32(row["OrderId"]);
                            orderDetail.OrderDate = Convert.ToDateTime(row["OrderDate"]).ToString("dd-MMM-yyyy");
                            orderDetail.ProductCode = Convert.ToInt32(row["ProductCode"]);
                            orderDetail.Quantity = Convert.ToInt32(row["Quantity"]);
                            orderDetail.ProductName = Convert.ToString(row["ProductName"]);
                            orderDetail.ProductImage = Convert.ToString(row["ProductImage"]);
                            orderDetail.Price = Convert.ToInt32(row["Price"]);
                            orderDetail.TaxPer = Convert.ToInt32(row["TaxPer"]);
                            orderDetail.TotalPrice = Convert.ToInt32(row["TotalPrice"]);
                            orderDetail.TotalTax = Convert.ToInt32(row["TotalTax"]);
                            orderDetail.TotalAmount = Convert.ToInt32(row["TotalAmount"]);
                            orderDetail.UserName = Convert.ToString(row["UserName"]);
                            orderDetail.Name = Convert.ToString(row["Name"]);
                            orderDetail.OrderStatus = Convert.ToString(row["Status"]);
                            lstOrderDetail.Add(orderDetail);
                        }
                        objResponse.Status = true;
                    }
                    else
                    {
                        objResponse.Status = false;
                    }

                    //order.OrderDetail = lstOrderDetail;
                }
                objResponse.ResponseValue = await Task.Run(() => new JavaScriptSerializer().Serialize(lstOrderDetail));
            }
            catch (Exception ex)
            {
                objResponse.Status = false;
                objResponse.ResponseMessage = ex.Message;
            }
            return objResponse;
        }

        public async Task<Response> AssignOrder(OrderAssignment assign)
        {
            Response objResponse = new Response();
            OrderAssignment assignList = new OrderAssignment();
            OrderDetail orderDetail = new OrderDetail();
            List<OrderDetail> lstOrderDetail = new List<OrderDetail>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<AssignData>");
                sb.AppendLine("<OrderID>"+ assign.OrderId +"</OrderID>");
                sb.AppendLine("<Supervisor>" + assign.SupervisorId + "</Supervisor>");
                sb.AppendLine("<DeliveryBoy>" + assign.DeliveryBoyId + "</DeliveryBoy>");
                sb.AppendLine("<CookData>");
                assign.ProductCook.ToList().ForEach(
                emp =>
                {
                    sb.AppendLine("<AssignCook>");                   
                    sb.AppendLine(("<ProductId>" + emp.ProductId + "</ProductId>"));
                    sb.AppendLine(("<CookID>" + emp.CookId + "</CookID>"));
                    sb.AppendLine("</AssignCook>");
                });
                sb.AppendLine("</CookData>");
                sb.AppendLine("</AssignData>");
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@regXML", sb.ToString())
                };
                using (DataSet ds = SqlHelper.ExecuteDataset(CONNECTION_STRING, "sp_AssignUser", parameters))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //Lets go ahead and create the list of employees
                        //assignList.Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]);
                        objResponse.Status = true;
                        objResponse.ResponseMessage = "Assigned successfuly";
                    }
                    else
                    {
                        objResponse.Status = false;
                        objResponse.ResponseMessage = "Some error occured";
                    }
                    //objResponse.ResponseValue = await Task.Run(() => new JavaScriptSerializer().Serialize(order));
                }
            }
            catch (Exception ex)
            {
                objResponse.Status = false;
                objResponse.ResponseMessage = ex.Message;
            }
            return objResponse;
        }

        public async Task<Response> GetAssignedOrderList(int userId)
        {
            Response objResponse = new Response();
            List<Orders> lstOrderDetail = new List<Orders>();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@UserID", userId)
                };
                using (DataSet ds = SqlHelper.ExecuteDataset(CONNECTION_STRING, "sp_GetAssgnedOrderSummaryList", parameters))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstOrderDetail = new List<Orders>();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            var orderDetail = new Orders();
                            orderDetail.OrderNo = Convert.ToInt32(row["OrderId"]);
                            orderDetail.OrderDate = Convert.ToDateTime(row["OrderDate"]).ToString("dd-MMM-yyyy");
                            orderDetail.TotalQuantity = Convert.ToInt32(row["TotalOrdQty"]);
                            orderDetail.OrderAmount = Convert.ToDecimal(row["NetPayable"]);
                            orderDetail.StallName = Convert.ToString(row["StallName"]);
                            orderDetail.OrderStatus = Convert.ToString(row["Remarks"]);
                            lstOrderDetail.Add(orderDetail);
                        }
                        objResponse.Status = true;
                    }
                    else
                    {
                        objResponse.Status = false;
                    }
                }
                objResponse.ResponseValue = await Task.Run(() => new JavaScriptSerializer().Serialize(lstOrderDetail));
            }
            catch (Exception ex)
            {
                objResponse.Status = false;
                objResponse.ResponseMessage = ex.Message;
            }
            return objResponse;
        }


        public async Task<Response> UpdateStatus(WorkStatus objworkstatus)
        {
            Response objResponse = new Response();
            objResponse.Status = true;
            objResponse.ResponseMessage = "Status Updated Successfully";
            try
            {
                using (var entity = new BKDHEntities())
                {
                    var OrderProducts = await Task.Run(() => entity.trnFoodOrderDetails.Where(r => r.OrderId == objworkstatus.OrderId).ToList());
                    var OrderMain = await Task.Run(() => entity.trnFoodOrderMains.Where(r => r.OrderId == objworkstatus.OrderId).FirstOrDefault());

                    if (objworkstatus.UserType.ToLower() == "cook")
                    {
                        foreach (var id in objworkstatus.ProductId)
                        {
                            var Product = OrderProducts.FirstOrDefault(r => r.ProductCode == id && r.CookID == objworkstatus.UserId);
                            Product.CookStatus = objworkstatus.Status;
                        }                        
                        entity.SaveChanges();
                    }
                    else if (objworkstatus.UserType.ToLower() == "supervisor")
                    {
                        var flag = true;
                        foreach (var id in objworkstatus.ProductId)
                        {
                            var Product = OrderProducts.FirstOrDefault(r => r.ProductCode == id && r.PckID == objworkstatus.UserId);
                            if (Product.CookStatus.ToLower() == "cooked")
                            {
                                Product.SuperVisiorStatus = objworkstatus.Status;
                            }
                            else
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            OrderMain.Remarks = objworkstatus.Status;
                            entity.SaveChanges();
                        }
                        else
                        {
                            objResponse.Status = false;
                            objResponse.ResponseMessage = "All products in order are not cooked yet.";
                        }
                    }
                    else if (objworkstatus.UserType.ToLower() == "deliveryboy")
                    {
                        var flag = true;
                        foreach (var id in objworkstatus.ProductId)
                        {
                            var Product = OrderProducts.FirstOrDefault(r => r.ProductCode == id && r.DelvID == objworkstatus.UserId);                           
                            if (Product.SuperVisiorStatus.ToLower() == "packed")
                            {
                                Product.DeliveryStatus = objworkstatus.Status;
                            }
                            else
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            OrderMain.Remarks = objworkstatus.Status;
                            int i =entity.SaveChanges();
                            if (i > 0)
                            {
                                AddStock(objworkstatus.OrderId);
                            }
                        }
                        else
                        {
                            objResponse.Status = false;
                            objResponse.ResponseMessage = "All products in order are not packed yet.";
                        }
                    }

                    if (objworkstatus.UserType.ToLower() == "cook")
                    {
                        var CookedProducts = await Task.Run(() => entity.trnFoodOrderDetails.Where(r => r.OrderId == objworkstatus.OrderId && r.CookStatus.ToLower() == "cooked").ToList());
                        if (OrderProducts.Count == CookedProducts.Count)
                        {
                            OrderMain.Remarks = "Cooked";                           
                        }
                        else
                        {
                            OrderMain.Remarks = "Pending";
                        }
                        entity.SaveChanges();
                    }
                 }                   
            }
            catch (Exception ex)
            {
                objResponse.Status = false;
                objResponse.ResponseMessage = ex.Message;
            }
            return objResponse;
        }

        public string AddStock(int Order)
        {
            string response = string.Empty;
            response = "Failed";
            try
            {
                using (var entitites = new BKDHEntities())
                {

                    var OrderSummary = (from r in entitites.trnFoodOrderMains where r.OrderId == Order && r.Remarks == "Delivered" select r).FirstOrDefault();
                    var orderProducts = (from r in entitites.trnFoodOrderDetails where r.OrderId == Order select r).ToList();

                    foreach (var product in orderProducts)
                    {
                        Im_CurrentStock objCurrentStock = new Im_CurrentStock();

                        objCurrentStock.FSessId = 0;
                        objCurrentStock.SupplierCode = OrderSummary.OrderToKitchen;
                        objCurrentStock.StockDate = DateTime.Now.Date;
                        objCurrentStock.RefNo = "Order:" + Order;
                        objCurrentStock.FCode = OrderSummary.OrderByStall;
                        objCurrentStock.GroupId = 0;
                        objCurrentStock.ProdId = Convert.ToString(product.ProductCode);
                        objCurrentStock.BatchCode = "";
                        objCurrentStock.Barcode = "";
                        objCurrentStock.SType = "I";
                        objCurrentStock.Qty = product.Quantity;
                        objCurrentStock.BType = "";
                        objCurrentStock.Remarks = "Stock Added";
                        objCurrentStock.BillType = "";

                        objCurrentStock.ActiveStatus = "Y";
                        objCurrentStock.EntryBy = "";
                        objCurrentStock.StockFor = "";
                        objCurrentStock.RecTimeStamp = DateTime.Now;
                        objCurrentStock.UserId = 0;
                        objCurrentStock.Version = "";
                        objCurrentStock.IsDisp = "N";
                        objCurrentStock.InvoiceNo = "";
                        objCurrentStock.ProdType = "P";
                        objCurrentStock.DispQty = 0;

                        entitites.Im_CurrentStock.Add(objCurrentStock);
                    }
                    int i = entitites.SaveChanges();
                    if (i > 0)
                    {
                        response = "Stock Added"; 
                    }
                }
            }
            catch (Exception ex)
            {
                response = "Failed";
            }
            return response;
        }

        public async Task<Response> LessStock(UsedStallProducts ConsumedProducts)
        {
            Response objResponse = new Response();
            objResponse.Status = true;
            objResponse.ResponseMessage = "Stock Updated Successfully";
            try
            {
                using (var entitites = new BKDHEntities())
                {
                    var currentStock = getCurrentStock(ConsumedProducts.Stall);
                    var isStockAvailable = true;
                    foreach (var product in ConsumedProducts.ProductList)
                    {
                        if (currentStock.Where(r => r.ProductCode == Convert.ToString(product.ProductCode)).FirstOrDefault().Quantity >= product.Quantity)
                        {
                            Im_CurrentStock objCurrentStock = new Im_CurrentStock();
                            objCurrentStock.FSessId = 0;
                            objCurrentStock.SupplierCode = "";
                            objCurrentStock.StockDate = DateTime.Now.Date;
                            objCurrentStock.RefNo = "";
                            objCurrentStock.FCode = ConsumedProducts.Stall;
                            objCurrentStock.GroupId = 0;
                            objCurrentStock.ProdId = Convert.ToString(product.ProductCode);
                            objCurrentStock.BatchCode = "";
                            objCurrentStock.Barcode = "";
                            objCurrentStock.SType = product.type;
                            objCurrentStock.Qty = -(product.Quantity);
                            objCurrentStock.BType = "";
                            objCurrentStock.Remarks = product.type == "W" ? "Stock Wasted" : "Stock Less";
                            objCurrentStock.BillType = "";

                            objCurrentStock.ActiveStatus = "Y";
                            objCurrentStock.EntryBy = "";
                            objCurrentStock.StockFor = "";
                            objCurrentStock.RecTimeStamp = DateTime.Now;
                            objCurrentStock.UserId = 0;
                            objCurrentStock.Version = "";
                            objCurrentStock.IsDisp = "N";
                            objCurrentStock.InvoiceNo = "";
                            objCurrentStock.ProdType = "P";
                            objCurrentStock.DispQty = 0;

                            entitites.Im_CurrentStock.Add(objCurrentStock);
                        }
                        else
                        {
                            isStockAvailable = false;
                            objResponse.Status = false;
                            objResponse.ResponseMessage = "Stock Not Available.";
                            break; 
                        }
                    }
                    int i = 0;
                    if (isStockAvailable)
                    {
                        i = await Task.Run(() => entitites.SaveChanges());

                        if (i <= 0)
                        {
                            objResponse.Status = false;
                            objResponse.ResponseMessage = "Something went wrong.";
                        }
                        else
                        {
                            objResponse.Status = true;
                            objResponse.ResponseMessage = "Stock Updated Successfully.";
                        }
                    }               
                }
            }
            catch (Exception ex)
            {
                objResponse.Status = false;
                objResponse.ResponseMessage = ex.Message;
            }
            return objResponse;
        }

        public List<StockReport> getCurrentStock(string Stall)
        {
            List<StockReport> CurrentStock = new List<StockReport>();
            try
            {
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now;

                using (var entity = new BKDHEntities())
                {
                    CurrentStock = (from r in entity.V_CurrentStockDetailNotForStockist
                                    where (Stall != "0" && Stall != "All" ? r.PartyCode == Stall : 1 == 1)
                                    orderby r.ProductName
                                    select new StockReport
                                    {
                                        PartyCode = r.PartyCode,
                                        PartyName = r.PartyName,
                                        ProductCode = r.ProdId,
                                        ProductName = r.ProductName,
                                        Quantity = r.Qty ?? 0
                                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return CurrentStock;
        }

        public async Task<Response> GetDateWiseStockReport(string PartyCode,string From,string To)
        {
            Response objResponse = new Response();            
            objResponse.Status = false;

            try
            {

                DateTime startDate = DateTime.Now.Date;
                DateTime endDate = DateTime.Now.Date;

                if (!string.IsNullOrEmpty(From))
                {
                    startDate = Convert.ToDateTime(DateTime.ParseExact(From, "dd-MM-yyyy", CultureInfo.InvariantCulture)).Date;                   
                }

                if (!string.IsNullOrEmpty(To))
                {
                    endDate = Convert.ToDateTime(DateTime.ParseExact(To, "dd-MM-yyyy", CultureInfo.InvariantCulture)).Date;
                }

                using (var entity = new BKDHEntities())
                {
                    var objStockModel = await Task.Run(() => (from r in entity.StockDetail(PartyCode,startDate, endDate)                                     
                                     select r).ToList());
                    objResponse.Status = true;
                    objResponse.ResponseValue = await Task.Run(() => new JavaScriptSerializer().Serialize(objStockModel));
                }
            }
            catch (Exception ex)
            {
                objResponse.Status = false;
                objResponse.ResponseMessage = ex.Message;
            }
            return objResponse;
        }

        public async Task<Response> GetStockReport(string PartyCode)
        {
            Response objResponse = new Response();
            StockReportModel objStockDetail = new StockReportModel();
            objStockDetail.StockDetail = new List<StockReport>();
            try
            {                
                using (var entity = new BKDHEntities())
                {
                    objStockDetail.StockDetail = await Task.Run(() => (from r in entity.V_CurrentStockDetailNotForStockist
                                         where (PartyCode != "0" && PartyCode != "All" ? r.PartyCode == PartyCode : 1 == 1)
                                         orderby r.ProductName
                                         select new StockReport
                                         {
                                             PartyCode = r.PartyCode,
                                             PartyName = r.PartyName,                                                                                                                                
                                             ProductCode = r.ProdId,
                                             ProductName = r.ProductName,                                            
                                             Quantity = r.Qty??0                                           
                                         }).ToList());

                    var CurrentDate = DateTime.Now.Date;
                    objStockDetail.IsStockUpdated = entity.Im_CurrentStock.Where(r => r.StockDate == CurrentDate && (r.SType == "O" || r.SType == "W") && r.FCode == PartyCode).Count() >0 ? "Y" : "N";

                    objResponse.Status = true;
                    objResponse.ResponseValue = await Task.Run(() => new JavaScriptSerializer().Serialize(objStockDetail));
                }
            }
            catch (Exception ex)
            {
                objResponse.Status = false;
                objResponse.ResponseMessage = ex.Message;
            }
            return objResponse;
        }




    }
}