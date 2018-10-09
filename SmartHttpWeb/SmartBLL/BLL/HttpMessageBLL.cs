using DapperHelper;
using SmartHttpEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace SmartBLL
{
    public class HttpMessageBLL : BaseRepository<HttpMessage>
    {
        public HttpMessageBLL()
        {

        }

        public string GetHttpAddress(int appID)
        {
            if (appID > 500)
            {
                var url = Do(db => db.DapperDo(con => con.QueryFirstOrDefault<string>("select top 1 Url from httpmessage where AppID=@AppID", new { AppID = appID })));
                if (!string.IsNullOrWhiteSpace(url))
                {
                    var arr = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (arr.Count > 2)
                    {
                        arr.RemoveAt(arr.Count - 1);
                        return string.Join("/", arr) + "/";
                    }
                    return url;
                }
            }
            else
            {

                var key = 7000 + appID;
                var sql = "SELECT top 1 *" +
                     " FROM [dbo].[HttpConfig]" +
                     " where TestValue like '%" + key.ToString() + "%'";
                var httpConfig = Do(db => db.DapperDo(con => con.QueryFirstOrDefault<HttpConfig>(sql, new { })));
                if (httpConfig != null)
                {
                    return "httpconfig-" + httpConfig.Name + "/";
                }
            }
            return "";
        }

        public List<HttpMessage> GetHttpMessageList(string method, long? appID, int pageIndex, string moudle, string submoudle, int? status, bool? isNotify, out int count)
        {
            var whereList = new List<string>();
            if (appID.HasValue)
            {
                whereList.Add("AppID=@AppID");
            }
            if (!string.IsNullOrWhiteSpace(method))
            {
                whereList.Add("Method like '%'+@Method+'%' or  Description like '%'+@Method+'%'");
            }
            if (!string.IsNullOrWhiteSpace(moudle))
            {
                whereList.Add("Moudle=@Moudle");
            }
            if (!string.IsNullOrWhiteSpace(submoudle))
            {
                whereList.Add("SubMoudle=@SubMoudle");
            }
            if (status.HasValue)
            {
                whereList.Add("Status=@Status");
            }
            if (isNotify.HasValue)
            {
                whereList.Add("IsNotify=@IsNotify");
            }
            var pageT = SmartDbHelper.BaseRepository<HttpMessage>.Search<HttpMessage>(
            pageIndex,
            10,
            false,
            "CreateTime",
            whereList,
            new
            {
                AppID = appID,
                Method = method.ToTrim(),
                Moudle = moudle,
                SubMoudle = submoudle,
                Status = status,
                IsNotify = isNotify
            });
            count = pageT.Count;
            return pageT.Ts;
        }

        public static HttpMessage GetByAppIDAndMethod(long appID, string method, string version)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                version = "1.0";
            }
            var sql = "select * from HttpMessage where AppID=@AppID and Method=@Method and Version=@Version";
            return Do(db => db.DapperDo(con => con.QueryFirstOrDefault<HttpMessage>(sql, new { AppID = appID, Method = method.Trim(), Version = version })));
        }

        public static IEnumerable<HttpMessage> GetHttpMessageList(long appID)
        {
            var sql = "select * from HttpMessage where AppID=@AppID";
            return Do(db => db.DapperDo(con => con.Query<HttpMessage>(sql, new { AppID = appID })));
        }

        public void DeleteApp(long appID)
        {
            var sql = "delete HttpMessage where AppID=@AppID";
            Do(db => db.DapperDo(con => con.Execute(sql, new { AppID = appID })));
        }

    }
}