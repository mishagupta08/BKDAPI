using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BKDAPI.Models
{
    public class UserRepository
    {
        public async Task<Response> Validate(Inv_M_UserMaster user)
        {
            Response objResponse = new Response();
            try
            {
                using (var entities = new BKDHEntities())
                {                    
                    var tblUser = await Task.Run(() => entities.Inv_M_UserMaster.FirstOrDefault(e => e.UserName == user.UserName && e.Passw==user.Passw));                    
                    
                    if (tblUser != null)
                    {
                        if (tblUser.ActiveStatus == "Y")
                        {
                            objResponse.ResponseValue = new JavaScriptSerializer().Serialize(tblUser);
                            objResponse.Status = true;
                            objResponse.ResponseMessage = "Login Successfull";
                        }
                        else
                        {
                            objResponse.Status = false;
                            objResponse.ResponseMessage = "Inactive User.";
                        }
                    }                  
                    else
                    {
                        objResponse.Status = false;
                        objResponse.ResponseMessage = "Incorrect Credentials";                        
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

        public async Task<Response> GetUserDetail(int userId)
        {
            Response objResponse = new Response();
            try
            {
                using (var entities = new BKDHEntities())
                {
                    var tblUser = await Task.Run(() => entities.Inv_M_UserMaster.FirstOrDefault(e => e.UserId == userId));

                    if (tblUser != null)
                    {
                        objResponse.ResponseValue = new JavaScriptSerializer().Serialize(tblUser);
                        objResponse.Status = true;
                        objResponse.ResponseMessage = "Details fetched successfully";                        
                    }
                    else
                    {
                        objResponse.Status = false;
                        objResponse.ResponseMessage = "User Not found";
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