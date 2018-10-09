using Dapper;
using SmartHttpEntity;
using System.Collections.Generic;
using System.Linq;

namespace SmartBLL
{
    public class HttpAppRep : SmartDbHelper.BaseRepository<HttpApp>
    {
        public static HttpApp GetByAppID(int appID)
        {
            var sql = "select top 1 * from HttpApp where AppID=@AppID";
            return Do(db => db.DapperDo(con => con.QueryFirstOrDefault<HttpApp>(sql, new { AppID = appID })));
        }

        public static List<HttpApp> GetHttpApps()
        {
            return GetAll().OrderBy(r => r.AppID).ToList();
        }
    }
}
