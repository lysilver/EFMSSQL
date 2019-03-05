using EntityFramework.Extensions;
using IRepository;
using ORM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Repository
{
	public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
	{
		protected IDbSet<T> DbSet;
		public DbContext BaseContext;
		private bool _flag = false;
		private DbTransaction DbTrans { get; set; }
		private bool _disposed = false;

		public BaseRepository()
		{
			this.BaseContext = ContextFactory.GetCurrentContext();
			this.DbSet = this.BaseContext.Set<T>();
		}

		public DbContext Db2
		{
			get
			{
				if (BaseContext == null)
				{
					BaseContext = ContextFactory.GetCurrentContext();
				}
				return BaseContext;
			}
		}

		public DbContext Db()
		{
			return ContextFactory.GetCurrentContext();
		}

		#region 事务

		public DbTransaction BeginTrans()
		{
			var dbConnection = BaseContext.Database.Connection;
			if (dbConnection.State == ConnectionState.Closed)
			{
				dbConnection.Open();
			}
			DbTrans = dbConnection.BeginTransaction();
			DbTrans.Commit();
			return DbTrans;
		}

		#endregion 事务

		#region 保存

		public bool Save()
		{
			return BaseContext.SaveChanges() > 0;
		}

		public bool SaveTrans()
		{
			try
			{
				BeginTrans();
				if (DbTrans != null)
				{
					if (BaseContext.SaveChanges() > 0)
					{
						_flag = true;
					}
					DbTrans.Commit();
				}
			}
			catch (Exception e)
			{
				if (DbTrans != null)
				{
					DbTrans.Rollback();
				}
			}
			return _flag;
		}

		#endregion 保存

		#region 添加

		public bool InsertAdd(T t)
		{
			if (t != null)
			{
				//BaseContext.Entry<T>(t).State = EntityState.Added;
				DbSet.Add(t);
				return Save();
			}
			return _flag;
		}

		public bool InsertAttach(T t)
		{
			if (t != null)
			{
				DbSet.Attach(t);
				BaseContext.Entry(t).State = EntityState.Added;
				return Save();
			}
			return _flag;
		}

		public bool InsertAdd(List<T> t)
		{
			t.ForEach(s => DbSet.Add(s));
			return Save();
		}

		public bool InsertAttach(List<T> t)
		{
			foreach (var entity in t)
			{
				BaseContext.Entry<T>(entity).State = EntityState.Added;
			}
			return Save();
		}

		public bool BulkInsert(string tableName, IList<T> list, int count = 10000, int timeout = 10000)
		{
			try
			{
				if (BaseContext.Database.Connection.State != ConnectionState.Open)
				{
					BaseContext.Database.Connection.Open();
				}
				using (var bulkCopy = new SqlBulkCopy((SqlConnection)BaseContext.Database.Connection))
				{
					bulkCopy.BatchSize = count;
					bulkCopy.DestinationTableName = tableName;
					bulkCopy.BulkCopyTimeout = timeout;
					var table = new DataTable();
					var props = TypeDescriptor.GetProperties(typeof(T))
						.Cast<PropertyDescriptor>()
						.Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
						.ToArray();

					foreach (var propertyInfo in props)
					{
						bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
						table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
					}

					var values = new object[props.Length];
					foreach (var item in list)
					{
						for (var i = 0; i < values.Length; i++)
						{
							values[i] = props[i].GetValue(item);
						}

						table.Rows.Add(values);
					}

					bulkCopy.WriteToServer(table);
				}
				return true;
			}
			catch (Exception ex)
			{
				return _flag;
			}
		}

		#endregion 添加

		#region 修改

		public bool Update(T t)
		{
			if (t != null)
			{
				DbSet.Attach(t);
				PropertyInfo[] props = t.GetType().GetProperties();
				foreach (PropertyInfo prop in props)
				{
					if (prop.GetValue(t, null) != null)
					{
						if (prop.GetValue(t, null).ToString() == "&nbsp;")
							BaseContext.Entry(t).Property(prop.Name).CurrentValue = null;
						BaseContext.Entry(t).Property(prop.Name).IsModified = true;
					}
				}
				return Save();
			}
			return _flag;
		}

		public bool Update2(T t)
		{
			if (t != null)
			{
				DbSet.Attach(t);
				PropertyInfo[] props = t.GetType().GetProperties();
				foreach (PropertyInfo prop in props)
				{
					if (prop.GetValue(t, null) != null)
					{
						if (prop.GetValue(t, null).ToString() == "&nbsp;")
						{
							BaseContext.Entry(t).Property(prop.Name).CurrentValue = null;
						}
						else
						{
							if (prop.Name == "Creator" || prop.Name == "CreateTime")
							{
								BaseContext.Entry(t).Property(prop.Name).IsModified = false;
							}
							else
							{
								BaseContext.Entry(t).Property(prop.Name).IsModified = true;
							}
						}
					}
				}
				return Save();
			}
			return _flag;
		}

		public bool UpdateFind(object id, T t)
		{
			var model = FindId(id);
			if (model != null)
			{
				PropertyInfo[] props = t.GetType().GetProperties();
				PropertyInfo[] modeldInfos = model.GetType().GetProperties();
				foreach (PropertyInfo prop in props)
				{
					if (prop.GetValue(t, null) != null)
					{
						var name = prop.Name;
						foreach (PropertyInfo modeldInfo in modeldInfos)
						{
							var namem = modeldInfo.Name;
							if (namem == name)
							{
								prop.SetValue(model, prop.GetValue(t, null));
							}
						}
					}
				}
				return Save();
			}
			return _flag;
		}

		#endregion 修改

		#region 删除

		public bool Delete(T t)
		{
			if (t != null)
			{
				DbSet.Attach(t);
				BaseContext.Entry(t).State = EntityState.Deleted;
				return Save();
			}
			return _flag;
		}

		public bool DeleteRemove(object id)
		{
			var model = DbSet.Find(id);
			DbSet.Remove(model);
			return Save();
		}

		public int DeleteInt(T t)
		{
			DbSet.Attach(t);
			BaseContext.Entry(t).State = EntityState.Deleted;
			return BaseContext.SaveChanges();
		}

		public bool Delete(Expression<Func<T, bool>> whereExpression)
		{
			var entitys = DbSet.Where(whereExpression).ToList();
			entitys.ForEach(m => BaseContext.Entry<T>(m).State = EntityState.Deleted);
			return Save();
		}

		public bool Delete(List<T> list)
		{
			list.ForEach(m => BaseContext.Entry<T>(m).State = EntityState.Deleted);
			return Save();
		}

		public bool ExecuteSqlCommand(string ids, string sql, SqlParameter[] parameters)
		{
			try
			{
				if (BaseContext.Database.Connection.State != ConnectionState.Open)
				{
					BaseContext.Database.Connection.Open();
				}
				int result = BaseContext.Database.ExecuteSqlCommand(sql, parameters);
				return result > 0;
			}
			catch (Exception ex)
			{
				return _flag;
			}
		}

		#endregion 删除

		#region 查询

		public DataTable ToDataTable(string sql, SqlParameter[] parameters)
		{
			if (BaseContext.Database.Connection.State != ConnectionState.Open)
			{
				BaseContext.Database.Connection.Open();
			}
			var conn = (SqlConnection)BaseContext.Database.Connection;
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = conn;
			cmd.CommandText = sql;
			if (parameters != null)
			{
				foreach (var item in parameters)
				{
					cmd.Parameters.Add(item);
				}
			}
			SqlDataAdapter adapter = new SqlDataAdapter(cmd);
			DataTable table = new DataTable();
			adapter.Fill(table);
			conn.Close();//连接需要关闭
			conn.Dispose();
			return table;
		}

		public SqlDataReader ToDataReader(string sql, SqlParameter[] parameters)
		{
			if (BaseContext.Database.Connection.State != ConnectionState.Open)
			{
				BaseContext.Database.Connection.Open();
			}
			var conn = (SqlConnection)BaseContext.Database.Connection;
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = conn;
			cmd.CommandText = sql;
			if (parameters != null)
			{
				foreach (var item in parameters)
				{
					cmd.Parameters.Add(item);
				}
			}
			SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			return reader;
		}

		public int SelectCount(Expression<Func<T, bool>> predicate)
		{
			return DbSet.Where(predicate).Count();
		}

		public T Single(Expression<Func<T, bool>> predicate)
		{
			return DbSet.Single(predicate);
		}

		public T FindId(object id)
		{
			return DbSet.Find(id);
		}

		public T FindEntity(Expression<Func<T, bool>> predicate)
		{
			return DbSet.FirstOrDefault(predicate);
		}

		public List<T> FindList(Expression<Func<T, bool>> predicate)
		{
			return DbSet.Where(predicate).ToList();
		}

		public IQueryable<T> IqQueryable(Expression<Func<T, bool>> predicate)
		{
			return DbSet.Where(predicate).AsQueryable();
		}

		public IEnumerable<T> Ienumerable(Expression<Func<T, bool>> predicate, Func<T, object> orderFunc, string order = "DESC")
		{
			if (string.Equals(order, "DESC", StringComparison.CurrentCultureIgnoreCase))
			{
				return DbSet.Where(predicate).OrderByDescending(orderFunc);
			}
			return DbSet.Where(predicate).OrderBy(orderFunc);
		}

		//sql
		public List<T> FindList(string sql)
		{
			return BaseContext.Database.SqlQuery<T>(sql).ToList();
		}

		public List<T> FindList(string sql, DbParameter dbParameter)
		{
			return BaseContext.Database.SqlQuery<T>(sql, dbParameter).ToList();
		}

		/// <summary>
		///确定排序字段用的方法
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="pageIndex"></param>
		/// <param name="rows"></param>
		/// <param name="orderFunc"></param>
		/// <param name="total"></param>
		/// <param name="orderBy"></param>
		/// <returns></returns>
		public List<T> PageList(Expression<Func<T, bool>> predicate, int pageIndex, int rows, Func<T, object> orderFunc, out int total, string orderBy = "DESC")
		{
			total = SelectCount(predicate);
			if (string.Equals(orderBy, "DESC", StringComparison.CurrentCultureIgnoreCase))
			{
				return DbSet.Where(predicate).OrderByDescending(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows)
				   .AsQueryable().ToList();
			}
			else
			{
				return DbSet.Where(predicate).OrderBy(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows).AsQueryable().ToList();
			}
		}

		public IEnumerable<T> PageIeEnumerable(Expression<Func<T, bool>> predicate, int pageIndex, int rows,
			 Func<T, object> orderFunc, out int total, string orderBy = "DESC")
		{
			total = SelectCount(predicate);
			if (string.Equals(orderBy, "DESC", StringComparison.CurrentCultureIgnoreCase))
			{
				return DbSet.Where(predicate).OrderByDescending(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows).ToArray();
			}
			else
			{
				return DbSet.Where(predicate).OrderBy(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows).ToArray();
			}
		}

		public List<T> PageListNoTracking(Expression<Func<T, bool>> predicate, int pageIndex, int rows, Func<T, object> orderFunc, out int total, string orderBy = "DESC")
		{
			total = SelectCount(predicate);
			if (string.Equals(orderBy, "DESC", StringComparison.CurrentCultureIgnoreCase))
			{
				return DbSet.Where(predicate).OrderByDescending(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows)
					.AsQueryable().AsNoTracking().ToList();
			}
			else
			{
				return DbSet.Where(predicate).OrderBy(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows).AsQueryable().AsNoTracking().ToList();
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="pageIndex"></param>
		/// <param name="rows"></param>
		/// <param name="orderFunc">排序字段</param>
		/// <param name="total"></param>
		/// <param name="orderBy"></param>
		/// <returns></returns>
		public List<T> PListNoTracking(Expression<Func<T, bool>> predicate, int pageIndex, int rows, string orderFunc,
			 out int total, string orderBy = "DESC")
		{
			total = SelectCount(predicate);
			if (string.Equals(orderBy, "DESC", StringComparison.CurrentCultureIgnoreCase))
			{
				return DbSet.Where(predicate).OrderByDescendingz(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows)
					.AsQueryable().AsNoTracking().ToList();
			}
			else
			{
				return DbSet.Where(predicate).OrderByz(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows).AsQueryable().AsNoTracking().ToList();
			}
		}

		public List<T> PListNoTracking(Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2,
			int pageIndex, int rows, string orderFunc, out int total, string orderBy = "DESC")
		{
			total = 0;
			if (predicate1 != null && predicate2 != null)
			{
				var query = DbSet.Where(predicate1).Where(predicate2);
				total = query.Count();
				if (string.Equals(orderBy, "DESC", StringComparison.CurrentCultureIgnoreCase))
				{
					return query.OrderByDescendingz(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows)
						.AsQueryable().AsNoTracking().ToList();
				}
				else
				{
					return query.OrderByz(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows).AsQueryable()
						.AsNoTracking().ToList();
				}
			}
			else
			{
				return null;
			}
		}

		public List<T> PListNoTracking(Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2,
			Expression<Func<T, bool>> predicate3, int pageIndex, int rows, string orderFunc, out int total,
			string orderBy = "DESC")
		{
			total = 0;
			if (predicate1 != null && predicate2 != null && predicate3 != null)
			{
				var query = DbSet.Where(predicate1).Where(predicate2).Where(predicate3);
				total = query.Count();
				if (string.Equals(orderBy, "DESC", StringComparison.CurrentCultureIgnoreCase))
				{
					return query.OrderByDescendingz(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows)
						.AsQueryable().AsNoTracking().ToList();
				}
				else
				{
					return query.OrderByz(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows).AsQueryable()
						.AsNoTracking().ToList();
				}
			}
			else
			{
				return null;
			}
		}

		public List<T> PListNoTracking(Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2,
			Expression<Func<T, bool>> predicate3, Expression<Func<T, bool>> predicate4, int pageIndex, int rows,
			string orderFunc, out int total, string orderBy = "DESC")
		{
			total = 0;
			if (predicate1 != null && predicate2 != null && predicate3 != null && predicate4 != null)
			{
				var query = DbSet.Where(predicate1).Where(predicate2).Where(predicate3).Where(predicate4);

				total = query.Count();
				if (string.Equals(orderBy, "DESC", StringComparison.CurrentCultureIgnoreCase))
				{
					return query.OrderByDescendingz(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows)
						.AsQueryable().AsNoTracking().ToList();
				}
				else
				{
					return query.OrderByz(orderFunc).Skip(rows * (pageIndex - 1)).Take(rows).AsQueryable()
						.AsNoTracking().ToList();
				}
			}
			else
			{
				return null;
			}
		}

		//protected virtual void Dispose(bool disposing)
		//{
		//    if (!this._disposed)
		//    {
		//        if (disposing)
		//        {
		//            BaseContext.Dispose();
		//        }
		//    }
		//    this._disposed = true;
		//}

		//public void Dispose()
		//{
		//    Dispose(true);
		//    GC.SuppressFinalize(this);
		//}

		#endregion 查询

		#region 存储过程

		public Dictionary<string, object> StoredProcedure(string storedProcedure, SqlParameter[] dbParameters)
		{
			//SqlParameter[] paramsParameters = new SqlParameter[]
			//{
			//    new SqlParameter("@code",SqlDbType.NVarChar,100),
			//    new SqlParameter("@code2",SqlDbType.NVarChar,100)
			//};
			//paramsParameters[0].Direction = ParameterDirection.Output;
			//paramsParameters[1].Direction = ParameterDirection.Output;
			//var dd = BaseContext.Database.SqlQuery<T>("exec [test] @code out,@code2 out", paramsParameters).ToList();
			//var code = paramsParameters[0].Value;
			//var code2 = paramsParameters[1].Value;
			Dictionary<string, object> data = new Dictionary<string, object>();
			var list = BaseContext.Database.SqlQuery<T>(storedProcedure, dbParameters).ToList();
			foreach (var parameter in dbParameters)
			{
				if (parameter.Direction == ParameterDirection.Output)
				{
					data.Add(parameter.ParameterName, parameter.Value);
				}
			}
			return data;
		}

		public Tuple<List<T>, Dictionary<string, object>> StoredProcedureTuple(string storedProcedure, SqlParameter[] dbParameters)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			var list = BaseContext.Database.SqlQuery<T>(storedProcedure, dbParameters).ToList();
			foreach (var parameter in dbParameters)
			{
				if (parameter.Direction == ParameterDirection.Output)
				{
					data.Add(parameter.ParameterName, parameter.Value);
				}
			}
			return new Tuple<List<T>, Dictionary<string, object>>(list, data);
		}

		#endregion 存储过程

		#region 扩展方法

		public bool PlDelete(Expression<Func<T, bool>> expression)
		{
			return DbSet.Where(expression).Delete() > 0;
		}

		public bool PlUpdate(Expression<Func<T, bool>> expression, Expression<Func<T, T>> iExpression)
		{
			return DbSet.Where(expression).Update(iExpression) > 0;
		}

		public List<T> PlSelectList(Expression<Func<T, bool>> expression)
		{
			var query = DbSet.Where(expression).Future().ToList();
			return query;
		}

		public List<T> PlSelectListFromCache(Expression<Func<T, bool>> expression)
		{
			var query = DbSet.Where(expression).FromCache().AsQueryable().ToList();
			//var query2 = DbSet.Where(expression).FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromSeconds(10))).AsQueryable().ToList();
			return query;
		}

		public Tuple<int, List<T>> PlPageList(Expression<Func<T, bool>> predicate, int pageIndex, int rows,
			 string orderFunc, string orderBy = "DESC")
		{
			List<T> list = null;
			var query = DbSet.Where(predicate);
			var count = query.FutureCount().Value;
			if (string.Equals(orderBy, "DESC", StringComparison.CurrentCultureIgnoreCase))
			{
				list = query.OrderByDescendingz(orderFunc).Skip((pageIndex - 1) * rows).Take(rows).AsQueryable().ToList();
			}
			else
			{
				list = query.OrderByz(orderFunc).Skip((pageIndex - 1) * rows).Take(rows).AsQueryable().ToList();
			}
			return new Tuple<int, List<T>>(count, list);
		}

		#endregion 扩展方法
	}
}