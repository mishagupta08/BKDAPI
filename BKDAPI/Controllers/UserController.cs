using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BKDAPI.Models;
using Newtonsoft.Json;

namespace BKDAPI.Controllers
{
    public class UserController : ApiController
    {
        public UserRepository repository;

        [HttpPost, Route("api/User/Validate")]
        public async Task<IHttpActionResult> Validate()
        {
            repository = new UserRepository();
            var detail = await Request.Content.ReadAsStringAsync();
            var Credentials = JsonConvert.DeserializeObject<Inv_M_UserMaster>(detail);
            var objResponse = await repository.Validate(Credentials);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet, Route("api/User/{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            repository = new UserRepository();          
            var objResponse = await repository.GetUserDetail(id);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }
        
        [HttpGet, Route("api/GetUserList/{KitchenCode}/{Type}")]
        public async Task<IHttpActionResult> GetUserList(string KitchenCode,string type)
        {
            repository = new UserRepository();
            var objResponse = await repository.GetUserList(KitchenCode, type);
            return Content(HttpStatusCode.OK, objResponse, Configuration.Formatters.JsonFormatter);
        }
    }
}
