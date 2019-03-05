using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
	public static class Tools
	{
		#region 判断对象是否为空

		/// <summary>
		/// 判断对象是否为空，为空返回true
		/// </summary>
		/// <typeparam name="T">要验证的对象的类型</typeparam>
		/// <param name="data">要验证的对象</param>
		public static bool IsNullOrEmpty<T>(this T data)
		{
			//如果为null
			if (data == null)
			{
				return true;
			}

			//如果为""
			if (data.GetType() == typeof(String))
			{
				if (string.IsNullOrEmpty(data.ToString().Trim()) || data.ToString() == "")
				{
					return true;
				}
			}

			//如果为DBNull
			if (data.GetType() == typeof(DBNull))
			{
				return true;
			}

			//不为空
			return false;
		}

		/// <summary>
		/// 判断对象是否为空，为空返回true
		/// </summary>
		/// <param name="data">要验证的对象</param>
		public static bool IsNullOrEmpty(this object data)
		{
			//如果为null
			if (data == null)
			{
				return true;
			}

			//如果为""
			if (data.GetType() == typeof(String))
			{
				if (string.IsNullOrEmpty(data.ToString().Trim()))
				{
					return true;
				}
			}

			//如果为DBNull
			if (data.GetType() == typeof(DBNull))
			{
				return true;
			}

			//不为空
			return false;
		}

		#endregion 判断对象是否为空

		#region 验证是否为数字

		/// <summary>
		/// 验证是否为数字
		/// </summary>
		/// <param name="number">要验证的数字</param>
		public static bool IsNumber(this string number)
		{
			//如果为空，认为验证不合格
			if (IsNullOrEmpty(number))
			{
				return false;
			}

			//清除要验证字符串中的空格
			number = number.Trim();

			//模式字符串
			string pattern = @"^[0-9]+[0-9]*[.]?[0-9]*$";

			//验证
			return RegexHelper.IsMatch(number, pattern);
		}

		#endregion 验证是否为数字
	}
}