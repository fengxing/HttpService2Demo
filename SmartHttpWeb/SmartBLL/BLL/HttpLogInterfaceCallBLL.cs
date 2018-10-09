using DapperHelper;
using SmartHttpEntity;
using System;
using System.Collections.Generic;
using Dapper;

namespace SmartBLL
{
    public class HttpLogInterfaceCallBLL
    {
        public DapperDb DapperDb { get; set; }

        public HttpLogInterfaceCallBLL()
        {
            var conn = System.Configuration.ConfigurationManager.AppSettings["SmartHttpLog"];
            DapperDb = new DapperDb("") { conn = conn };
        }

        public List<HttpLogInterfaceCall> GetHttpLogInterfaceCallList(int? appID,
           string method, string issuccess, string args, string executeid, string uid, int? code, int pageIndex, out int count)
        {
            var whereList = new List<string>();
            if (appID.HasValue)
            {
                whereList.Add("AppID=@AppID");
            }
            if (!string.IsNullOrWhiteSpace(method))
            {
                whereList.Add("Method=@Method");
            }
            var s = false;
            if (!string.IsNullOrWhiteSpace(issuccess))
            {
                s = Convert.ToBoolean(issuccess);
                whereList.Add("Issuccess=@Issuccess");
            }
            if (!string.IsNullOrWhiteSpace(args))
            {
                whereList.Add("(CONTAINS(Response,'\"*" + args + "*\"')  or CONTAINS(Request,'\"*" + args + "*\"'))");
            }
            if (!string.IsNullOrWhiteSpace(uid))
            {
                whereList.Add("UID=@UID");
            }
            if (!string.IsNullOrWhiteSpace(executeid))
            {
                whereList.Add("ExecuteID=@ExecuteID");
            }
            if (code.HasValue)
            {
                whereList.Add("StatusCode=@StatusCode");
            }
            var where = "";
            if (whereList.Count > 0)
            {
                where = " where " + string.Join(" And ", whereList);
            }
            var pageT = DapperDb.PageQuery<HttpLogInterfaceCall>(
            pageIndex,
            10,
            where,
            new
            {
                AppID = appID,
                Method = method.ToTrim(),
                Issuccess = s,
                UID = uid,
                StatusCode = code,
                Args = args,
                ExecuteID = executeid
            });
            count = pageT.Count;
            return pageT.Ts;
        }

        public HttpLogInterfaceCall GetByID(Guid id)
        {
            return DapperDb.DapperDo(con =>
            {
                var sql = "select * from HttpLogInterfaceCall where ID=@ID";
                return con.QueryFirstOrDefault<HttpLogInterfaceCall>(sql, new { ID = id });
            });
        }
    }
}
