using System;
using System.Collections.Generic;

namespace SmartHttpEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 是否可空类型
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 值的描述
        /// </summary>
        public string[] ValueDescriptions { get; set; }




        /// <summary>
        /// 最大长度
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// 集合中的类描述
        /// </summary>
        public List<ServiceAttribute> ClassDescriptions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ServiceAttribute> GetInnerServices()
        {
            var all = new List<ServiceAttribute>();
            foreach (var item in ClassDescriptions)
            {
                if (item.ClassDescriptions != null &&
                    item.ClassDescriptions.Count > 0)
                {
                    all.Add(item);
                }
            }
            return all;
        }


        /// <summary>
        /// 值的描述
        /// </summary>
        public Dictionary<string, string> GetValueDescriptions()
        {
            var d = new Dictionary<string, string>();
            if (this.ValueDescriptions != null)
            {
                foreach (var item in ValueDescriptions)
                {
                    var arr = item.Split(new char[] { ':', ',', '、', '：' }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length == 2)
                    {
                        d.Add(arr[0], arr[1]);
                    }
                    else
                    {
                        d.Add(item, item);
                    }
                }
            }
            return d;
        }
    }
}
