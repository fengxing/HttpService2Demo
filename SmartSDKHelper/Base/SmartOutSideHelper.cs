using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;

namespace SmartSDKHelper
{
    public static partial class SmartOutSideHelper
    {
        /// <summary>
        /// Http采集帮助类
        /// </summary>
        private class HttpHelper
        {
            public static HttpHelperReturn PostRequest(string LoginUrl, string postData, Dictionary<string, string> dic = null, int time = 15)
            {
                var ret = new HttpHelperReturn();
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                Stream stream = null;
                StreamReader streamReader = null;
                try
                {
                    request = (HttpWebRequest)WebRequest.Create(LoginUrl);
                    request.ServicePoint.ConnectionLimit = 1024;
                    request.Timeout = time * 1000;
                    byte[] data = Encoding.UTF8.GetBytes(postData);
                    if (dic != null)
                    {
                        foreach (var d in dic)
                        {
                            request.Headers.Add(d.Key, d.Value);
                        }
                    }
                    request.Method = "POST";
                    request.ContentType = "text/html";
                    request.ContentLength = data.Length;
                    request.KeepAlive = false;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.100 Safari/534.30";
                    stream = request.GetRequestStream();
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    response = (HttpWebResponse)request.GetResponse();
                    ret.code = (int)response.StatusCode;
                    if (ret.code == 200 ||
                        ret.code == 888 ||
                        ret.code == 601 ||
                        ret.code == 804)
                    {
                        streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                        ret.d = streamReader.ReadToEnd();
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.Timeout)
                    {
                        ret.d = "内部请求超时,最大响应时间为:" + time + "秒";
                        ret.code = -1;
                    }
                    else
                    {
                        #region 处理异常
                        response = (HttpWebResponse)ex.Response;
                        if (response != null)
                        {
                            ret.code = (int)response.StatusCode;
                            try
                            {
                                using (Stream data = response.GetResponseStream())
                                {
                                    using (StreamReader reader = new StreamReader(data))
                                    {
                                        ret.d = reader.ReadToEnd();
                                    }
                                }
                            }
                            catch { };
                        }
                        #endregion
                    }
                }
                finally
                {
                    #region disponse
                    if (stream != null)
                    {
                        stream.Close();
                    }
                    if (response != null)
                    {
                        response.Close();
                    }
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (request != null)
                    {
                        request.Abort();
                    }
                    #endregion
                }
                return ret;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class HttpHelperReturn
        {
            /// <summary>
            /// 请求成功大于0
            /// 内部失败小于0
            /// </summary>
            public int code { get; set; }

            /// <summary>
            /// 数据
            /// </summary>
            public string d { get; set; }

            public bool IsSuccess
            {
                get
                {
                    if (this.code == 200 ||
                        this.code == 888)
                    {
                        return true;
                    }
                    return false;
                }
            }

            public Wrong GetWrong()
            {
                if (this.IsSuccess)
                {
                    return Wrong.None;
                }
                if (this.code <= 0)
                {
                    if (this.code == -1000)
                    {
                        return Wrong.RequestTimeOut;
                    }
                    return Wrong.UnKnow;
                }
                else
                {
                    try
                    {
                        return (Wrong)this.code;
                    }
                    catch (Exception)
                    {
                        return Wrong.UnKnow;
                    }
                }
            }
        }

        #region 地址
        /// <summary>
        /// token
        /// </summary>
        private static LoginToken _token = null;


        private static string _3rdHttp = System.Configuration.ConfigurationManager.AppSettings["3rdHttp"];


        private static string _path;
        /// <summary>
        /// 自动获取调用信息
        /// </summary>
        /// <returns></returns>
        private static string GetPath()
        {
            if (string.IsNullOrEmpty(_path))
            {
                var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
                if (location != null)
                {
                    var s = location.ToLower();
                    var t = "";
                    if (string.IsNullOrEmpty(t))
                    {
                        try
                        {
                            _path = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        _path = t;
                    }
                }
                if (string.IsNullOrEmpty(_path))
                {
                    _path = "Unknow:";
                    try
                    {
                        _path = Dns.GetHostName();
                        _path += "|" + Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
                    }
                    catch (Exception)
                    {

                    }
                }

            }
            return _path;
        }


        private static string GetDLLs()
        {
            try
            {
                var file = System.Reflection.Assembly.GetExecutingAssembly().Location;
                var path = Path.GetDirectoryName(file);
                var files = Directory.GetFiles(path);
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = Path.GetFileName(files[i]);
                }
                return string.Join(",", files);
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static string ip = "";

        private static string GetHostIP()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ip))
                {
                    ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(r => r.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault().ToString();
                }
                return ip;
            }
            catch
            {
                return "";
            }
        }

        private static string GetHostName()
        {
            try
            {
                return Dns.GetHostName();
            }
            catch
            {
                return "";
            }
        }
        #endregion

        private static bool isset = false;

        private static int _timeout = 15;

        /// <summary>
        /// 主动修改服务地址
        /// </summary>
        /// <param name="newUrl"></param>
        public static void Set3rdHttp(string newUrl)
        {
            _3rdHttp = newUrl;
        }

        /// <summary>
        /// 修改超时时间
        /// </summary>
        /// <param name="timeout"></param>
        public static void SetTimeout(int timeout)
        {
            _timeout = timeout;
        }

