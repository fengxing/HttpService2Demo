using System;

namespace ApiTranRequest
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateUserItem
    {
        /// <summary>
        /// 创建人编号
        /// </summary>
        public Guid CreateUserID { get; set; }

        /// <summary>
        /// 创建人
        /// <value>maxlength:200</value>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
