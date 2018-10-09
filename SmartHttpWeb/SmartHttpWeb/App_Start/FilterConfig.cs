using System;
using System.Web;
using System.Web.Mvc;

namespace SmartHttpWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LogExceptionAttribute());
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class LogExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                //string controllerName = (string)filterContext.RouteData.Values["controller"];
                //string actionName = (string)filterContext.RouteData.Values["action"];
                //string msg = "在执行 controller[{0}] 的 action[{1}] 时产生异常";
                //Exception ex = filterContext.Exception;
                //System.IO.StreamWriter sw = new System.IO.StreamWriter(@"c:\123.txt");
                //sw.WriteLine(ex.Message);
                //sw.Flush();
                //sw.Close();
                //doing some log
            }

            if (filterContext.Result is JsonResult)
            {
                //当结果为json时，设置异常已处理
                filterContext.ExceptionHandled = true;
            }
            else
            {
                //否则调用原始设置
                base.OnException(filterContext);
            }
        }
    }
}
