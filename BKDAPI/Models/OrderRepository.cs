using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BKDAPI.Models
{
    public class OrderRepository
    {
        //public async Task<Response> List(int userId)
        //{
        //    Response objResponse = new Response();
        //    try
        //    {
        //        using (var entities = new BKDHEntities())
        //        {
        //            var user = await Task.Run(() => entities.Inv_M_UserMaster.FirstOrDefault(r => r.UserId == userId));
        //            if (user != null)
        //            {
        //                var productList = await Task.Run(() => entities.trnFoodOrderMains.Where(r => r.u == userId).ToList());
        //                if (productList != null)
        //                {
        //                    objResponse.Status = true;
        //                    objResponse.ResponseValue = new JavaScriptSerializer().Serialize(productList);
        //                    objResponse.ResponseMessage = productList.Count + " products found in cart.";
        //                }
        //                else
        //                {
        //                    objResponse.Status = false;
        //                    objResponse.ResponseMessage = "Cart empty";
        //                }
        //            }
        //            else
        //            {
        //                objResponse.Status = false;
        //                objResponse.ResponseMessage = "User Not Found.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objResponse.Status = false;
        //        objResponse.ResponseMessage = ex.Message;
        //    }
        //    return objResponse;
        //}
    }
}