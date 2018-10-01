namespace SmartHttpEntity
{
    /// <summary>
    /// 默认触发参数
    /// </summary>
    public class HttpInvokeArg : SmartBaseEntity.BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }
    }
}
