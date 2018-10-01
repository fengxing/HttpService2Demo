using System.Collections.Generic;
using System.Linq;

namespace SmartHttp
{
    public class HttpRequests
    {
        /// <summary>
        /// 应用号
        /// </summary>
        public int AppID { get; set; }

        public List<HttpCommand> HttpCommands { get; set; }

        public bool IsGet()
        {
            var isGet = true;
            foreach (var item in HttpCommands)
            {
                if (!item.Method.Contains("Get"))
                {
                    isGet = false;
                }
            }
            return isGet;
        }
    }

    public class HttpCommand
    {
        /// <summary>
        /// 应用号
        /// </summary>
        public int AppID { get; set; }

        public string Method { get; set; }

        public List<string> RequestObjs { get; set; }
    }

    public class HttpRequestsResponse
    {
        public List<HttpRequestResponse> HttpRequestResponses { get; set; }

        public HttpRequestsResponse()
        {
            this.HttpRequestResponses = new List<HttpRequestResponse>();
        }
    }

    public class HttpRequestResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Json { get; set; }
    }
}
