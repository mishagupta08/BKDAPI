using System;
using System.Collections.Generic;
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
                    //var tblUser = await Task.Run(() => entities.Inv_M_UserMaster.FirstOrDefault(e => e.UserName == user.UserName && e.Passw == user.Passw));
                    //var GROUP = tblUser
                    var User = await Task.Run(() => (
                    from us in entities.Inv_M_UserMaster
                    join groups in entities.M_GroupMaster on us.GroupId equals groups.GroupId
                    where us.UserName == user.UserName && us.Passw == user.Passw
                    select new User
                    {
                        UserName = us.UserName,
                        UId = us.UId,
                        UserId = us.UserId,
                        Passw = us.Passw,
                        GroupId = us.GroupId,
                        GroupName = groups.GroupName,
                        ActiveStatus = us.ActiveStatus,
                        BranchCode = us.BranchCode,
                        FCode = us.FCode

                    }).FirstOrDefault());
                    if (User != null)
                    {
                        if (User.ActiveStatus == "Y")
                        {
                            objResponse.ResponseValue = new JavaScriptSerializer().Serialize(User);
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

        public async Task<Response> GetUserList(string KitchenCode, string type)
        {
            Response objResponse = new Response();
            var tblUser = new List<KitchenUser>();
            try
            {
                using (var entities = new BKDHEntities())
                {

                    if (!string.IsNullOrEmpty(KitchenCode) && !string.IsNullOrEmpty(type))
                    {
                        if (type.ToLower() == "cook")
                        {
                            tblUser = await Task.Run(() => entities.Inv_M_UserMaster.Where(r => r.BranchCode == KitchenCode && r.GroupId == 103 && r.ActiveStatus == "Y").Select(r=>new KitchenUser {                                
                                UserId = r.UserId,
                                UserName = r.UserName,
                                Passw = r.Passw,
                                ActiveStatus = r.ActiveStatus,
                                BranchCode = r.BranchCode
                            }).ToList());
                                
                        }
                        else if (type.ToLower() == "supervisor")
                        {
                            tblUser = await Task.Run(() => entities.Inv_M_UserMaster.Where(r => r.BranchCode == KitchenCode && r.GroupId == 102 && r.ActiveStatus == "Y").Select(r => new KitchenUser
                            {
                                UserId = r.UserId,
                                UserName = r.UserName,
                                Passw = r.Passw,
                                ActiveStatus = r.ActiveStatus,
                                BranchCode = r.BranchCode
                            }).ToList());
                        }
                        else if (type.ToLower() == "deliveryboy")
                        {
                            tblUser = await Task.Run(() => entities.Inv_M_UserMaster.Where(r => r.BranchCode == KitchenCode && r.GroupId == 104 && r.ActiveStatus == "Y").Select(r => new KitchenUser
                            {
                                UserId = r.UserId,
                                UserName = r.UserName,
                                Passw = r.Passw,
                                ActiveStatus = r.ActiveStatus,
                                BranchCode = r.BranchCode
                            }).ToList());
                        }
                    }

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