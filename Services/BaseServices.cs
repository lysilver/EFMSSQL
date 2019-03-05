using IRepository;
using IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
	public class BaseServices<T> : IBaseServices<T> where T : class, new()
	{
		protected IBaseRepository<T> IBaseRepository;

		//public BaseServices()
		//{
		//    IBaseRepository = new BaseRepository<T>();   //不使用AutoMapper 可以使用这种方式
		//}

		public DbTransaction BeginTrans()
		{
			return IBaseRepository.BeginTrans();
		}

		#region 添加

		public bool InsertAdd(T t)
		{
			return IBaseRepository.InsertAdd(t);
		}

		public bool InsertAttach(T t)
		{
			return IBaseRepository.InsertAttach(t);
		}

		public bool InsertAdd(List<T> t)
		{
			return IBaseRepository.InsertAdd(t);
		}

		public bool InsertAttach(List<T> t)
		{
			return IBaseRepository.InsertAttach(t);
		}

		public bool BulkInsert(string tableName, IList<T> list, int count = 10000, int timeout = 10000)
		{
			return IBaseRepository.BulkInsert(tableName, list, count, timeout);
		}

		#endregion 添加

		#region 保存

		public bool Save()
		{
			return IBaseRepository.Save();
		}

		public bool SaveTrans()
		{
			return IBaseRepository.SaveTrans();
		}

		#endregion 保存

		#region 修改

		public bool Update(T t)
		{
			return IBaseRepository.Update(t);
		}

		public bool Update2(T t)
		{
			return IBaseRepository.Update2(t);
		}

		public bool UpdateFind(object id, T t)
		{
			return IBaseRepository.UpdateFind(id, t);
		}

		#endregion 修改

		#region 删除

		public bool Delete(T t)
		{
			return IBaseRepository.Delete(t);
		}

		public bool DeleteRemove(object id)
		{
			return IBaseRepository.DeleteRemove(id);
		}

		public int DeleteInt(T t)
		{
			return IBaseRepository.DeleteInt(t);
		}

		public bool Delete(Expression<Func<T, bool>> wherExpression)
		{
			return IBaseRepository.Delete(wherExpression);
		}

		public bool Delete(List<T> list)
		{
			return IBaseRepository.Delete(list);
		}

		public bool ExecuteSqlCommand(string ids, string sql, SqlParameter[] parameters)
		{
			return IBaseRepository.ExecuteSqlCommand(ids, sql, parameters);
		}

		#endregion 删除

		#region 查询

		public int SelectCount(Expression<Func<T, bool>> predicate)
		{
			return IBaseRepository.SelectCount(predicate);
		}

		public T FindId(object id)
		{
			return IBaseRepository.FindId(id);
		}

		public T Single(Expression<Func<T, bool>> predicate)
		{
			return IBaseRepository.Single(predicate);
		}

		public T FindEntity(Expression<Func<T, bool>> predicate)
		{
			return IBaseRepository.FindEntity(predicate);
		}

		public List<T> FindList(Expression<Func<T, bool>> predicate)
		{
			return IBaseRepository.FindList(predicate);
		}

		public IQueryable<T> IqQueryable(Expression<Func<T, bool>> predicate)
		{
			return IBaseRepository.IqQueryable(predicate);
		}

		public IEnumerable<T> Ienumerable(Expression<Func<T, bool>> predicate, Func<T, object> orderFunc,
			string order = "DESC")
		{
			return IBaseRepository.Ienumerable(predicate, orderFunc, order);
		}

		public DataTable ToDataTable(string sql, SqlParameter[] parameters)
		{
			return IBaseRepository.ToDataTable(sql, parameters);
		}

		//sql
		public List<T> FindList(string sql)
		{
			return IBaseRepository.FindList(sql);
		}

		public List<T> FindList(string sql, DbParameter dbParameter)
		{
			return IBaseRepository.FindList(sql, dbParameter);
		}

		//fenye
		/// <summary>
		///EF 分页
		/// </summary>
		/// <param name="predicate">查询条件表达式</param>
		/// <param name="pageIndex">当前页</param>
		/// <param name="rows">显示行数</param>
		/// <param name="orderFunc">排序字段</param>
		/// <param name="total">总数</param>
		/// <param name="orderBy">排序方式,默认DESC</param>
		/// <returns></returns>
		public List<T> PageList(Expression<Func<T, bool>> predicate, int pageIndex, int rows, Func<T, object> orderFunc,
			out int total, string orderBy = "DESC")
		{
			return IBaseRepository.PageList(predicate, pageIndex, rows, orderFunc, out total, orderBy);
		}

		public List<T> PListNoTracking(Expression<Func<T, bool>> predicate, int pageIndex, int rows, string orderFunc, out int total, string orderBy = "DESC")
		{
			return IBaseRepository.PListNoTracking(predicate, pageIndex, rows, orderFunc, out total, orderBy);
		}

		public List<T> PageListNoTracking(Expression<Func<T, bool>> predicate, int pageIndex, int rows,
			Func<T, object> orderFunc, out int total, string orderBy = "DESC")
		{
			return IBaseRepository.PageListNoTracking(predicate, pageIndex, rows, orderFunc, out total, orderBy);
		}

		public IEnumerable<T> PageIeEnumerable(Expression<Func<T, bool>> predicate, int pageIndex, int rows,
			Func<T, object> orderFunc, out int total, string orderBy = "DESC")
		{
			return IBaseRepository.PageIeEnumerable(predicate, pageIndex, rows, orderFunc, out total, orderBy);
		}

		public List<T> PListNoTracking(Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2,
			int pageIndex, int rows, string orderFunc, out int total, string orderBy = "DESC")
		{
			return IBaseRepository.PListNoTracking(predicate1, predicate2, pageIndex, rows, orderFunc, out total,
				orderBy);
		}

		public List<T> PListNoTracking(Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2,
			Expression<Func<T, bool>> predicate3, int pageIndex, int rows, string orderFunc, out int total,
			string orderBy = "DESC")
		{
			return IBaseRepository.PListNoTracking(predicate1, predicate2, predicate3, pageIndex, rows, orderFunc, out total,
				orderBy);
		}

		public List<T> PListNoTracking(Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2,
			Expression<Func<T, bool>> predicate3, Expression<Func<T, bool>> predicate4, int pageIndex, int rows,
			string orderFunc, out int total, string orderBy = "DESC")
		{
			return IBaseRepository.PListNoTracking(predicate1, predicate2, predicate3, predicate4, pageIndex, rows, orderFunc, out total,
				orderBy);
		}

		#endregion 查询

		#region 存储过程

		public Dictionary<string, object> StoredProcedure(string storedProcedure, SqlParameter[] dbParameters)
		{
			return IBaseRepository.StoredProcedure(storedProcedure, dbParameters);
		}

		public Tuple<List<T>, Dictionary<string, object>> StoredProcedureTuple(string storedProcedure,
			SqlParameter[] dbParameters)
		{
			return IBaseRepository.StoredProcedureTuple(storedProcedure, dbParameters);
		}

		#endregion 存储过程

		#region 扩展方法

		public bool PlDelete(Expression<Func<T, bool>> expression)
		{
			return IBaseRepository.PlDelete(expression);
		}

		public bool PlUpdate(Expression<Func<T, bool>> expression, Expression<Func<T, T>> iExpression)
		{
			return IBaseRepository.PlUpdate(expression, iExpression);
		}

		public List<T> PlSelectList(Expression<Func<T, bool>> expression)
		{
			return IBaseRepository.PlSelectList(expression);
		}

		public List<T> PlSelectListFromCache(Expression<Func<T, bool>> expression)
		{
			return IBaseRepository.PlSelectListFromCache(expression);
		}

		public Tuple<int, List<T>> PlPageList(Expression<Func<T, bool>> predicate, int pageIndex, int rows,
			string orderFunc, string orderBy = "DESC")
		{
			return IBaseRepository.PlPageList(predicate, pageIndex, rows, orderFunc, orderBy);
		}

		#endregion 扩展方法
	}
}