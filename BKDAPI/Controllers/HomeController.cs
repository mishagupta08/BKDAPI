using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BKDAPI.Controllers
{
    public class HomeController : ApiController
    {
        public HomeRepository repository;

        [HttpPost, Route("api/Home/ValidateUser")]
        public async Task<IHttpActionResult> Login(string MobileNo)
        {
            repository = new HomeRepository();
            var objResponse = await repository.Login(MobileNo);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }
    }
}
