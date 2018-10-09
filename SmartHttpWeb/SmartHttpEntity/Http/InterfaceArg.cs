namespace SmartHttpEntity
{
    /// <summary>
    /// 接口参数
    /// </summary>
    public class InterfaceArg
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsAllowNull { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MaxLength { get; set; }
    }
}
