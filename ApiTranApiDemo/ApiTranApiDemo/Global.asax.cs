using ApiBase;
using ApiBase.Core;
using ApiTranService;
using System.Web.Http;

namespace ApiTranApiDemo
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        protected void Application_Start()
        {
#if DEBUG
            GlobalConfiguration.Configuration.MessageHandlers.Add(new HttpGetDelegatingHandler());
#endif
            GlobalConfiguration.Configure(WebApiConfig.RegisterAll);
            var commitUser = new TagService();
            SmartDbHelper.DbFactory.Register(commitUser);
            DapperHelper.DapperDb.Register(commitUser);
        }
    }
}
