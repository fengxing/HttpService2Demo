using Dapper;
using SmartHttpEntity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBLL
{
    public class HttpConfigBLL
    {
        public List<HttpConfig> GetAll()
        {
            using (var con = DrapperHelper.GetSmartHttpOpenConnection())
            {
                try
                {
                    var sql = "select *   FROM  [HttpConfig]";
                    var ret= con.Query<HttpConfig>(sql).ToList();
                    return ret;
                }
                catch (Exception)
                {
                    return new List<HttpConfig>();
                }
            } 
        }
    }
}
