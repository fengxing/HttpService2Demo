using System;
using SmartSDKHelper;

namespace Test
{
    public class LoginProcess : ILogin
    {
        /// <summary>
        /// 
        /// </summary>
        public string Token
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UID
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appID"></param>
        /// <returns></returns>
        public bool IsLocalToken(int appID)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public LoginToken Login(int appID)
        {
            var loginToken = SmartSDKHelper.SmartOutSideHelper.Request<SmartSDKHelper.LoginToken>(new SmartSDKHelper.HttpInvoke()
            {
                AppID = 7,
                Method = "Login",
                RequestObjs = new System.Collections.Generic.List<string>() {
                        "3",
                        "202CB962AC59075B964B07152D234B70",
                        "admin"},
                SenderTime = DateTime.Now,
                Version = "1.0",
                CanRetry = true,
            }, (result) => { throw new Exception("登录失败"); });
            return loginToken;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appID"></param>
        /// <returns></returns>
        public bool NeedLogin(int appID)
        {
            return true;
        }
    }
}
