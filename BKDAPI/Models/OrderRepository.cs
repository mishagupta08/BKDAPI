using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
                        OrderMain.Remarks = objworkstatus.Status;
                        entity.SaveChanges();
                    }
                    else if (objworkstatus.UserType.ToLower() == "supervisor")
                    {
                        foreach (var id in objworkstatus.ProductId)
                        {
                            var Product = OrderProducts.FirstOrDefault(r => r.ProductCode == id && r.PckID == objworkstatus.UserId);
                            Product.SuperVisiorStatus = objworkstatus.Status;
                        }
                        OrderMain.Remarks = objworkstatus.Status;
                        entity.SaveChanges();
                    }
                    else if (objworkstatus.UserType.ToLower() == "deliveryboy")
                    {
                        foreach (var id in objworkstatus.ProductId)
                        {
                            var Product = OrderProducts.FirstOrDefault(r => r.ProductCode == id && r.DelvID == objworkstatus.UserId);
                            Product.DeliveryStatus = objworkstatus.Status;
                        }
                        OrderMain.Remarks = objworkstatus.Status;
                        entity.SaveChanges();
                    }

                    objResponse.Status = true;
                    objResponse.ResponseMessage = "Status Updated Successfully";
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