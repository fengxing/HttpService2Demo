using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartHttpService.Models
{
    public class SaveTokenRequest
    {
        /// <summary>
        /// AppID
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
    }
}