using Dapper;
using SmartAuthBLL;
using SmartHttpEntity;
using System;

namespace SmartBLL
{
    public static class HttpLogBLL
    {
        /// <summary>
        /// 异步保存日志
        /// </summary>
        /// <param name="log"></param>
        public static void SaveLog(HttpLogInterfaceCall log)
        {
            try
            {
                var sql = DapperHelper.DHelper.ToInsertSql(log);
                HttpLogInterfaceCallRep.Do(db => db.DapperDo(con => con.Execute(sql, log)));
            }
            catch (Exception ex)
            {
              
            }
        }


        public static void ApiCommitSuccess(Guid executeID)
        {
            try
            {
                var sql = "update HttpLogInterfaceCall set IsSuccess=1,ExecuteResult=1 where ExecuteID=@ExecuteID";
                HttpLogInterfaceCallRep.Do(db => db.DapperDo(con => con.Execute(sql, new { ExecuteID = executeID })));
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}