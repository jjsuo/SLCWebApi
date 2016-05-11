using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using BusinessService;

namespace SLCWebApi.Controllers
{
     [RoutePrefix("api/Contact")]
    public class ContactController : ApiController
    {
        [HttpGet]
        [Route("GetContactInfo")]
        public User GetContactInfo(string userId)
        {
            try
            {
                UserService service = new UserService();
                return service.GetUser(userId);
            }
            catch (Exception exception)
            {
                { }
                throw;
            }

        }
    }
}
