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
        public static void Delete(Guid id)
        {
            var entity = Activator.CreateInstance<TEntity>();
            entity.SetID(id);
            HttpLogDbFactory.Instance.Delete<TEntity>(entity);
        }

        public static void Add(TEntity t)
        {
            HttpLogDbFactory.Instance.Add(t);
        }

        public static void AddUpdate(TEntity t)
        {
            HttpLogDbFactory.Instance.AddUpdate(t);
        }

        public static void Update(TEntity t)
        {
            HttpLogDbFactory.Instance.Update(t);
        }

        public static T Do<T>(Func<DapperDb, T> func)
        {
            return func(HttpLogDbFactory.Instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void Do(Action<DapperDb> action)
        {
            action(HttpLogDbFactory.Instance);
        }

        public static List<TEntity> GetAll()
        {
            return HttpLogDbFactory.Instance.DapperDo(con =>
            {
                var sql = "select * from " + typeof(TEntity).Name;
                return con.Query<TEntity>(sql).ToList();
            });
        }
    }
}
