using SmartBaseEntity;
using SmartHttpEntity;
using System.Collections.Generic;
using System.Linq;

namespace SmartHttpEntity
{
    /// <summary>
    /// 测试用例
    /// </summary>
    public class TestCase : BaseEntity
    {
        /// <summary>
        /// 应用号
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 接口版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 接口方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 用例名称
        /// </summary>
        public string CaseName { get; set; }

        /// <summary>
        /// 是否验证结果
        /// 为空 ：接口执行
        /// true :验证结果成功
        /// false:验证结果失败
        /// </summary>
        public bool? ValidResult { get; set; }

        /// <summary>
        /// 结果路径
        /// </summary>
        public string ResultPath { get; set; }

        /// <summary>
        /// equals 相等
        /// contains 包含
        /// greater 大于
        /// less    小于
        /// </summary>
        public string ValidType { get; set; }

        /// <summary>
        /// 结果值
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// 接口参数
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// 临时变量
        /// </summary>
        public string Json1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TestCaseArg> GetArgs()
        {
            if (!string.IsNullOrWhiteSpace(this.Json))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<TestCaseArg>>(this.Json);
            }
            return new List<TestCaseArg>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TempArg> GetTempArgs()
        {
            if (!string.IsNullOrWhiteSpace(this.Json1))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempArg>>(this.Json1);
            }
            return new List<TempArg>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetRequestObjs()
        {
            var args = GetArgs();
            return args.Select(r => r.Value).ToList();
        }

       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        public string Description()
        {
            var s = "";
            if (this.ValidResult.HasValue)
            {
                if (this.ValidResult.Value)
                {
                    s += "正确验证,结果" + GetValidTypeString() + this.Result;
                }
                else
                {
                    s += "错误验证,结果" + GetValidTypeString() + this.Result;
                }
            }
            else
            {
                s += "接口执行";
            }
            return s;
        }

        private string GetValidTypeString()
        {
            if (this.ValidType == "equals")
            {
                return "等于";
            }
            else if (this.ValidType == "contains")
            {
                return "包含";
            }
            else if (this.ValidType == "greater")
            {
                return "大于";
            }
            else if (this.ValidType == "less")
            {
                return "小于";
            }
            return "";
        }
    }
}