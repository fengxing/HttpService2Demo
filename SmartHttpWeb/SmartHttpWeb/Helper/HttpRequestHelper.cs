using Newtonsoft.Json;
using SmartBLL;
using SmartHttpEntity;
using System;
using System.Collections.Generic;

namespace SmartHttpWeb.Helper
{
    public class HttpRequestHelper
    {
        public static R Request(string ip, HttpMessage entity, string token, string uid, List<string> ValueArgs, string version, string HashCode, int? Sort)
        {
            HttpHelper http = new HttpHelper() { Version = version, IP = ip.ToTrim() };
            http.ContentType = "text/html";
            var _3rdHttp = System.Configuration.ConfigurationManager.AppSettings["3rdHttp"];
            if (ValueArgs == null)
            {
                ValueArgs = new List<string>();
            }
            if (!string.IsNullOrWhiteSpace(_3rdHttp))
            {
                http.UID = uid;
                http.Token = token;
                http.AppID = entity.AppID;
                http.ExecuteID = HashCode;
                http.Sort = Sort.HasValue ? Sort.Value : 0;
                var json = JsonConvert.SerializeObject(new { AppID = entity.AppID, Method = entity.Method, RequestObjs = ValueArgs });
                http.datas = json;
                var ret = http.PostRequest(_3rdHttp + "Http/HttpRequest");
                if (ret == null || string.IsNullOrWhiteSpace(ret.Return))
                {
                    throw new BException("远程访问内部会员服务失败:" + _3rdHttp + "Http/HttpRequest");
                }
                if (ret.StatusCode == 200)
                {
                    var httpReturn = JsonConvert.DeserializeObject<HttpReturn>(ret.Return);
                    var code = "(InnerHttpCode:" + httpReturn.StatusCode + ")";
                    if (httpReturn.StatusCode == 200 ||
                        httpReturn.StatusCode == 888)
                    {
                        if (httpReturn.IsApiCache)
                        {
                            httpReturn.Url = httpReturn.Url + "【ApiCache】";
                        }
                        else if (httpReturn.IsServiceCache)
                        {
                            httpReturn.Url = httpReturn.Url + "【ServiceCache】";
                        }
                        return new R()
                        {
                            err = httpReturn.ExceptionMessage,
                            result = true,
                            request = httpReturn.Request,
                            response = httpReturn.Response,
                            url = httpReturn.Url + code
                        };
                    }
                    else
                    {
                        return new R()
                        {
                            err = httpReturn.ExceptionMessage,
                            result = false,
                            request = httpReturn.Request,
                            response = httpReturn.Response,
                            url = httpReturn.Url + code
                        };
                    }
                }
                else
                {
                    throw new BException("3rdHttp配置为空");
                }
            }
            else
            {
                throw new BException("3rdHttp配置为空");
            }
        }


        public static LoginResponse GetLogin(string account, string pwd, int appid)
        {
            pwd = pwd.ToMd532Upper();
            var login = SmartSDKHelper.SmartOutSideHelper.Request<LoginResponse>(new SmartSDKHelper.HttpInvoke()
            {
                AppID = appid,
                Method = "Login",
                Version = "1.0",
                CanRetry = false,
                RequestObjs = new List<string>() { pwd, account },
                RetryTimes = 0,
                SenderTime = DateTime.Now,
                ID = Guid.NewGuid().ToString()
            }, (error) => { error.ThrowIfFailed(); });
            return login;
        }
    }

    public class R
    {
        public string err { get; set; }

        public bool result { get; set; }

        public string request { get; set; }
        public string response { get; set; }
        public string url { get; set; }


    }


    /// <summary>
    /// Http返回值
    /// </summary>
    public class HttpReturn
    {
        /// <summary>
        /// MallID
        /// </summary>
        public long MallID { get; set; }

        /// <summary>
        /// 访问的接口
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 请求的URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求的消息
        /// </summary>
        public string Request { get; set; }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// 请求的参数
        /// </summary>
        public List<string> RequestObjs { get; set; }

        /// <summary>
        /// 访问方式
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string Moudle { get; set; }

        /// <summary>
        /// 待加解密信息
        /// </summary>
        public string ResponseEncrypt { get; set; }

        /// <summary>
        /// 待加解密信息
        /// </summary>
        public string RequestEncrypt { get; set; }

        /// <summary>
        /// 返回状态
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsServiceCache { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsApiCache { get; set; }
    }



    /// <summary>
    ///登录请求参数
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LoginResponse()
        {
            this.Token = "";
            this.UserName = "";
            this.UID = "";
        }
    }
}