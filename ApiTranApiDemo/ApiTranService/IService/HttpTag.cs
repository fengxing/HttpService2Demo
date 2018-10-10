using SmartBaseEntity;
using System;

namespace ApiTranService
{
    /// <summary>
    /// 访问基本信息
    /// </summary>
    public class HttpTag : IHttpTag
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 请求在客户端发出时间
        /// </summary>
        public DateTime SenderTime { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string CIP { get; set; }

        /// <summary>
        /// 服务端IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UID { get; set; }

        /// <summary>
        /// 版本号 暂时不用
        /// </summary>
        public string RowVersion { get; set; }

        /// <summary>
        /// 语言版本
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 执行标识
        /// </summary>
        public Guid? ExecuteID { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 执行顺序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RequestID { get; set; }

        public int? ExecuteHashCode { get; set; }

        /// <summary>
        /// 请求基本信息
        /// </summary>
        public HttpTag()
        {
            this.Token = "";
            this.SenderTime = DateTime.MaxValue;
            this.CIP = "";
            this.UID = Guid.Empty;
            this.RowVersion = "";
            this.IP = "";
            this.Method = "";
            this.AppID = 0;
            this.RequestID = "";
        }
    }

}
