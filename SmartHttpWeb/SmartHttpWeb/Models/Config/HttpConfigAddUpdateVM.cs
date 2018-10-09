using System;

namespace SmartHttpWeb.Models.Config
{
    public class HttpConfigAddUpdateVM
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 键值
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 测试值
        /// </summary>
        public string TestValue { get; set; }

        /// <summary>
        /// 正式值
        /// </summary>
        public string ProdcutValue { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}