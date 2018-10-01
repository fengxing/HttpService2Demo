using ApiBase.Controllers;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace ApiBase.Core
{
    public class ControllerNotFoundSelector : DefaultHttpControllerSelector
    {
        public ControllerNotFoundSelector(HttpConfiguration configuration)
            : base(configuration)
        {

        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            try
            {
                return base.SelectController(request);
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage() {
                    StatusCode = (HttpStatusCode)999,
                    Content = new StringContent("ApiCNotFoundException") });
            }
        }
    }


    public class ActionNotFoundSelector : ApiControllerActionSelector
    {
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            try
            {
                return base.SelectAction(controllerContext);
            }
            catch (HttpResponseException)
            {
                throw new HttpResponseException(new HttpResponseMessage() { StatusCode = (HttpStatusCode)999, Content = new StringContent("ApiANotFoundException!") });
            }
        }

    }

}