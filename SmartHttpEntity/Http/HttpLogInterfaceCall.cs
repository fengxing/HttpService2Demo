using SmartBaseEntity;
using System;

namespace SmartHttpEntity
{
    /// <summary>
    /// 日志记录信息
    /// </summary>
    public class HttpLogInterfaceCall : BaseEntity
    {

        /// <summary>
        /// 调用耗时
        /// </summary>
        public long Mills { get; set; }

        /// <summary>
        /// 请求的地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求的消息
        /// </summary>
        public string Request { get; set; }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// 请求的接口
        /// </summary>
        public string Method { get; set; }


        /// <summary>
        /// 请求的方式
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        public string Exception { get; set; }


        /// <summary>
        /// 异常提示
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public long AppID { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string Moudle { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public string RequestArgs { get; set; }

        /// <summary>
        /// 待加解密信息
        /// </summary>
        public string ResponseEncrypt { get; set; }

        /// <summary>
        /// 待加解密信息
        /// </summary>
        public string RequestEncrypt { get; set; }


        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 异常类型
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// 请求来源
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }


        /// <summary>
        /// 异步发送时间
        /// </summary>
        public string SenderTime { get; set; }

        /// <summary>
        /// 是否异步操作
        /// </summary>
        public bool IsAynsc { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string CIP { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RequestID { get; set; }

        /// <summary>
        /// 其他的扩展
        /// </summary>
        public string Other { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }

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
        /// 计算机名称
        /// </summary>
        public string ComputerName { get; set; }

        /// <summary>
        /// 是否通知
        /// </summary>
        public bool IsNotify { get; set; }

        /// <summary>
        /// 是否处理
        /// </summary>
        public bool IsProcess { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid ExecuteID { get; set; }

        /// <summary>
        /// 是否执行成功
        /// <value>0:等待返回执行结果(超时20分钟算失败)</value>
        /// <value>1:执行成功</value>
        /// </summary>
        public int ExecuteResult { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpLogInterfaceCall()
        {
            this.RowVersion = 0;
            this.UpdateTime = DateTime.Now;
        }
    }
}
