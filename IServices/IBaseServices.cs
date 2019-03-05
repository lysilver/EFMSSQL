using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace IServices
{
    public interface IBaseServices<T> where T : class, new()
    {
        #region 事务

        DbTransaction BeginTrans();

        #endregion 事务

        #region 添加

        /// <summary>
        /// DbSet.Add
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool InsertAdd(T t);

        /// <summary>
        ///BaseContext.Entry(t).State = EntityState.Added
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool InsertAttach(T t);

        bool InsertAdd(List<T> t);

        bool InsertAttach(List<T> t);

        /// <summary>
        /// 使用SqlBulkCopy批量插入数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        bool BulkInsert(string tableName, IList<T> list, int count = 10000, int timeout = 10000);

        #endregion 添加

        /// <summary>
        /// SaveChanges()
        /// </summary>
        /// <returns></returns>
        bool Save();

        /// <summary>
        /// 事务提交
        /// </summary>
        /// <returns></returns>
        bool SaveTrans();

        #region 修改

        /// <summary>
        /// 需要将修改人、修改时间值改成null,否则会修改这两个值
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Update(T t);

        /// <summary>
        /// 已经排除修改人、修改时间，但是字段命名可能不一样，需要修改下
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Update2(T t);

        bool UpdateFind(object id, T t);

        #endregion 修改

        #region 删除

        bool Delete(T t);

        bool DeleteRemove(object id);

        int DeleteInt(T t);

        bool Delete(Expression<Func<T, bool>> wherExpression);

        bool Delete(List<T> list);

        bool ExecuteSqlCommand(string ids, string sql, SqlParameter[] parameters);

        #endregion 删除

        #region 查询

        int SelectCount(Expression<Func<T, bool>> predicate);

        T FindId(object id);

        T Single(Expression<Func<T, bool>> predicate);

        T FindEntity(Expression<Func<T, bool>> predicate);

        List<T> FindList(Expression<Func<T, bool>> predicate);

        IQueryable<T> IqQueryable(Expression<Func<T, bool>> predicate);

        IEnumerable<T> Ienumerable(Expression<Func<T, bool>> predicate, Func<T, object> orderFunc,
            string order = "DESC");

        DataTable ToDataTable(string sql, SqlParameter[] parameters);

        //sql
        List<T> FindList(string sql);

        List<T> FindList(string sql, DbParameter dbParameter);

        //fenye

        /// <summary>
        ///EF 分页
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="rows">显示行数</param>
        /// <param name="orderFunc">排序字段</param>
        /// <param name="total">总数</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns></returns>
        List<T> PageList(Expression<Func<T, bool>> predicate, int pageIndex, int rows, Func<T, object> orderFunc, out int total, string orderBy = "DESC");

        List<T> PListNoTracking(Expression<Func<T, bool>> predicate, int pageIndex, int rows, string orderFunc, out int total, string orderBy = "DESC");

        List<T> PageListNoTracking(Expression<Func<T, bool>> predicate, int pageIndex, int rows, Func<T, object> orderFunc, out int total, string orderBy = "DESC");

        IEnumerable<T> PageIeEnumerable(Expression<Func<T, bool>> predicate, int pageIndex, int rows, Func<T, object> orderFunc, out int total, string orderBy = "DESC");

        List<T> PListNoTracking(Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2, int pageIndex, int rows, string orderFunc, out int total, string orderBy = "DESC");

        List<T> PListNoTracking(Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2, Expression<Func<T, bool>> predicate3, int pageIndex, int rows, string orderFunc, out int total, string orderBy = "DESC");

        List<T> PListNoTracking(Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2, Expression<Func<T, bool>> predicate3, Expression<Func<T, bool>> predicate4, int pageIndex, int rows, string orderFunc, out int total, string orderBy = "DESC");

        #endregion 查询

        #region 存储过程

        Dictionary<string, object> StoredProcedure(string storedProcedure, SqlParameter[] dbParameters);

        Tuple<List<T>, Dictionary<string, object>> StoredProcedureTuple(string storedProcedure, SqlParameter[] dbParameters);

        #endregion 存储过程

        #region 扩展方法  EF 批量操作的时候效率不高 EntityFramework.Extensions

        bool PlDelete(Expression<Func<T, bool>> expression);

        bool PlUpdate(Expression<Func<T, bool>> expression, Expression<Func<T, T>> iExpression);

        List<T> PlSelectList(Expression<Func<T, bool>> expression);

        List<T> PlSelectListFromCache(Expression<Func<T, bool>> expression);

        Tuple<int, List<T>> PlPageList(Expression<Func<T, bool>> predicate, int pageIndex, int rows,
           string orderFunc, string orderBy = "DESC");

        #endregion 扩展方法  EF 批量操作的时候效率不高 EntityFramework.Extensions
    }
}