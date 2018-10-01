using RedisHelp;
using System;

namespace SmartHttp
{
    public class Redis
    {
        private static RedisHelper _db = null;

        public static RedisHelper GetHelper()
        {
            try
            {
                if (_db == null)
                {
                    int db = 0;
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["RedisDb"]))
                        {
                            db = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["RedisDb"]);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    _db = new RedisHelper(db);
                }
                return _db;
            }
            catch
            {
                return null;
            }
        }
    }
}
