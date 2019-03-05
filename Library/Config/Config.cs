using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Config
{
    public class Config
    {
        public static object GetAppsetting(string key)
        {
            return ConfigurationManager.AppSettings[key].Trim();
        }
    }
}