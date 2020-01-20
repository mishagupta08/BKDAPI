using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
    }
}