using System;
using System.ComponentModel;

namespace SmartSDKHelper
{
    /// <summary>
    /// 接口返回结果
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 错误
        /// </summary>
        public Wrong Wrong { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int HttpCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result()
        {
            this.Wrong = Wrong.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!this.IsSuccess)
            {
                if (!string.IsNullOrWhiteSpace(this.Data))
                {
                    return string.Format("失败!异常类型:{0},原因:{1},状态码:{2}", ToDesc(this.Wrong), this.Data, this.HttpCode);
                }
                else
                {
                    return string.Format("失败!异常类型:{0},状态码:{1}", ToDesc(this.Wrong), this.HttpCode);
                }
            }
            return "";
        }



        /// <summary>
        /// 获取枚举的属性
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        private string ToDesc(Enum enumValue)
        {
            try
            {
                string str = enumValue.ToString();
                System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
                object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs == null || objs.Length == 0) return str;
                DescriptionAttribute da = (DescriptionAttribute)objs[0];
                return da.Description;
            }
            catch
            {
                return "";
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Wrong
    {
        /// <summary>
        /// 无异常
        /// </summary>
        [Description("无异常")]
        None = -1,

        /// <summary>
        /// 未知异常
        /// </summary>
        [Description("未知异常")]
        UnKnow = -999,

        /// <summary>
        /// 请求超时
        /// </summary>
        [Description("请求超时")]
        RequestTimeOut = -1000,

        /// <summary>
        /// 登录超时
        /// </summary>
        [Description("登录超时")]
        LoginTimeOut = 501,

        /// <summary>
        /// 未登录
        /// </summary>
        [Description("未登录")]
        UnLogin = 500,

        /// <summary>
        /// 业务异常
        /// </summary>
        [Description("业务异常")]
        BusinessError = 601,

        /// <summary>
        /// 服务不存在
        /// </summary>
        [Description("服务不存在")]
        ServiceNotFound = 600,

        /// <summary>
        /// 请求参数异常
        /// </summary>
        [Description("请求参数异常")]
        RequestArgError = 602,

        /// <summary>
        /// 网关异常
        /// </summary>
        [Description("网关异常")]
        GateWayError = 604,

        /// <summary>
        /// 网关配置异常
        /// </summary>
        [Description("网关配置异常")]
        GateWaySettingError = 700,

        /// <summary>
        /// 数据库异常
        /// </summary>
        [Description("数据库异常")]
        GateWayDatabaseError = 800,

        /// <summary>
        /// 数据库更新唯一键异常
        /// </summary>
        [Description("数据库更新唯一键异常")]
        GateWayDatabaseUniqueError = 801,

        /// <summary>
        /// 数据库并发更新异常
        /// </summary>
        [Description("数据库并发更新异常")]
        GateWayDatabaseConflictError = 802,

        /// <summary>
        /// 数据库实体长度异常
        /// </summary>
        [Description("数据库并发更新异常")]
        GateWayDatabaseEntityLengthError = 803,

        /// <summary>
        /// 数据库实体长度异常
        /// </summary>
        [Description("数据删除引用关系异常")]
        GateWayDatabaseIDRefError = 804,

        /// <summary>
        /// 网关缓存异常
        /// </summary>
        [Description("网关缓存异常")]
        GateWayRedisError = 850,

        /// <summary>
        /// 网关运行异常
        /// </summary>
        [Description("网关运行异常")]
        GateWayRuntimeError = 990,

        /// <summary>
        /// 网关系统异常
        /// </summary>
        [Description("网关系统异常")]
        GateWaySystemError = 999,

        /// <summary>
        /// Json序列化空异常
        /// </summary>
        [Description("Json序列化空异常")]
        JsonDeserializeNullError,
    }
}