        private static void Init()
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = GetHostIP();
            }
            if (isset == false)
            {
                isset = true;
                ServicePointManager.DefaultConnectionLimit = 1024;
            }
            var _login = _logins[_3rdHttp];
            if (_login == null)
            {
                throw new Exception("没有注册登录函数");
            }
            if (_jsonConvert == null)
            {
                throw new Exception("没有注册序列化函数");
            }
        }


        private static Result Run(HttpInvoke httpInvoke, Func<Dictionary<string, string>> loadArgs = null)
        {
            Init();
            if (httpInvoke == null)
            {
                throw new Exception("请求参数不正确");
            }
            var postdata = _jsonConvert.SerializeObject(new
            {
                AppID = httpInvoke.AppID,
                Method = httpInvoke.Method,
                RequestObjs = httpInvoke.RequestObjs
            });
            Dictionary<string, string> dic;
            if (loadArgs != null)
            {
                dic = loadArgs() ?? new Dictionary<string, string>();
            }
            else
            {
                dic = new Dictionary<string, string>();
            }
            #region loadArgs
            dic.Add("hversion", httpInvoke.Version);
            dic.Add("channel", GetPath());
            var _login = _logins[_3rdHttp];
            if (_token != null && _login.NeedLogin(httpInvoke.AppID))
            {
                if (_login.IsLocalToken(httpInvoke.AppID))
                {
                    dic.Add("uid", _login.UID);
                    dic.Add("token", _login.Token);
                }
                else
                {
                    dic.Add("uid", _token.UID);
                    dic.Add("token", _token.Token);
                }
            }
            dic.Add("cip", ip);
            dic.Add("sendertime", httpInvoke.SenderTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            if (!string.IsNullOrWhiteSpace(httpInvoke.ID))
            {
                dic.Add("requestid", httpInvoke.ID);
            }
            if (httpInvoke.ExecuteID != Guid.Empty)
            {
                dic.Add("executeid", httpInvoke.ExecuteID.ToString());
            }
            #endregion
            var ret = HttpHelper.PostRequest(_3rdHttp + "Http/CHttpRequest", postdata, dic, _timeout);
            if (ret.IsSuccess == false)
            {
                var error = ret.GetWrong();
                if (error == Wrong.UnLogin || error == Wrong.LoginTimeOut)
                {
                    SetToken(GetLogin().Login(httpInvoke.AppID));
                }
                if (httpInvoke.CanRetry && httpInvoke.RetryTimes > 0)
                {
                    for (int i = 0; i < httpInvoke.RetryTimes; i++)
                    {
                        Console.WriteLine("重试{0}次", i + 1);
                        ret = HttpHelper.PostRequest(_3rdHttp + "Http/CHttpRequest", postdata, dic, _timeout);
                        if (ret.IsSuccess)
                        {
                            break;
                        }
                    }
                }
            }
            return new Result()
            {
                Data = ret.d,
                IsSuccess = ret.IsSuccess,
                HttpCode = ret.code,
                Wrong = ret.GetWrong(),
            };
        }

        private static Result Run(HttpInvokes httpInvokes, Func<Dictionary<string, string>> loadArgs = null)
        {
            Init();
            if (httpInvokes == null || httpInvokes.HttpCommands == null)
            {
                throw new Exception("请求参数不正确");
            }
            var postdata = _jsonConvert.SerializeObject(new
            {
                AppID = httpInvokes.AppID,
                HttpCommands = httpInvokes.HttpCommands
            });
            Dictionary<string, string> dic;
            if (loadArgs != null)
            {
                dic = loadArgs() ?? new Dictionary<string, string>();
            }
            else
            {
                dic = new Dictionary<string, string>();
            }
            #region loadArgs
            dic.Add("channel", GetPath());
            var _login = _logins[_3rdHttp];
            if (_token != null && _login.NeedLogin(httpInvokes.AppID))
            {
                if (_login.IsLocalToken(httpInvokes.AppID))
                {
                    dic.Add("uid", _login.UID);
                    dic.Add("token", _login.Token);
                }
                else
                {
                    dic.Add("uid", _token.UID);
                    dic.Add("token", _token.Token);
                }
            }
            if (httpInvokes.ExexuteID.HasValue)
            {
                dic.Add("executeid", httpInvokes.ExexuteID.Value.ToString());
            }
            dic.Add("cip", ip);
            dic.Add("sendertime", httpInvokes.SenderTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            #endregion
            var ret = HttpHelper.PostRequest(_3rdHttp + "Http/CHttpRequests", postdata, dic, _timeout);
            if (ret.IsSuccess == false)
            {
                var error = ret.GetWrong();
                if (error == Wrong.UnLogin || error == Wrong.LoginTimeOut)
                {
                    SetToken(GetLogin().Login(httpInvokes.AppID));
                }
                if (httpInvokes.CanRetry && httpInvokes.RetryTimes > 0)
                {
                    for (int i = 0; i < httpInvokes.RetryTimes; i++)
                    {
                        Console.WriteLine("重试{0}次", i + 1);
                        ret = HttpHelper.PostRequest(_3rdHttp + "Http/CHttpRequests", postdata, dic, _timeout);
                        if (ret.IsSuccess)
                        {
                            break;
                        }
                    }
                }
            }
            return new Result()
            {
                Data = ret.d,
                IsSuccess = ret.IsSuccess,
                Wrong = ret.GetWrong(),
                HttpCode = ret.code,
            };
        }
    }
}
