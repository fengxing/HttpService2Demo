using DapperHelper;
using SmartHttpEntity;
using Dapper;
using System;

namespace SmartBLL
{
    public class HttpNotifyRep : DapperDb
    {
        public HttpNotifyRep() : base(Conn.LogConnString) { }


        public HttpNotify GetHttpNotify(int appID, string method)
        {
            var sql = "select * from HttpNotify where AppID=@AppID and Method=@Method";
            return DapperDo(con => con.QueryFirstOrDefault<HttpNotify>(sql, new { AppID = appID, Method = method }));
        }


        public HttpNotify GetByID(Guid id)
        {
            var sql = "select * from HttpNotify where ID=@ID";
            return DapperDo(con => con.QueryFirstOrDefault<HttpNotify>(sql, new { ID = id }));
        }
    }
}
