using SmartHttpEntity;
using System;
using System.Collections.Generic;

namespace SmartHttpWeb.Models
{

    public class HttpMessageAddUpdateVM
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 
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
        /// 接口模块
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
        /// 加密模板
        /// </summary>
        public string EncryptTemplate { get; set; }

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
        /// 短信第三方接口参数
        /// </summary>
        public List<InterfaceArg> InterfaceArgs { get; set; }

        /// <summary>
        /// 短信第三方接口异常返回参数
        /// </summary>
        public List<WsExcepiton> WsExcepitons { get; set; }

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
        /// 是否使用验证
        /// </summary>
        public int IsValid { get; set; }

        /// <summary>
        /// 名字参数
        /// </summary>
        public List<string> NameArgs { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public List<string> TypeArgs { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        public List<string> DescriptionArgs { get; set; }

        /// <summary>
        /// 是否为空
        /// </summary>
        public List<string> IsAllowNullArgs { get; set; }

        /// <summary>
        /// 异常代码
        /// </summary>
        public List<string> Codes { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// 超时时间(秒)
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        /// 循环次数
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
        public int IsCache { get; set; }

        /// <summary>
        /// 缓存时间(秒)
        /// </summary>
        public int CacheSeconds { get; set; }

        public bool IsLog { get; set; }

        public bool IsNotify { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }


        /// <summary>
        /// 行版本
        /// </summary>
        public int RowVersion { get; set; }


        public HttpMessageAddUpdateVM()
        {
            this.NameArgs = new List<string>();
            this.TypeArgs = new List<string>();
            this.Codes = new List<string>();
            this.Messages = new List<string>();
            this.DescriptionArgs = new List<string>();
        }

        public HttpMessage ToEntity()
        {
            var entity = new HttpMessage()
            {
                Url = this.Url.ToTrim(),
                Status = this.Status,
                Description = this.Description.ToTrim(),
                Method = this.Method.ToTrim(),
                AppID = this.AppID,
                Moudle = this.Moudle.ToTrim(),
                SubMoudle = this.SubMoudle.Trim(),
                ContentType = string.IsNullOrWhiteSpace(this.ContentType) ? "" : this.ContentType,
                HttpType = this.HttpType,
                WebServiceTemplate = string.IsNullOrWhiteSpace(this.WebServiceTemplate) ? "" : this.WebServiceTemplate,
                ResultPath = string.IsNullOrWhiteSpace(this.ResultPath) ? "" : this.ResultPath,
                ExceptionPath = string.IsNullOrWhiteSpace(this.ExceptionPath) ? "" : this.ExceptionPath,
                TrueResult = string.IsNullOrWhiteSpace(this.TrueResult) ? "" : this.TrueResult,
                TrueResultContain = string.IsNullOrWhiteSpace(this.TrueResultContain) ? "" : this.TrueResultContain,
                Headers = string.IsNullOrWhiteSpace(this.Headers) ? "" : this.Headers,
                HttpEncoding = string.IsNullOrWhiteSpace(this.HttpEncoding) ? "" : this.HttpEncoding,
                UserAgent = string.IsNullOrWhiteSpace(this.UserAgent) ? "" : this.UserAgent,
                WSExceptionType = string.IsNullOrWhiteSpace(this.WSExceptionType) ? "" : this.WSExceptionType,
                IsValid = this.IsValid == 1 ? true : false,
                FalseResultContain = string.IsNullOrWhiteSpace(this.FalseResultContain) ? "" : this.FalseResultContain,
                EncryptDLLName = string.IsNullOrWhiteSpace(this.EncryptDLLName) ? "" : this.EncryptDLLName,
                LoopTime = this.LoopTime,
                LoopWaitTime = this.LoopWaitTime,
                TimeOut = this.TimeOut,
                IsCache = this.IsCache == 1 ? true : false,
                CacheSeconds = this.CacheSeconds,
                IsLog = this.IsLog,
                IsNotify = this.IsNotify,
                AppName = this.AppName.ToTrim(),
                Version = string.IsNullOrWhiteSpace(this.Version) ? "1.0" : this.Version,
            };
            if (!string.IsNullOrWhiteSpace(this.ID))
            {
                entity.SetID(Guid.Parse(this.ID));
                entity.RowVersion = this.RowVersion;
            }
            var interfaceArgs = new List<InterfaceArg>();
            for (int i = 0; i < NameArgs.Count; i++)
            {
                interfaceArgs.Add(new InterfaceArg()
                {
                    Name = this.NameArgs[i].ToTrim(),
                    Type = this.TypeArgs[i].ToTrim(),
                    Description = this.DescriptionArgs[i],
                    IsAllowNull = this.IsAllowNullArgs == null ? false : (this.IsAllowNullArgs[i] == "" ? false : Convert.ToBoolean(this.IsAllowNullArgs[i]))
                });
            }
            entity.SetInterfaceArgs(interfaceArgs);

            var wsExcepitons = new List<WsExcepiton>();
            for (int i = 0; i < Codes.Count; i++)
            {
                wsExcepitons.Add(new WsExcepiton()
                {
                    Name = this.Codes[i].ToTrim(),
                    Value = this.Messages[i].ToTrim()
                });
            }
            entity.SetWsExcepitons(wsExcepitons);

            if (entity.IsCache)
            {
                if (entity.CacheSeconds == 0)
                {
                    throw new BException("禁止永久缓存");
                }
            }
            return entity;
        }
    }
}