using System.Data;
using System.Data.SqlClient;

namespace SmartBLL
{
    public class DrapperHelper
    {
        public static IDbConnection GetSmartHttpOpenConnection()
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["SmartHttp"].ToString();
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }
    }
}
