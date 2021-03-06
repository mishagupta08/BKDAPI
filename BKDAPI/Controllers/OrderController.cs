﻿using BKDAPI.Models;
using Newtonsoft.Json;
using System.Net;
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
        [HttpGet, Route("api/Order/ProductList/{OrderId}/{UserId}")]
        public async Task<IHttpActionResult> OrderProduct(int OrderId,int UserId)
        {
            repository = new OrderRepository();
            var objResponse = await repository.GetOrderProducts(OrderId,UserId);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }
        [HttpPost, Route("api/Order/AssignOrder")]
        public async Task<IHttpActionResult> AssignOrder()
        {
            repository = new OrderRepository();
            var detail = await Request.Content.ReadAsStringAsync();
            var assignList = JsonConvert.DeserializeObject<OrderAssignment>(detail);
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

        [HttpGet, Route("api/Order/GetAssignedOrderList/{UserId}")]
        public async Task<IHttpActionResult> GetAssignedOrderList(int UserId)
        {
            repository = new OrderRepository();
            var objResponse = await repository.GetAssignedOrderList(UserId);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost, Route("api/Order/UpdateStatus")]
        public async Task<IHttpActionResult> UpdateStatus()
        {
            repository = new OrderRepository();
            var detail = await Request.Content.ReadAsStringAsync();
            var assignList = JsonConvert.DeserializeObject<WorkStatus>(detail);
            var objResponse = await repository.UpdateStatus(assignList);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost, Route("api/Order/LessStock")]
        public async Task<IHttpActionResult> LessStock()
        {
            repository = new OrderRepository();
            var detail = await Request.Content.ReadAsStringAsync();
            var assignList = JsonConvert.DeserializeObject<UsedStallProducts>(detail);
            var objResponse = await repository.LessStock(assignList);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet, Route("api/Order/GetStock/{PartyCode}")]
        public async Task<IHttpActionResult> GetStock(string PartyCode)
        {
            repository = new OrderRepository();                        
            var objResponse = await repository.GetStockReport(PartyCode);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet, Route("api/Order/GetStockSummary/{PartyCode}/{From}/{To}")]
        public async Task<IHttpActionResult> GetStockSummary(string PartyCode,string From,string To)
        {
            repository = new OrderRepository();
            var objResponse = await repository.GetDateWiseStockReport(PartyCode,From,To);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }


    }
}
