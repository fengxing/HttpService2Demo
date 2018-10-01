using System.Collections.Generic;

namespace SmartHttpService.Models
{
    /// <summary>
    /// Token请求
    /// </summary>
    public class GetTokenRequest
    {
        /// <summary>
        /// AppID
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// DLL清单
        /// </summary>
        public string DLLs { get; set; }

        /// <summary>
        /// 主机名称
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 主机IP
        /// </summary>
        public string HostIP { get; set; }

        public void Valid()
        {
            if (AppID <= 0)
            {
                throw new System.Exception("AppID不正确:" + AppID);
            }
        }
    }
}