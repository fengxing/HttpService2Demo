using SmartBaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SmartHttpEntity
{
    /// <summary>
    /// 应用程序配置
    /// </summary>
    public class HttpApp : BaseEntity
    {
        /// <summary>
        /// 程序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public int AppID { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 模块JSON数据
        /// </summary>
        public string MoudlesJsonString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetMoudles()
        {
            try
            {
                var allMoudles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(this.MoudlesJsonString);
                for (int i = 0; i < allMoudles.Count; i++)
                {
                    var moudle = allMoudles[i];
                    if (moudle.Contains("["))
                    {
                        var start = moudle.IndexOf("[") + 1;
                        var subMoudle = moudle.Substring(start).TrimEnd(']');
                        var arr = subMoudle.Split(',');
                        allMoudles[i] = allMoudles[i].Substring(0, start - 1);
                    }
                }
                return allMoudles;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moudle"></param>
        /// <returns></returns>
        public List<string> GetSubMoudles(string moudle)
        {
            try
            {
                var allMoudles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(this.MoudlesJsonString);
                var subMoudle = allMoudles.FirstOrDefault(r => r.StartsWith(moudle + "["));
                var start = subMoudle.IndexOf("[") + 1;
                return subMoudle.Substring(start).TrimEnd(']').Split(',').ToList();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void SetMoudles(List<string> args)
        {
            this.MoudlesJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(args);
        }

    }
}
