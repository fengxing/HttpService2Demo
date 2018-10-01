using DapperHelper;

namespace SmartAuthBLL
{
    public class HttpDbFactory
    {
        private static DapperDb instance;
        private static object _lock = new object();

        private HttpDbFactory()
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
                            instance = new DapperDb("SmartHttp");
                        }
                    }
                }
                return instance;
            }
        }
    }
}
