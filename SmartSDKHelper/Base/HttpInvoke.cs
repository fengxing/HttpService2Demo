using System;
using System.Collections.Generic;

namespace SmartSDKHelper
{
    /// <summary>
    /// Http异步请求命令
    /// </summary>
    public class HttpInvoke
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 请求对象 禁止为空
        /// </summary>
        public List<string> RequestObjs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid ExecuteID { get; set; }

        /// <summary>
        /// 应该ID
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 版本号 
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SenderTime { get; set; }

        /// <summary>
        /// 是否可以重试
        /// </summary>
        public bool CanRetry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RetryTimes { get; set; }       

        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpInvoke()
        {
            this.ID = "";
            this.RequestObjs = new List<string>();
        }
    }
}
