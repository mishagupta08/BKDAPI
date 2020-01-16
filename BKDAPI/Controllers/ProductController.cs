using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BKDAPI.Models;

namespace BKDAPI.Controllers
{
    public class ProductController : ApiController
    {
        public ProductRepository repository;

        [HttpGet, Route("api/Product/List")]
        public async Task<IHttpActionResult> List()
        {
            repository = new ProductRepository();           
            var objResponse = await repository.List();
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet, Route("api/Product/{code}")]
        public async Task<IHttpActionResult> GetProduct(int code)
        {
            repository = new ProductRepository();
            var objResponse = await repository.GetProduct(code);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }
    }
}
