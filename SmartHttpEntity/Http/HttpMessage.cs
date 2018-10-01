using SmartBaseEntity;
using System.Collections.Generic;

namespace SmartHttpEntity
{
    /// <summary>
    /// Http的消息实体
    /// </summary>
    public class HttpMessage : BaseEntity
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 接口方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// AppID
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public string Moudle { get; set; }

        /// <summary>
        /// 子模块
        /// </summary>
        public string SubMoudle { get; set; }

        /// <summary>
        /// 第三方接口类型
        /// </summary>
        public HttpType HttpType { get; set; }

        /// <summary>
        /// 结果路径
        /// </summary>
        public string ResultPath { get; set; }

        /// <summary>
        /// 正确结果
        /// </summary>
        public string TrueResult { get; set; }


        /// <summary>
        /// 正确结果包含判断
        /// </summary>
        public string TrueResultContain { get; set; }

        /// <summary>
        /// 错误结果包含判断
        /// </summary>
        public string FalseResultContain { get; set; }

        /// <summary>
        /// 异常路径
        /// </summary>
        public string ExceptionPath { get; set; }

        /// <summary>
        /// WebService内容模板
        /// </summary>
        public string WebServiceTemplate { get; set; }

        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///  Http UserAgent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 自定义头
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// Http协议编码
        /// </summary>
        public string HttpEncoding { get; set; }

        /// <summary>
        /// 接口参数JSON定义
        /// </summary>
        public string InterfaceArgsJsonString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int InterfaceArgsCount { get; set; }

        /// <summary>
        /// 是否需要登录
        /// </summary>
        public bool IsNeedLogin { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void SetInterfaceArgs(List<InterfaceArg> args)
        {
            if (args != null & args.Count > 0)
            {
                this.InterfaceArgsJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(args);
                this.InterfaceArgsCount = args.Count;
            }
            else
            {
                this.InterfaceArgsCount = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<InterfaceArg> GetInterfaceArgs()
        {
            if (!string.IsNullOrWhiteSpace(this.InterfaceArgsJsonString))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<InterfaceArg>>(this.InterfaceArgsJsonString);
            }
            return new List<InterfaceArg>();
        }



        /// <summary>
        /// 接口异常返回定义
        /// </summary>
        public string WsExcepitonsJsonString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void SetWsExcepitons(List<WsExcepiton> args)
        {
            if (args != null & args.Count > 0)
            {
                this.WsExcepitonsJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<WsExcepiton> GetWsExcepitons()
        {
            if (!string.IsNullOrWhiteSpace(this.WsExcepitonsJsonString))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<WsExcepiton>>(this.WsExcepitonsJsonString);

            }
            return new List<WsExcepiton>();
        }



        /// <summary>
        /// 异常返回值判断方式
        /// </summary>
        public string WSExceptionType { get; set; }

        /// <summary>
        /// 接口描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// 是否使用验证 替换为[是否异常映射]
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 超时时间(秒)
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        /// 失败尝试次数
        /// </summary>
        public int LoopTime { get; set; }

        /// <summary>
        /// 在循环等待时间(秒)
        /// </summary>
        public int LoopWaitTime { get; set; }

        /// <summary>
        /// 全局报文加解密
        /// </summary>
        public string EncryptDLLName { get; set; }

        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool IsCache { get; set; }

        /// <summary>
        /// 缓存时间(秒)
        /// 0永久缓存
        /// -1不缓存 开始但是不缓存  用于模拟数据
        /// </summary>
        public int CacheSeconds { get; set; }

        /// <summary>
        /// 是否关闭日志
        /// </summary>
        public bool IsLog { get; set; }

        /// <summary>
        /// 接口报文定义
        /// </summary>
        public string Define { get; set; }

        /// <summary>
        /// 是否通知
        /// <value>true:通知</value>
        /// <value>false:不通知</value>
        /// </summary>
        public bool IsNotify { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpMessage()
        {
            this.Version = "1.0";
            this.Method = "";
            this.AppName = "";
            this.Url = "";
            this.Moudle = "";
            this.ResultPath = "";
            this.TrueResult = "";
            this.TrueResultContain = "";
            this.FalseResultContain = "";
            this.ExceptionPath = "";
            this.WebServiceTemplate = "";
            this.ContentType = "";
            this.UserAgent = "";
            this.Headers = "";
            this.HttpEncoding = "";
            this.InterfaceArgsJsonString = "";
            this.WsExcepitonsJsonString = "";
            this.WSExceptionType = "";
            this.Description = "";
            this.EncryptDLLName = "";
            this.Define = "";
        }
    }
}