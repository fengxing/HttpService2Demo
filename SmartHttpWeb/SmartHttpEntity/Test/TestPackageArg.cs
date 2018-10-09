using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHttpEntity
{
    /// <summary>
    /// 接口参数
    /// </summary>
    public class TestPackageArg
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
    }
}
