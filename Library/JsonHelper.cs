using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Library
{
    public class JsonHelper
    {
        /// <summary>
        /// 类对像转换成json格式
        /// </summary>
        /// <returns></returns>
        public static string ToJson(object t)
        {
            return JsonConvert.SerializeObject(t, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });
        }

        /// <summary>
        /// 类对像转换成json格式
        /// </summary>
        /// <param name="t"></param>
        /// <param name="HasNullIgnore">是否忽略NULL值</param>
        /// <returns></returns>
        public static string ToJson(object t, bool hasNullIgnore)
        {
            if (hasNullIgnore)
                return JsonConvert.SerializeObject(t, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            else
                return ToJson(t);
        }

        public static T FromJson<T>(string strJson) where T : class
        {
            if (!string.IsNullOrEmpty(strJson))
                return JsonConvert.DeserializeObject<T>(strJson);
            return null;
        }
    }
}