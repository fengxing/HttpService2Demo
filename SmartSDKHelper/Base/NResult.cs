namespace SmartSDKHelper
{
    /// <summary>
    /// 强类型返回结果
    /// </summary>
    public class NResult
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public Result Result { get; set; }

        /// <summary>
        /// 请求信息
        /// </summary>
        public HttpInvoke HttpInvoke { get; set; }
    }
}
