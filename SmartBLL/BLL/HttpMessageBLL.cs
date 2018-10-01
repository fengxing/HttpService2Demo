using Dapper;
using SmartHttpEntity;
using System;


namespace SmartBLL
{
    public class HttpMessageBLL
    {
        public static HttpMessage GetByAppIDAndMethod(int AppID, string method, string version)
        {
            using (var con = DrapperHelper.GetSmartHttpOpenConnection())
            {
                try
                {
                    var sql = "select Method,Url,AppID,HttpType,Moudle,LoopTime,LoopWaitTime,IsLog,WebServiceTemplate,ContentType,Headers,HttpEncoding,UserAgent,InterfaceArgsCount,InterfaceArgsJsonString,WsExcepitonsJsonString,WSExceptionType,IsValid,IsCache,CacheSeconds,[Version],[Status],[TimeOut],IsNeedLogin,IsNotify  FROM  [HttpMessage] where AppID=@AppID and Method=@Method and [Version]=@Version";
                    if (string.IsNullOrWhiteSpace(version))
                    {
                        version = "1.0";
                    }                   
                    var ret =   con.QuerySingle<HttpMessage>(sql, new { AppID = AppID, method = method, version = version });
                    return ret;
                }
                catch
                {
                    return null;
                }               
            }
        }

    }
}
