using SmartHttpEntity;
using System.Diagnostics;

namespace SmartHttp
{
    /// <summary>
    /// 请求信息
    /// </summary>
    public class RequestInfo
    {
        public HttpRequest HttpRequest { get; set; }

        public HttpTag HttpTag { get; set; }

        public HttpMessage HttpMessage { get; set; }

        public Stopwatch Stopwatch { get; set; }

        public HttpReturn HttpReturn { get; set; }

        public RequestInfo()
        {
            this.Stopwatch = new Stopwatch();
        }

        public void Start()
        {
            this.Stopwatch.Start();
        }

        public void Stop()
        {
            this.Stopwatch.Stop();
        }
    }
}
