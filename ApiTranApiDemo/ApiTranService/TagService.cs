using SmartBaseEntity;
using System;
using System.Linq;
using System.Web;

namespace ApiTranService
{
    public class TagService : ITagService, ICommitUser
    {
        public static int AppID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AppID"]);

        public Guid UID
        {
            get
            {
                return GetGuidTag("uid");
            }
        }

        public string RequestID
        {
            get
            {
                return GetTag("requestid");
            }
        }

        public IHttpTag GetTag()
        {
            var appid = GetIntTag("appid");
            var httpTag = new HttpTag()
            {
                SenderTime = GetDateTimeTag("sendertime"),
                Token = GetTag("token"),
                CIP = GetTag("cip"),
                UID = GetGuidTag("uid"),
                Language = GetTag("other"),
                IP = GetTag("sip"),
                ExecuteID = GetGuidTag("executeid"),
                RequestID = GetTag("requestid"),
                AppID = appid.HasValue ? appid.Value : AppID
            };
            if (httpTag.ExecuteID.HasValue)
            {
                httpTag.Method = GetTag("method");
            }
            return httpTag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private bool HasTag(string tagName)
        {
            try
            {
                if (HttpContext.Current.Request.Headers.AllKeys != null &&
                    HttpContext.Current.Request.Headers.AllKeys.Contains(tagName))
                {
                    return true;
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private DateTime GetDateTimeTag(string tagName)
        {
            try
            {
                if (HasTag(tagName))
                {
                    return Convert.ToDateTime(HttpContext.Current.Request.Headers.GetValues(tagName).FirstOrDefault());
                }
            }
            catch { }
            return DateTime.MaxValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private string GetTag(string tagName)
        {
            try
            {
                if (HasTag(tagName))
                {
                    return HttpContext.Current.Request.Headers.GetValues(tagName).FirstOrDefault();
                }
            }
            catch { }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private Guid GetGuidTag(string tagName)
        {
            try
            {
                if (HasTag(tagName))
                {
                    var tag = HttpContext.Current.Request.Headers.GetValues(tagName).FirstOrDefault();
                    return new Guid(tag);
                }
            }
            catch { }
            return Guid.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private int? GetIntTag(string tagName)
        {
            try
            {
                if (HasTag(tagName))
                {
                    var tag = HttpContext.Current.Request.Headers.GetValues(tagName).FirstOrDefault();
                    return Convert.ToInt32(tag);
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CommitUser GetCommitUser()
        {
            var user = new CommitUser()
            {
                ExecuteID = GetGuidTag("executeid"),
                UserID = this.UID,
                Method = GetTag("method"),
            };
            return user;
        }
    }
}
