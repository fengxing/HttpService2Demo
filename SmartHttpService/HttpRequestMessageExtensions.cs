using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace SmartHttpService
{
    public static class HttpRequestMessageExtensions
    {
        private const string HttpContext = "MS_HttpContext";

        public static string GetClientIpString(this HttpRequestMessage request)
        {
            try
            {
                if (request.Properties.ContainsKey(HttpContext))
                {
                    var ctx = request.Properties[HttpContext];
                    if (ctx != null)
                    {
                        var http = (System.Web.HttpContextWrapper)ctx;
                        if (http != null)
                        {
                            return http.Request.UserHostAddress;
                        }
                    }
                }
                if (System.Web.HttpContext.Current != null)
                {
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static IPAddress GetClientIpAddress(this HttpRequestMessage request)
        {
            var ipString = request.GetClientIpString();
            IPAddress ipAddress = new IPAddress(0);
            if (IPAddress.TryParse(ipString, out ipAddress))
            {
                return ipAddress;
            }
            return ipAddress;
        }
    }
}