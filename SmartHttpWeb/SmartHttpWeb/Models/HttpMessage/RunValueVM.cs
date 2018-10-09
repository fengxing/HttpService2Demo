using System;
using System.Collections.Generic;

namespace SmartHttpWeb.Models
{
    public class RunValueVM
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }

        public List<string> NameArgs { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public List<string> ValueArgs { get; set; }
    }
}