using SmartHttpEntity;
using System.Collections.Generic;
using Dapper;

namespace SmartBLL
{
    public class HttpInvokeArgResp : BaseRepository<HttpInvokeArg>
    {
        public static void AddUpdate(int appID, string method, IEnumerable<HttpInvokeArg> args)
        {
            var sql = "delete HttpInvokeArg where AppID=@AppID and Method=@Method";
            Do(db => db.DapperDo(con => con.Execute(sql, new { AppID = appID, Method = method })));
            var list = new List<SmartBaseEntity.BaseEntity>();
            list.AddRange(args);
            Do(db => db.AddUpdate(list));
        }

        public static IEnumerable<HttpInvokeArg> Get(int appID,string method)
        {
            var sql = "select * from HttpInvokeArg where AppID=@AppID and Method=@Method";
            return Do(db => db.DapperDo(con => con.Query<HttpInvokeArg>(sql, new { AppID = appID, Method = method })));
        }
    }
}
