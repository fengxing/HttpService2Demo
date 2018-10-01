using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartHttpService
{
    public class IP
    {
        public static string GetHostAddress()
        {
            var file = new System.IO.StreamWriter("d:\\111.txt");

            foreach (var key in HttpContext.Current.Request.ServerVariables.AllKeys)
            {
                file.WriteLine(key + "            " + HttpContext.Current.Request.ServerVariables[key]);
            }
            file.Flush();
            file.Close();
            string userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(userHostAddress))
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                    userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
            }
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}