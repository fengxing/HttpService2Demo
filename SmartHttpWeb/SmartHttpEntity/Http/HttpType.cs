using System.ComponentModel;

namespace SmartHttpEntity
{
    /// <summary>
    /// 三方接口类型
    /// </summary>
    public enum HttpType
    {
        /// <summary>
        /// WebService
        /// </summary>
        [Description("WebService")]
        WebService = 0,

        /// <summary>
        /// HttpGet
        /// </summary>
        [Description("HttpGet")]
        HttpGet = 1,

        /// <summary>
        /// HttpPost
        /// </summary>
        [Description("HttpPost")]
        HttpPost = 2,

        /// <summary>
        /// HttpPut
        /// </summary>
        [Description("HttpPut")]
        HttpPut = 3
    }
}
