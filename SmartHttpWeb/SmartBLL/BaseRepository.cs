using Dapper;
using DapperHelper;
using SmartBaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBLL
{
    public abstract class BaseRepository<TEntity> where TEntity : BaseEntity
    {
        public static void Delete(Guid id)
        {
            var entity = Activator.CreateInstance<TEntity>();
            entity.SetID(id);
            DbFactory.Instance.Delete<TEntity>(entity);
        }

        public static void Add(TEntity t)
        {
            DbFactory.Instance.Add(t);
        }

        public static void AddUpdate(TEntity t)
        {
            DbFactory.Instance.AddUpdate(t);
        }

        public static void Update(TEntity t)
        {
            DbFactory.Instance.Update(t);
        }


        public static TEntity GetByID(Guid id)
        {
            return DbFactory.Instance.DapperDo(con =>
            {
                var sql = "select * from " + typeof(TEntity).Name + " where ID=@ID";
                return con.QueryFirstOrDefault<TEntity>(sql, new { ID = id });
            });
        }

        public static T Do<T>(Func<DapperDb, T> func)
        {
            return func(DbFactory.Instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void Do(Action<DapperDb> action)
        {
            action(DbFactory.Instance);
        }

        public static List<TEntity> GetAll()
        {
            return DbFactory.Instance.DapperDo(con =>
            {
                var sql = "select * from " + typeof(TEntity).Name;
                return con.Query<TEntity>(sql).ToList();
            });
        }
    }
}
