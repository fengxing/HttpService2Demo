using System.Collections.Generic;

namespace SmartHttpEntity
{
    /// <summary>
    /// 测试包
    /// </summary>
    public class TestPackage : SmartBaseEntity.BaseEntity
    {
        /// <summary>
        /// 包类型
        /// 0 业务包
        /// 1 数据包
        /// 2 业务包
        /// </summary>
        public int PackageType { get; set; }

        /// <summary>
        /// 归属应用
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 包名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 接口参数
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// 输入表达式参数
        /// </summary>
        public string Json1 { get; set; }

        /// <summary>
        /// 输出表达式参数
        /// </summary>
        public string Json2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TestPackageArg> GetTestCaseArgs()
        {
            if (!string.IsNullOrWhiteSpace(this.Json))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<TestPackageArg>>(this.Json);
            }
            return new List<TestPackageArg>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TestInputArg> GetInputArgs()
        {
            if (!string.IsNullOrWhiteSpace(this.Json1))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<TestInputArg>>(this.Json1);
            }
            return new List<TestInputArg>();
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<OutputArg> GetOutputAgs()
        {
            if (!string.IsNullOrWhiteSpace(this.Json2))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<OutputArg>>(this.Json2);
            }
            return new List<OutputArg>();
        }
    }
}
