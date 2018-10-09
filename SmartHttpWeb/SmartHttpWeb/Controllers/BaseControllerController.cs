using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartHttpWeb.Controllers
{

    public class BaseController : Controller
    {
        public static string e = System.Configuration.ConfigurationManager.AppSettings["E"];  
  

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controller = filterContext.RouteData.Values["controller"].ToString();
            string action = filterContext.RouteData.Values["action"].ToString();
            ViewBag.url = string.Format("/{0}/{1}", controller, action);
            base.OnActionExecuting(filterContext);
        }
    }
}