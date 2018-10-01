using ApiBase.Core;
using SmartBaseEntity;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;

namespace SmartHttpService
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            cors.SupportsCredentials = true;
            cors.PreflightMaxAge = 300;
            cors.ExposedHeaders.Add("ApiCache");
            cors.ExposedHeaders.Add("ServiceCache");
            cors.ExposedHeaders.Add("method");
            cors.ExposedHeaders.Add("ErrorMethod");
            cors.ExposedHeaders.Add("ErrorIndex");
            GlobalConfiguration.Configuration.EnableCors(cors);
            GlobalConfiguration.Configure(RegisterBase);
            GlobalConfiguration.Configure(RegisterAttrHandler);
            ServicePointManager.ServerCertificateValidationCallback += (object sender, System.Security.Cryptography.X509Certificates.X509Certificate cert, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors error) => { return true; };
            ServicePointManager.Expect100Continue = false;
            Smart.Http.AllEncrypt.Facade.LoadVipOperator();
            DapperHelper.DapperDb.Register(new CommitUser());
            SmartLogHelper.LogTxt.GetFilePath = GetPath;
        }

        public static string GetPath()
        {
            var path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin", "Log", DateTime.Now.ToString("yyyyMMdd"));
            return path;
        }

        public class CommitUser : ICommitUser
        {
            public SmartBaseEntity.CommitUser GetCommitUser()
            {
                return new SmartBaseEntity.CommitUser();
            }
        }

        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }


        public static void RegisterAttrHandler(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IHttpControllerSelector), new ControllerNotFoundSelector(config));
            config.Services.Replace(typeof(IHttpActionSelector), new ActionNotFoundSelector());
        }

        public static void RegisterBase(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
               name: "Error404",
               routeTemplate: "{*url}",
               defaults: new { controller = "Error", action = "Handle404" }
           );
            ServicePointManager.DefaultConnectionLimit = 1024;
        }
    }
}
