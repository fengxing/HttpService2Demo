namespace SmartSDKHelper
{
    /// <summary>
    /// 登录接口
    /// </summary>
    public interface ILogin
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        LoginToken Login(int appID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appID"></param>
        /// <returns></returns>
        bool NeedLogin(int appID);

        /// <summary>
        /// 是否使用本地的Token
        /// </summary>
        /// <param name="appID"></param>
        /// <returns></returns>
        bool IsLocalToken(int appID);

        /// <summary>
        /// UID
        /// </summary>
        /// <returns></returns>
        string UID { get; }

        /// <summary>
        /// Token
        /// </summary>
        string Token { get;  }
    }
}
