using System;
using System.Collections.Generic;

namespace SmartSDKHelper
{
    /// <summary>
    /// 请求帮助类
    /// </summary>
    public static partial class SmartOutSideHelper
    {
        private static IJsonConvert _jsonConvert;
        private static Dictionary<string, ILogin> _logins = new Dictionary<string, ILogin>();

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="jsonConvert">json序列化接口</param>
        /// <param name="login">登录接口</param>
        /// <param name="url">接口地址</param>
        public static void Register(IJsonConvert jsonConvert,
                                    ILogin login,
                                    string url = "")
        {
            _jsonConvert = jsonConvert;
            if (!string.IsNullOrWhiteSpace(url))
            {
                _3rdHttp = url;
            }
            if (string.IsNullOrWhiteSpace(_3rdHttp))
            {
                throw new Exception("接口地址为空[AppSetting][3rdHttp]");
            }
            if (_logins.ContainsKey(_3rdHttp))
            {
                _logins[_3rdHttp] = login;
            }
            else
            {
                _logins.Add(_3rdHttp, login);
            }
            if (_token == null)
            {
                var appID = 0;
                if (System.Configuration.ConfigurationManager.AppSettings["AppID"] != null)
                {
                    appID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AppID"]);
                }
                SetToken(GetLogin().Login(appID));
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public static void SetToken(LoginToken token)
        {
            _token = token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ILogin GetLogin()
        {
            ILogin _login = null;
            _logins.TryGetValue(_3rdHttp, out _login);
            return _login;
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="httpInvoke"></param>
        /// <param name="loadArgs">Http基础请求添加额外字段</param>
        /// <returns></returns>
        public static Result Request(this HttpInvoke httpInvoke, Func<Dictionary<string, string>> loadArgs = null)
        {
            var result = Run(httpInvoke, loadArgs);
            if (result.Wrong == Wrong.UnLogin || result.Wrong == Wrong.LoginTimeOut)
            {
                result = Run(httpInvoke, loadArgs);
            }
            return result;
        }


        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="httpInvokes"></param>
        /// <param name="loadArgs">Http基础请求添加额外字段</param>
        /// <returns></returns>
        public static Result Requests(this HttpInvokes httpInvokes, Func<Dictionary<string, string>> loadArgs = null)
        {
            var result = Run(httpInvokes, loadArgs);
            return result;
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="httpInvoke"></param>
        /// <param name="processError">异常处理,会抛出异常</param>
        /// <param name="processSuccess">成功处理</param>
        /// <param name="loadArgs">Http基础请求添加额外字段</param>
        /// <returns></returns>
        public static T Request<T>(this HttpInvoke httpInvoke,
                                   Action<Result> processError,
                                   Action<T> processSuccess = null,
                                   Func<Dictionary<string, string>> loadArgs = null) where T : class
        {
            var result = Run(httpInvoke, loadArgs);
            if (result.Wrong == Wrong.UnLogin || result.Wrong == Wrong.LoginTimeOut)
            {
                var token = GetLogin().Login(httpInvoke.AppID);
                SetToken(token);
            }
            if (result.IsSuccess)
            {
                var t = _jsonConvert.DeserializeObject<T>(result.Data);
                if (t != null)
                {
                    processSuccess?.Invoke(t);
                }
                else
                {
                    result.IsSuccess = false;
                    result.Wrong = Wrong.JsonDeserializeNullError;
                    processError(result);
                }
                return t;
            }
            else
            {
                processError(result);
            }
            return null;
        }
    }
}
