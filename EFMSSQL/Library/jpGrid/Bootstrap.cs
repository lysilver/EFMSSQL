using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.jpGrid
{
    public class Bootstrap
    {
        public class BootstrapParams
        {
            /// <summary>
            /// 页码*页面显示行数=offset
            /// </summary>
            public int offset { get; set; }

            /// <summary>
            /// 页面显示行数
            /// </summary>
            public int limit { get; set; }

            /// <summary>
            /// 排序字段
            /// </summary>
            public string sort { get; set; }

            /// <summary>
            /// 排序方式
            /// </summary>
            public string order { get; set; }
        }

        public static object GridData(int total, object data)
        {
            var jsonData = new
            {
                total = total,
                rows = data
            };
            return jsonData;
        }
    }
}