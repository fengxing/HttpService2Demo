using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SmartHttp
{
    /// <summary>
    /// Http采集帮助类
    /// </summary>
    public class HttpHelper
    {
        private HttpWebRequest request;
        private HttpWebResponse response;
        private Stream stream;
        private StreamReader streamReader;

        public Encoding Encoding { get; set; }
        public string RequestData { get; set; }
        public string UserAgent { get; set; }
        public string ContentType { get; set; }
        public List<string> Headers { get; set; }
        public Dictionary<string, string> DHeaders { get; set; }
        public int Timeout { get; set; }

        public string Token { get; set; }
        public string SenderTime { get; set; }

        /// <summary>
        /// 默认构造 UTF-8
        /// Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.204 Safari/534.16
        /// </summary>
        public HttpHelper()
        {
            this.ContentType = "application/x-www-form-urlencoded";
            this.Encoding = Encoding.UTF8;
            this.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.100 Safari/534.30";
            RequestData = "";
            this.Headers = new List<string>();
            this.Timeout = 10;
            this.DHeaders = new Dictionary<string, string>();
        }


        public HttpHelperReturn PutRequest(string LoginUrl)
        {
            var ret = new HttpHelperReturn();
            try
            {
                request = (HttpWebRequest)WebRequest.Create(LoginUrl);
                request.Proxy = null;
                request.ServicePoint.ConnectionLimit = 1024;
                if (this.Timeout > 0)
                {
                    request.Timeout = this.Timeout * 1000;
                }
                byte[] data = Encoding.GetBytes(RequestData);
                foreach (var header in this.Headers)
                {
                    request.Headers.Add(header);
                }
                foreach (var header in DHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
                request.Method = "PUT";
                request.ContentType = this.ContentType;
                request.ContentLength = data.Length;
                request.KeepAlive = false;
                request.UserAgent = UserAgent;
                stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                response = (HttpWebResponse)request.GetResponse();
                ret.StatusCode = (int)response.StatusCode;
                streamReader = new StreamReader(response.GetResponseStream(), Encoding);
                ret.Return = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                ProcessError(ret, ex);
            }
            finally
            {
                Close();
            }
            return ret;
        }

        public HttpHelperReturn PostRequest(string LoginUrl)
        {
            var ret = new HttpHelperReturn();
            try
            {
                request = (HttpWebRequest)WebRequest.Create(LoginUrl);
                request.Proxy = null;
                request.ServicePoint.ConnectionLimit = 1024;
                if (this.Timeout > 0)
                {
                    request.Timeout = this.Timeout * 1000;
                }
                byte[] data = Encoding.GetBytes(RequestData);
                foreach (var header in this.Headers)
                {
                    request.Headers.Add(header);
                }
                foreach (var header in DHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
                request.Method = "POST";
                request.ContentType = this.ContentType;
                request.ContentLength = data.Length;
                request.KeepAlive = true;
                request.UserAgent = UserAgent;
                stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                response = (HttpWebResponse)request.GetResponse();
                ret.StatusCode = (int)response.StatusCode;
                streamReader = new StreamReader(response.GetResponseStream(), Encoding);         
                ret.Return = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                ProcessError(ret, ex);
            }
            finally
            {
                Close();
            }
            return ret;
        }

        public HttpHelperReturn GetRequest(string LocationUrl)
        {
            var ret = new HttpHelperReturn();
            try
            {
                request = (HttpWebRequest)WebRequest.Create(LocationUrl);
                request.Proxy = null;
                request.ServicePoint.ConnectionLimit = 1024;
                if (this.Timeout > 0)
                {
                    request.Timeout = this.Timeout * 1000;
                }
                foreach (var header in this.Headers)
                {
                    request.Headers.Add(header);
                }
                foreach (var header in DHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
                request.Method = "GET";
                request.KeepAlive = false;
                request.UserAgent = UserAgent;
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                streamReader = new StreamReader(stream, Encoding);
                ret.StatusCode = (int)response.StatusCode;
                ret.Return = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                ProcessError(ret, ex);
            }
            finally
            {
                Close();
            }
            return ret;
        }

        private void Close()
        {
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
        }

        private void ProcessError(HttpHelperReturn ret, WebException ex)
        {
            if (ex.Status == WebExceptionStatus.Timeout)
            {
                var time = this.Timeout == 0 ? 10 : this.Timeout;
                throw new Exception("请求接口超时,超时时间为" + time + "秒");
            }
            HttpWebResponse response = (HttpWebResponse)ex.Response;
            if (response != null)
            {
                ret.StatusCode = (int)response.StatusCode;
                try
                {
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
            else
            {
                throw ex;
            }
        }

        public Dictionary<string, string> cookies = new Dictionary<string, string>();
    }

    public class HttpHelperReturn
    {
        public int StatusCode { get; set; }

        public string Return { get; set; }
    }
}
