using System;
using System.Collections.Concurrent;
using System.IO;

namespace SmartHttp.Base
{
    public class AppMapperConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public static ConcurrentDictionary<int, int> AppMappers = new ConcurrentDictionary<int, int>();

        public static void Load()
        {
            var lines = File.ReadAllLines(System.Configuration.ConfigurationManager.AppSettings["AppMapperPath"]);
            if (lines != null)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    var arr = lines[i].Split(',');
                    var appID = Convert.ToInt32(arr[0]);
                    var mapperAppID = Convert.ToInt32(arr[1]);
                    AppMappers.AddOrUpdate(appID, mapperAppID, (key, existingVal) =>
                    {
                        if (existingVal != mapperAppID)
                        {
                            return mapperAppID;
                        }
                        else
                        {
                            return existingVal;
                        }
                    });
                }
            }
        }

        public static int GetMapperAppID(int appID)
        {
            if (AppMappers.Count == 0)
            {
                Load();
            }
            if (AppMappers.ContainsKey(appID))
            {
                return AppMappers[appID];
            }
            return appID;
        }
    }
}
