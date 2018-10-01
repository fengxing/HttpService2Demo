using SmartSDKHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class JsonConvert : IJsonConvert
    {
        public T DeserializeObject<T>(string value)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            }
            catch { }
            return default(T);
        }

        public string SerializeObject(object value)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }
            catch { }
            return "";
        }
    }
}
