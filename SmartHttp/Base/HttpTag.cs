using SmartHttp.Base;
using System;
using System.Collections.Generic;

namespace SmartHttp
{
    public class HttpTag
    {
        /// <summary>
        /// 渠道
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 请求在客户端发出时间
        /// </summary>
        public string SenderTime { get; set; }

        /// <summary>
        /// 是否异步
        /// </summary>
        public bool IsAynsc { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string CIP { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string RowVersion { get; set; }

        /// <summary>
        /// 其他的扩展
        /// </summary>
        public string Other { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 执行ID
        /// </summary>
        public Guid? ExecuteID { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 是否强制访问业务
        /// 不使用缓存
        /// </summary>
        public bool Retry { get; set; }

        /// <summary>
        /// 是否预加载
        /// </summary>
        public bool IsPreLoad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RequestID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ComputerName { get; set; }

        public HttpTag()
        {
            this.Channel = "";
            this.Version = "";
            this.Token = "";
            this.SenderTime = "";
            this.IsAynsc = false;
            this.CIP = "";
            this.UID = "";
            this.RowVersion = "";
            this.Other = "";
            this.IP = "";
            this.Browser = "";
            this.Retry = false;
            this.IsPreLoad = false;
        }

        public string GetKey(int appID)
        {
            var mapperAppID = AppMapperConfig.GetMapperAppID(appID);
            return (mapperAppID + "Token" + this.UID).ToLower();
        }
    }
}