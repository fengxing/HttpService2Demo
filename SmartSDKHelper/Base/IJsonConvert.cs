namespace SmartSDKHelper
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJsonConvert
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string SerializeObject(object value);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        T DeserializeObject<T>(string value);
    }
}
