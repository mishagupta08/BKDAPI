using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BKDAPI.Models
{
    public class CartRepository
    {
        public async Task<Response> List(int userId)
        {
            Response objResponse = new Response();
            try
            {
                FoodCart objFoodCart = new FoodCart();
                using (var entities = new BKDHEntities())
                {
                    var user = await Task.Run(() => entities.Inv_M_UserMaster.FirstOrDefault(r => r.UserId == userId));
                    if (user != null)
                    {
                        var CartproductList = await Task.Run(() => entities.trnFoodCarts.Where(r => r.User_id == userId).ToList());
                       
                        if (CartproductList != null && CartproductList.Count()>0)
                        {   
                            objFoodCart.TotalItem = CartproductList.Sum(r=>r.Quantity)??0;
                            objFoodCart.TotalAmount = CartproductList.Sum(r => r.TotalAmount) ?? 0;
                            objFoodCart.TotalTax = CartproductList.Sum(r => r.TotalTax) ?? 0;
                            objFoodCart.TotalPrice = CartproductList.Sum(r => r.TotalPrice) ?? 0;
                            objFoodCart.User_id = userId;
                            objFoodCart.ProductList = (from r in CartproductList
                                                       join t in entities.M_ProductMaster on r.ProductCode equals t.ProductCode
                                                       where r.User_id == userId
                                                       select (new FoodItem
                                                       {
                                                           ProductCode = r.ProductCode,
                                                           ProductName = t.ProductName,
                                                           ProductImage = t.ImagePath,
                                                           Quantity = r.Quantity??0,
                                                           Price = r.Price,
                                                           TotalAmount =r.TotalAmount,
                                                           TaxAmt = r.TaxAmt,
                                                           TaxPer = r.TaxPer,
                                                           TotalPrice = r.TotalPrice
                                                       })).ToList();

                            objResponse.Status = true;
                            objResponse.ResponseValue = new JavaScriptSerializer().Serialize(objFoodCart);
                            objResponse.ResponseMessage = CartproductList.Count + " products found in cart.";
                        }
                        else
                        {
                            objResponse.Status = false;
                            objResponse.ResponseValue = new JavaScriptSerializer().Serialize(objFoodCart);
                            objResponse.ResponseMessage = "Cart empty";
                        }
                    }
                    else
                    {
                        objResponse.Status = false;
                        objResponse.ResponseMessage = "User Not Found.";
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

        public async Task<Response> Add(FoodCart UserCartDetail)
        {
            Response objResponse = new Response();
            try
            {
                using (var entities = new BKDHEntities())
                {
                    var user = await Task.Run(() => entities.Inv_M_UserMaster.FirstOrDefault(r => r.UserId == UserCartDetail.User_id));
                    if (user != null)
                    {
                        if (UserCartDetail.ProductList != null && UserCartDetail.ProductList.Count() > 0)
                        {
                            foreach (var product in UserCartDetail.ProductList)
                            {
                                var cartDetail = new trnFoodCart();

                                var productDetail = await Task.Run(() => entities.M_ProductMaster.FirstOrDefault(r => r.ProductCode == product.ProductCode));
                                if (productDetail != null)
                                {
                                    var objCartItem = await Task.Run(() => entities.trnFoodCarts.FirstOrDefault(r => r.User_id == UserCartDetail.User_id && r.ProductCode == product.ProductCode));
                                    if (objCartItem == null)
                                    {
                                        cartDetail.ProductCode = product.ProductCode;
                                        cartDetail.User_id = UserCartDetail.User_id;
                                        cartDetail.Quantity = product.Quantity;
                                        cartDetail.TaxPer = productDetail.BuyingTax;
                                        cartDetail.TaxAmt = (productDetail.MRP) * ((productDetail.BuyingTax) / 100);
                                        cartDetail.Price = productDetail.MRP - cartDetail.TaxAmt;
                                        cartDetail.TotalTax = cartDetail.TaxAmt * product.Quantity;
                                        cartDetail.TotalPrice = cartDetail.Price * product.Quantity;
                                        cartDetail.TotalAmount = cartDetail.TotalTax + cartDetail.TotalPrice;
                                        cartDetail.CreatedDate = DateTime.Now;
                                        entities.trnFoodCarts.Add(cartDetail);
                                    }
                                    else
                                    {
                                        var Quantity = product.Quantity + objCartItem.Quantity;
                                        objCartItem.ProductCode = product.ProductCode;
                                        objCartItem.User_id = UserCartDetail.User_id;
                                        objCartItem.Quantity = Quantity;
                                        objCartItem.TaxPer = productDetail.BuyingTax;
                                        objCartItem.TaxAmt = (productDetail.MRP) * ((productDetail.BuyingTax) / 100);
                                        objCartItem.Price = productDetail.MRP - objCartItem.TaxAmt;
                                        objCartItem.TotalTax = objCartItem.TaxAmt * Quantity;
                                        objCartItem.TotalPrice = objCartItem.Price * Quantity;
                                        objCartItem.TotalAmount = objCartItem.TotalTax + objCartItem.TotalPrice;
                                    }
                                }
                                await entities.SaveChangesAsync();
                                objResponse.Status = true;
                                objResponse.ResponseMessage = "Item added to cart.";
                            }
                        }
                        else
                        {
                            objResponse.Status = false;
                            objResponse.ResponseMessage = "No product found";
                        }
                    }
                    else
                    {
                        objResponse.Status = false;
                        objResponse.ResponseMessage = "User Not Found.";
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

        public async Task<Response> Update(trnFoodCart cartDetail)
        {
            Response objResponse = new Response();
            try
            {
                using (var entities = new BKDHEntities())
                {
                    if (cartDetail != null)
                    {
                        var user = await Task.Run(() => entities.Inv_M_UserMaster.FirstOrDefault(r => r.UserId == cartDetail.User_id));
                        if (user != null)
                        {
                            var objCartItem = await Task.Run(() => entities.trnFoodCarts.FirstOrDefault(r => r.User_id == cartDetail.User_id && r.ProductCode==cartDetail.ProductCode));
                            if (objCartItem != null)
                            {
                                var productDetail = await Task.Run(() => entities.M_ProductMaster.FirstOrDefault(r => r.ProductCode == cartDetail.ProductCode));
                                if (productDetail != null)
                                {
                                    objCartItem.Quantity = cartDetail.Quantity;
                                    objCartItem.TaxPer = productDetail.BuyingTax;
                                    objCartItem.TaxAmt = (productDetail.MRP) * ((productDetail.BuyingTax) / 100);
                                    objCartItem.Price = productDetail.MRP - objCartItem.TaxAmt;
                                    objCartItem.TotalTax = objCartItem.TaxAmt * objCartItem.Quantity;
                                    objCartItem.TotalPrice = objCartItem.Price * objCartItem.Quantity;
                                    objCartItem.TotalAmount = objCartItem.TotalTax + objCartItem.TotalPrice;                                                                                                            
                                }
                            }                           
                            await entities.SaveChangesAsync();
                            objResponse.Status = true;
                            objResponse.ResponseMessage = "Cart item updated";
                        }
                        else
                        {
                            objResponse.Status = false;
                            objResponse.ResponseMessage = "User not found.";
                        }
                    }
                    else
                    {
                        objResponse.Status = false;
                        objResponse.ResponseMessage = "Send Complete Details.";
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

        public async Task<Response> Delete(trnFoodCart cartDetail)
        {
            Response objResponse = new Response();
            try
            {
                using (var entities = new BKDHEntities())
                {
                    if (cartDetail != null)
                    {
                        var user = await Task.Run(() => entities.Inv_M_UserMaster.FirstOrDefault(r => r.UserId == cartDetail.User_id));
                        if (user != null)
                        {
                            var objCartItem = await Task.Run(() => entities.trnFoodCarts.FirstOrDefault(r => r.User_id == cartDetail.User_id && r.ProductCode == cartDetail.ProductCode));
                            if (objCartItem != null)
                            {
                                entities.trnFoodCarts.Remove(objCartItem);
                            }
                            await entities.SaveChangesAsync();
                            objResponse.Status = true;
                            objResponse.ResponseMessage = "Item Removed";
                        }
                        else
                        {
                            objResponse.Status = false;
                            objResponse.ResponseMessage = "User not found.";
                        }
                    }
                    else
                    {
                        objResponse.Status = false;
                        objResponse.ResponseMessage = "Send Complete Details.";
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