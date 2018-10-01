using System.ComponentModel;

namespace SmartHttpEntity
{
    /// <summary>
    /// 状态
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// 删除|-1
        /// </summary>
        [Description("删除")]
        Deleted = -1,

        /// <summary>
        /// 冻结|0
        /// </summary>
        [Description("冻结")]
        Freezed = 0,

        /// <summary>
        /// 正常|1
        /// </summary>
        [Description("正常")]
        Normal = 1

    }
}
