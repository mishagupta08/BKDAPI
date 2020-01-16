using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BKDAPI.Models
{
    public class ProductRepository
    {
        public async Task<Response> List()
        {
            Response objResponse = new Response();
            try
            {
                using (var entities = new BKDHEntities())
                {
                    var productList = await Task.Run(() => entities.M_ProductMaster.Where(r=>r.ActiveStatus=="Y").ToList());                   
                    if (productList != null)
                    {
                        objResponse.Status = true;
                        objResponse.ResponseValue = new JavaScriptSerializer().Serialize(productList);
                        objResponse.ResponseMessage = productList.Count + " products found." ;                        
                    }
                    else
                    {
                        objResponse.Status = false;
                        objResponse.ResponseMessage = "No Product Found";
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

        public async Task<Response> GetProduct(int code)
        {
            Response objResponse = new Response();
            try
            {
                using (var entities = new BKDHEntities())
                {
                    var product = await Task.Run(() => entities.M_ProductMaster.FirstOrDefault(r => r.ProductCode == code && r.ActiveStatus == "Y"));
                    if (product != null)
                    {
                        objResponse.Status = true;
                        objResponse.ResponseValue = new JavaScriptSerializer().Serialize(product);
                        objResponse.ResponseMessage = "Product found.";
                    }
                    else
                    {
                        objResponse.Status = false;
                        objResponse.ResponseMessage = "Product not Found";
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
    }
}