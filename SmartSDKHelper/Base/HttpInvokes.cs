using System;
using System.Collections.Generic;

namespace SmartSDKHelper
{
    /// <summary>
    /// 多命令执行
    /// </summary>
    public class HttpInvokes
    {
        /// <summary>
        /// 执行编号
        /// </summary>
        public Guid? ExexuteID { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// AppID
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 命令列表
        /// </summary>
        public List<HttpCommand> HttpCommands { get; set; }

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
    }

    /// <summary>
    /// 命令
    /// </summary>
    public class HttpCommand
    {
        /// <summary>
        /// 应用编号
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 请求对象
        /// </summary>
        public List<string> RequestObjs { get; set; }
    }
}
