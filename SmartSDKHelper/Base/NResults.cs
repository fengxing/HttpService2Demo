namespace SmartSDKHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class NResults
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public Result Result { get; set; }

        /// <summary>
        /// 请求信息
        /// </summary>
        public HttpInvokes HttpInvokes { get; set; }


        /// <summary>
        /// 是否本地请求超时
        /// </summary>
        public bool IsTimeOut
        {
            get
            {
                return this.Result.Wrong == Wrong.RequestTimeOut;
            }
        }

        /// <summary>
        /// 是否登录超时
        /// </summary>
        public bool IsLoginTimeOut
        {
            get
            {
                return this.Result.Wrong == Wrong.LoginTimeOut;
            }
        }
    }
}
