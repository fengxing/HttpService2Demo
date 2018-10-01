using SmartSDKHelper;

namespace System
{
    /// <summary>
    /// 静态扩展类(partial)
    /// </summary>
    public static partial class ExtendHelper
    {
        /// <summary>
        /// 如果执行失败，则抛出异常
        /// </summary>
        /// <param name="result"></param>
        public static Result ThrowIfFailed(this Result result)
        {
            if (!result.IsSuccess)
            {
                throw new Exception(result.HttpCode + "|" + result.Data);
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 如果执行失败，则抛出异常（过滤异常）
        /// </summary>
        /// <param name="result"></param>
        /// <param name="filter">异常过滤</param>
        /// <returns></returns>
        public static Result ThrowIfFailed(this Result result, Func<Result, string> filter)
        {
            if (!result.IsSuccess)
            {
                if (filter != null)
                {
                    var err = filter(result);
                    throw new Exception(result.HttpCode + "|" + err);
                }
                else
                {
                    throw new Exception(result.HttpCode + "|" + result.Data);
                }
            }
            else
            {
                return result;
            }
        }
    }
}