using System;

namespace ApiTranRequest
{
    /// <summary>
    /// 更新人
    /// </summary>
    public class UpdateUserItem
    {
        /// <summary>
        /// 更新人编号
        /// </summary>
        public Guid UpdateUserID { get; set; }

        /// <summary>
        /// 更新人人
        /// <value>maxlength:200</value>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
