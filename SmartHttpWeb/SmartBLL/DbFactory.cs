using DapperHelper;

namespace SmartBLL
{
    public class DbFactory
    {
        private static DapperDb instance;
        private static DapperDb instance1;
        private static object _lock = new object();
        private static object _lock1 = new object();

        private DbFactory()
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


        public static DapperDb LogInstance
        {
            get
            {
                if (instance1 == null)
                {
                    lock (_lock1)
                    {
                        if (instance1 == null)
                        {
                            instance1 = new DapperDb("SmartHttpLog");
                        }
                    }
                }
                return instance1;
            }
        }
    }
}
