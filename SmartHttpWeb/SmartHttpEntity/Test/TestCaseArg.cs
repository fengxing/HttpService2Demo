namespace SmartHttpEntity
{
    /// <summary>
    /// 接口参数
    /// </summary>
    public class TestCaseArg
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
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsAllowNull { get; set; }
    }
}
