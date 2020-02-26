using BKDAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BKDAPI.Controllers
{
    public class OrderController : ApiController
    {
        public OrderRepository repository;
        [HttpPost, Route("api/Order/PostOrder")]
        public async Task<IHttpActionResult> PostOrder()
        {
            repository = new OrderRepository();
            var detail = await Request.Content.ReadAsStringAsync();
            var Credentials = JsonConvert.DeserializeObject<Inv_M_UserMaster>(detail);
            var objResponse = await repository.PostOrder(Credentials);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet, Route("api/Order/List/{UserId}")]
        public async Task<IHttpActionResult> List(int UserId)
        {
            repository = new OrderRepository();
            var objResponse = await repository.GetOrderList(UserId);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }
        [HttpPost, Route("api/Order/AssignOrder")]
        public async Task<IHttpActionResult> AssignOrder()
        {
            repository = new OrderRepository();
            var detail = await Request.Content.ReadAsStringAsync();
            var assignList = JsonConvert.DeserializeObject<List<Assign>>(detail);
            var objResponse = await repository.AssignOrder(assignList);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet, Route("api/Order/GetOrderStatus/{UserId}")]
        public async Task<IHttpActionResult> GetOrderStatus(int UserId)
        {
            repository = new OrderRepository();
            var objResponse = await repository.GetOrderStatus(UserId);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }
    }
}
