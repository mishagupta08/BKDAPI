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
    public class CartController : ApiController
    {
        public CartRepository repository;

        [HttpGet, Route("api/Cart/List/{UserId}")]
        public async Task<IHttpActionResult> List(int UserId)
        {
            repository = new CartRepository();
            var objResponse = await repository.List(UserId);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost, Route("api/Cart/Add")]
        public async Task<IHttpActionResult> Add()
        {
            repository = new CartRepository();
            var detail = await Request.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<FoodCart>(detail);
            var objResponse = await repository.Add(product);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost, Route("api/Cart/Update")]
        public async Task<IHttpActionResult> Update()
        {
            repository = new CartRepository();
            var detail = await Request.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<trnFoodCart>(detail);
            var objResponse = await repository.Update(product);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost, Route("api/Cart/Delete")]
        public async Task<IHttpActionResult> Delete()
        {
            repository = new CartRepository();
            var detail = await Request.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<trnFoodCart>(detail);
            var objResponse = await repository.Delete(product);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }
    }
}
