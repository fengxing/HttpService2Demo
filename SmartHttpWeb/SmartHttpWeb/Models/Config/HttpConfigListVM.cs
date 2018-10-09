using SmartHttpEntity;
using System.Collections.Generic;

namespace SmartHttpWeb.Models
{
    public class HttpConfigListVM
    {
        public List<HttpConfig> HttpConfigs { get; set; }

        public HttpConfigListVM()
        {
            this.HttpConfigs = new List<HttpConfig>();
        }
    }
}