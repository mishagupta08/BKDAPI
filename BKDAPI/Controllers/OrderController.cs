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
    }
}
