using Dapper;
using DapperHelper;
using SmartBaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartAuthBLL
{
    public abstract class HttpLogBaseRepository<TEntity> where TEntity : BaseEntity
    {
        public static T Do<T>(Func<DapperDb, T> func)
        {
            return func(HttpLogDbFactory.Instance);
        }
    }
}
