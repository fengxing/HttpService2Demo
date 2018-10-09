using SmartBaseEntity;
using SmartSDKHelper;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmartHttpWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var jsonConvert = new JsonConvert1();
            var loginProcess = new LoginProcess();
            SmartOutSideHelper.Register(jsonConvert, loginProcess);
            DapperHelper.DapperDb.Register(new CommitUser());
        }
    }

    public class CommitUser : ICommitUser
    {
        public SmartBaseEntity.CommitUser GetCommitUser()
        {
            return new SmartBaseEntity.CommitUser()
            {

            };
        }
    }

    public class JsonConvert1 : IJsonConvert
    {
        public T DeserializeObject<T>(string value)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            }
            catch { }
            return default(T);
        }

        public string SerializeObject(object value)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }
            catch { }
            return "";
        }
    }

    public class LoginProcess : ILogin
    {
        public string UID => "";

        public string Token => "";

        public bool IsLocalToken(int appID)
        {
            return true;
        }

        public LoginToken Login(int appID)
        {
            return new LoginToken() { Token = "", UID = "" };
        }

        public bool NeedLogin(int appID)
        {
            return false;
        }
    }
}
