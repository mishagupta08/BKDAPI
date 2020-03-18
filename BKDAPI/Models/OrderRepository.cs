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

        public async Task<Response> GetOrderProducts(int OrderId)
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
                new SqlParameter("@OrderId", OrderId)
                };
                using (DataSet ds = SqlHelper.ExecuteDataset(CONNECTION_STRING, "sp_GetOrderProductList", parameters))
                {
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
                            //foreach (DataRow rowCook in ds.Tables[1].Rows)
                            //{
                            //    user = new User();
                            //    user.UserId = Convert.ToInt32(rowCook["UserId"]);
                            //    user.UserName = Convert.ToString(rowCook["UserName"]);
                            //    user.Name = Convert.ToString(rowCook["Name"]);
                            //    lstUser.Add(user);
                            //}

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



        public async Task<Response> AssignOrder(List<Assign> assign)
        {
            Response objResponse = new Response();
            Assign assignList = new Assign();
            OrderDetail orderDetail = new OrderDetail();
            List<OrderDetail> lstOrderDetail = new List<OrderDetail>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<AssignData>");
                assign.ToList().ForEach(
                emp =>
                {
                    sb.AppendLine("<Assign>");
                    sb.AppendLine(("<Id>"
                                    + emp.Id + "</Id>"));
                    sb.AppendLine(("<UserId>"
                                + emp.AssignUserId + "</UserId>"));
                    sb.AppendLine(("<Type>"
                                + emp.AssignType + "</Type>"));
                    sb.AppendLine("</Assign>");
                });
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
                        assignList.Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]);
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
    }
}