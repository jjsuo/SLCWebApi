using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SLCWebApi.App_Start
{
    public class TestController : ApiController
    {
        public string Get(int id)
        {
            return "value";
        }

    }
}
