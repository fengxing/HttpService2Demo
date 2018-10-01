using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiBase.Controllers
{
    public class ErrorController
    {
        [Route("Error/Handle404")]
        [HttpPost, HttpGet]
        public HttpResponseMessage Handle404()
        {
            var ex = new ServiceNotFoundException();
            return new HttpResponseMessage() { StatusCode = (HttpStatusCode)ex.HResult, Content = new StringContent("ServiceNotFoundException") };
        }
    }
}
