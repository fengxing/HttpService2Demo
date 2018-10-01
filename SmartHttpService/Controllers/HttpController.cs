using SmartHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartHttpService.Controllers
{
    public class HttpController : BaseController
    {
        /// <summary>
        /// 这个方法用户httpweb站点(返回接口请求和返回详情信息)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("Http/HttpRequest")]
        [HttpPost]
        public HttpReturn HttpRequest(HttpRequest request)
        {
            if (request == null)
            {
                return new HttpReturn()
                {
                    AppID = request.AppID,
                    Exception = "用户信息不存在,请先登陆!",
                    ExceptionMessage = "用户信息不存在,请先登陆!",
                    HttpMethod = "",
                    IsServiceCache = false,
                    LoopTime = 0,
                    Method = request.Method,
                    Moudle = "",
                    Request = "",
                    RequestEncrypt = "",
                    RequestObjs = request.RequestObjs,
                    Response = "请求数据为空,请先登陆!",
                    ResponseEncrypt = "",
                    StatusCode = 601,
                    Url = "",
                };
            }
            var httpTag = GetHttpTag();
            var httpMessage = HttpFacade.GetHttpMessage(request.AppID, request.Method, httpTag);
            if (httpMessage != null && httpMessage.IsNeedLogin)
            {
                //这里做token验证
            }
            httpTag.IP = this.Request.GetClientIpString();
            var ret = HttpFacade.Request(request, httpMessage, httpTag);
            if (httpTag.IsPreLoad)
            {
                ret.Response = "";
            }
            return ret;
        }

        /// <summary>
        /// 这个方法给外部调用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("Http/CHttpRequest")]
        [HttpPost]
#if DEBUG
        [HttpGet]
#endif
        public HttpResponseMessage CHttpRequest(HttpRequest request)
        {
            var response = new HttpResponseMessage();
            try
            {
                if (request == null || request.RequestObjs == null)
                {
                    response.StatusCode = (HttpStatusCode)601;
                    response.Content = new StringContent("请求数据为空");
                    return response;
                }
                var httpTag = GetHttpTag();
                var httpMessage = HttpFacade.GetHttpMessage(request.AppID, request.Method, httpTag);
                if (httpMessage != null && httpMessage.IsNeedLogin)
                {
                    //这里做登录验证
                }
                httpTag.IP = this.Request.GetClientIpString();
                var ret = HttpFacade.Request(request, httpMessage, httpTag);
                if (ret.IsSuccess)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    if (!httpTag.IsPreLoad)
                    {
                        response.Content = new StringContent(ret.Response);
                    }
                }
                else
                {
                    response.StatusCode = (HttpStatusCode)ret.StatusCode;
                    if (ret.StatusCode == 601 ||
                        ret.StatusCode == 804)
                    {
                        response.Headers.Add("method", request.Method);
                        response.Content = new StringContent(ret.ErrorMessage());
                    }
                }
                if (ret.IsApiCache)
                {
                    response.Headers.Add("ApiCache", "SameRequest");
                }
                if (ret.IsServiceCache)
                {
                    response.Headers.Add("ServiceCache", "ServiceCache");
                }
                return response;
            }
            catch (Exception ex)
            {
                if (ex is BException)
                {
                    response.StatusCode = (HttpStatusCode)601;
                }
                else
                {
                    response.StatusCode = (HttpStatusCode)990;
                }
                response.Content = new StringContent(ex.Message);
                return response;
            }
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        [Route("Http/CTimeRequest")]
        [HttpGet]
        public HttpResponseMessage CTimeRequest()
        {
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) };
        }
    }
}
