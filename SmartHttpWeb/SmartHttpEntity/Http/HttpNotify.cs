using SmartBaseEntity;

namespace SmartHttpEntity
{
    /// <summary>
    /// 接口通知用户配置
    /// </summary>
    public class HttpNotify : SmartBaseEntity.BaseEntity, ISame
    {
        /// <summary>
        /// 通知的用户
        /// <value>maxlength:20000</value>
        /// </summary>
        public string NotifyUsers { get; set; }

        /// <summary>
        /// 方法
        /// <value>maxlength:128</value>
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 方法版本
        /// <value>maxlength:128</value>
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 方法应用编号
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 要通知的应用编号
        /// <value>maxlength:2000</value>
        /// </summary>
        public string AppIDs { get; set; }

        /// <summary>
        /// 通知的消息模板
        /// <value>maxlength:500</value>
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 通知的消息模板参数路径
        /// <value>maxlength:2000</value>
        /// </summary>
        public string MessagePaths { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetCols()
        {
            return nameof(AppID) + "," + nameof(Method);
        }
    }
}
