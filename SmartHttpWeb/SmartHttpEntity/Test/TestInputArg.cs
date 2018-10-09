using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHttpEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class TestInputArg
    {
        /// <summary>
        /// 
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 接口方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 用例名称
        /// </summary>
        public string CaseName { get; set; }

        /// <summary>
        /// 表达式($开头)/值
        /// $开头
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 参数序号
        /// </summary>
        public int ArgIndex { get; set; }
    }
}
