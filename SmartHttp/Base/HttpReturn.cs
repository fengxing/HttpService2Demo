using SmartHttpEntity;
using System.Collections.Generic;
using System.Text;

namespace SmartHttp
{
    /// <summary>
    /// Http返回值
    /// </summary>
    public class HttpReturn
    {
        /// <summary>
        /// AppID
        /// </summary>
        public int AppID { get; set; }

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
        public bool IsSuccess
        {
            get
            {
                if (this.StatusCode == 200 || this.StatusCode == 888)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 异常
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public string Moudle { get; set; }

        /// <summary>
        /// 解密后的返回信息
        /// </summary>
        public string ResponseEncrypt { get; set; }

        /// <summary>
        /// 加密后的请求信息
        /// </summary>
        public string RequestEncrypt { get; set; }

        /// <summary>
        /// 返回状态
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int LoopTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsServiceCache { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsApiCache
        {
            get
            {
                if (this.StatusCode == 888)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public WsExcepiton WsExcepiton { get; set; }

        public string ErrorMessage()
        {
            if (!string.IsNullOrWhiteSpace(this.Response))
            {
                return this.Response;
            }
            return this.ExceptionMessage;
        }
    }
}
