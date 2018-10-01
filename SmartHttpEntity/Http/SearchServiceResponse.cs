using System.Collections.Generic;

namespace SmartHttpEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchServiceResponse
    {
        /// <summary>
        /// 特性
        /// </summary>
        public List<string> Attributes { get; set; }

        /// <summary>
        /// 是否允许匿名访问
        /// </summary>
        public bool IsAllowAnonymous
        {
            get
            {
                if (Attributes != null)
                {
                    return this.Attributes.Contains("System.Web.Http.AllowAnonymousAttribute");
                }
                return false;
            }
        }

        /// <summary>
        /// 是否禁止服务合并
        /// </summary>
        public bool IsDisAllowCommands
        {
            get
            {
                if (Attributes != null)
                {
                    return this.Attributes.Contains("System.DisAllowCommandsAttribute");
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ServcieName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RelativePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Request { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Response { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ServiceAttribute> Requests { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public List<ServiceAttribute> Responses { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public List<string> Exceptions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SearchServiceResponse()
        {
            this.Requests = new List<ServiceAttribute>();
            this.Responses = new List<ServiceAttribute>();
            this.Exceptions = new List<string>();
        }
    }
}
