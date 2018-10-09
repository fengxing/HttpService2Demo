using SmartBaseEntity;

namespace SmartHttpEntity
{
    /// <summary>
    /// 心跳配置
    /// </summary>
    public class Heart : BaseEntity
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 总线地址
        /// </summary>
        public string Url { get; set; }
    }
}
