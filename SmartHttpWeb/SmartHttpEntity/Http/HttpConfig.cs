
using SmartBaseEntity;
using System;

namespace SmartHttpEntity
{
    /// <summary>
    /// 接口动态参数配置
    /// </summary>
    public class HttpConfig: BaseEntity
    { 
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
