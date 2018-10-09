using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SmartHttpWeb
{
    public static partial class ExtendHelper
    {

        #region 获取枚举Description内容
        /// <summary>
        /// 获取枚举的Description属性
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Description(this System.Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(description);
            if (fieldInfo == null)
            {
                return "";
            }
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            else
            {
                description = value.ToString();//如果没有设置Description返回英文
            }
            return description;
        }
        #endregion

        /// <summary>
        /// 格式化json输出
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static string FormatJson(this string jsonString)
        {
            try
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                StringReader sr = new StringReader(jsonString);
                object obj = serializer.Deserialize(new Newtonsoft.Json.JsonTextReader(sr));

                if (obj != null)
                {
                    StringWriter sw = new StringWriter();
                    Newtonsoft.Json.JsonWriter jsonWriter = new Newtonsoft.Json.JsonTextWriter(sw)
                    {
                        Formatting = Newtonsoft.Json.Formatting.Indented,
                        Indentation = 4,
                        IndentChar = ' '
                    };
                    serializer.Serialize(jsonWriter, obj);
                    return sw.ToString();
                }
                else
                {
                    return jsonString;
                }
            }
            catch
            {
                return jsonString;
            }
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToString(this DateTime? time, string format)
        {
            if (time.HasValue)
            {
                return time.Value.ToString(format);
            }
            return "";
        }


        public static string GetJsonExpression(this string json, string path)
        {
            var oldPath = path;
            var compare = json;
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (path.StartsWith("$"))
                {
                    path = path.Substring(1, path.Length - 1);
                }
                if (path.EndsWith(".Length"))
                {
                    path = path.Replace(".Length", "");
                    var token = JToken.Parse(compare);
                    var count = token.SelectToken(path).ThrowIfNull("表达式不正确,表达式:" + oldPath).Children().Count();
                    compare = count.ToString();
                }
                else
                {
                    var token = JToken.Parse(compare);
                    var p = token.SelectToken(path).ThrowIfNull("表达式不正确,表达式:" + oldPath);
                    compare = p.ToString();
                }
            }
            return compare;
        }


    }
}