using DapperHelper;

namespace SmartAuthBLL
{
    public class HttpLogDbFactory
    {
        private static DapperDb instance;

        private static object _lock = new object();
        private HttpLogDbFactory()
        {

        }

        public static DapperDb Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new DapperDb("SmartHttpLog");
                        }
                    }
                }
                return instance;
            }
        }
    }
}
