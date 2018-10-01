using SmartHttp;
using System;
using System.Linq;
using System.Web.Http;

namespace SmartHttpService.Controllers
{
    public class BaseController : ApiController
    {
        public HttpTag GetHttpTag()
        {
            var httpTag = new HttpTag()
            {
                Channel = GetTag("channel"),
                IsAynsc = HasTag("isAynsc"),
                SenderTime = GetTag("sendertime"),
                Token = GetTag("token"),
                Version = GetTag("hversion"),
                CIP = GetTag("cip"),
                UID = GetTag("uid"),
                RowVersion = GetTag("rowversion"),
                Other = GetTag("other"),
                Browser = GetBrowser(),
                IsPreLoad = HasTag("preload"),
                Retry = HasTag("retry"),
                RequestID = GetTag("requestid"),
                ComputerName = "",
                ExecuteID = GetGuidTag("executeid"),
            };
            return httpTag;
        }

        private string GetComputerName()
        {
            try
            {
                if (this.Request.Properties.ContainsKey("MS_HttpContext"))
                {
                    var content = this.Request.Properties["MS_HttpContext"];
                    var wapper = (System.Web.HttpContextWrapper)content;
                    if (wapper != null && wapper.Request != null)
                    {
                        return wapper.Request.UserHostName;
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        private string GetBrowser()
        {
            try
            {
                if (this.Request.Properties.ContainsKey("MS_HttpContext"))
                {
                    var content = this.Request.Properties["MS_HttpContext"];
                    var wapper = (System.Web.HttpContextWrapper)content;
                    if (wapper != null && wapper.Request != null)
                    {
                        return wapper.Request.UserAgent;
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        private Guid? GetGuidTag(string tagName)
        {
            try
            {
                if (this.Request.Headers.Contains(tagName))
                {
                    var guid = Guid.Empty;
                    var tag = this.Request.Headers.GetValues(tagName).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(tag) && Guid.TryParse(tag, out guid))
                    {
                        if (guid != Guid.Empty)
                        {
                            return guid;
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }


        private string GetTag(string tagName)
        {
            try
            {
                var tag = "";
                if (this.Request.Headers.Contains(tagName))
                {
                    try
                    {
                        tag = this.Request.Headers.GetValues(tagName).FirstOrDefault();
                    }
                    catch { }
                }
                return tag;
            }
            catch
            {
                return "";
            }
        }

        private bool HasTag(string tagName)
        {
            try
            {
                if (this.Request.Headers.Contains(tagName))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
