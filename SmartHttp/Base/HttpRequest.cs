using System.Collections.Generic;

namespace SmartHttp
{
    public class HttpRequest
    {
        public int AppID { get; set; }

        public string Method { get; set; }

        public List<string> RequestObjs { get; set; }
    }
}