using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SmartHttpWeb
{
    /// <summary>
    /// Http采集帮助类
    /// </summary>
    public class HttpHelper
    {
        public CookieContainer myCookieContainer { get; set; }
        private HttpWebRequest request;
        private HttpWebResponse response;
        private Stream stream;
        private StreamReader streamReader;
        public Encoding encoding { get; set; }
        public string datas { get; set; }
        public string UserAgent { get; set; }
        public string ContentType { get; set; }
        public List<string> Headers { get; set; }
        public int Timeout { get; set; }
        public string Token { get; set; }
        public string UID { get; set; }
        public string IP { get; set; }
        public int AppID { get; set; }

        public string ExecuteID { get; set; }

        public int Sort { get; set; }
        public string Version { get; set; }

        /// <summary>
        /// 默认构造 UTF-8
        /// Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.204 Safari/534.16
        /// </summary>
        public HttpHelper()
        {
            this.encoding = Encoding.UTF8;
            this.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.100 Safari/534.30";
            myCookieContainer = new CookieContainer();
            datas = "";
            this.ContentType = "application/x-www-form-urlencoded";
            this.Headers = new List<string>();
        }

        public HttpHelper(Encoding enc)
            : this()
        {
            if (enc != null)
            {
                this.encoding = enc;
            }
        }

        public HttpHelper(Encoding enc, string UserAgent)
            : this()
        {
            if (enc != null)
            {
                this.encoding = enc;
            }
            if (UserAgent != null)
            {
                this.UserAgent = UserAgent;
            }

        }

        public HttpHelperReturn PostRequest(string LoginUrl)
        {
            var ret = new HttpHelperReturn();
            try
            {
                request = (HttpWebRequest)WebRequest.Create(LoginUrl);
                //request.Proxy= new WebProxy("127.0.0.1:8888", true);
                byte[] data = encoding.GetBytes(datas);
                foreach (var header in this.Headers)
                {
                    request.Headers.Add(header);
                }
                if (!string.IsNullOrEmpty(this.Version))
                {
                    request.Headers.Add("hversion", this.Version);
                }
                request.Headers.Add("channel", "HttpWebd");
                request.Headers.Add("token", this.Token);
                request.Headers.Add("cip", this.IP);
                request.Headers.Add("uid", this.UID);
                request.Headers.Add("appid", this.AppID.ToString());
                request.Headers.Add("retry", "true");
                if (!string.IsNullOrWhiteSpace(this.ExecuteID))
                {
                    request.Headers.Add("executeid", this.ExecuteID.ToTrim());
                }
                request.Method = "POST";
                request.ContentType = this.ContentType;
                request.ContentLength = data.Length;
                request.KeepAlive = false;
                request.UserAgent = UserAgent;
                foreach (var cookie in cookies)
                {
                    myCookieContainer.Add(new Uri(LoginUrl), new System.Net.Cookie(cookie.Key, cookie.Value));
                }
                request.CookieContainer = myCookieContainer;
                stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                response = (HttpWebResponse)request.GetResponse();
                ret.StatusCode = (int)response.StatusCode;
                streamReader = new StreamReader(response.GetResponseStream(), encoding);
                ret.Return = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                if (response != null)
                {
                    try
                    {
                        ret.StatusCode = (int)response.StatusCode;
                        using (Stream data = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(data))
                            {
                                ret.Return = reader.ReadToEnd();
                            }
                        }
                    }
                    catch { };
                }
            }
            return ret;
        }

        public HttpHelperReturn GetRequest(string LocationUrl)
        {
            var ret = new HttpHelperReturn();
            try
            {
                request = (HttpWebRequest)WebRequest.Create(LocationUrl);
                if (this.Timeout > 0)
                {
                    request.Timeout = this.Timeout * 1000;
                }
                foreach (var header in this.Headers)
                {
                    request.Headers.Add(header);
                }
                request.Method = "GET";
                request.KeepAlive = false;
                request.UserAgent = UserAgent;
                foreach (var cookie in cookies)
                {
                    myCookieContainer.Add(new System.Net.Cookie(cookie.Key, cookie.Value));
                }
                request.CookieContainer = myCookieContainer;
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                streamReader = new StreamReader(stream, encoding);
                ret.StatusCode = (int)response.StatusCode;
                ret.Return = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                if (response != null)
                {
                    try
                    {
                        ret.StatusCode = (int)response.StatusCode;
                        using (Stream data = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(data))
                            {
                                ret.Return = reader.ReadToEnd();
                            }
                        }
                    }
                    catch { };
                }
            }
            return ret;
        }


        public Dictionary<string, string> cookies = new Dictionary<string, string>();


        /// <summary>
        /// 清除所有的数据
        /// </summary>
        public void ClearDatas()
        {
            this.datas = "";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HttpHelperReturn
    {
        public int StatusCode { get; set; }

        public string Return { get; set; }
    }
}
