using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
namespace NetCoreMvcWebExample
{
    public class AppConfig
    {
        static IConfigurationRoot cr;
        public static void Init()
        {
            if (cr == null)
            {
                cr = new JsonAppConfigHelper("appsettings.json").GetConfiguration();
                /*
             System.TypeInitializationException:“The type initializer for 'NetCoreMvcWebExample.AppConfig' threw an exception.”
            The configuration file 'appsettings.json' was not found and is not optional. The physical path is 'x\bin\Debug\netcoreapp2.0\appsettings.json'.
            json文件没复制到输出目录
             */
            }
        }
        static AppConfig()
        {
            Init();
        }
        static string mainConn;
        public static string MainDBConn
        {
            get
            {
                if (string.IsNullOrEmpty(mainConn))
                {
                   mainConn= cr.GetConnectionString("DBConnStrinbg");
                }
                return mainConn;
            }
        }
    }
    /// <summary>
    /// 目前查找json文件时已知默认路径在bin目录下
    /// </summary>
    public class JsonAppConfigHelper
    {
        string jsonfile;
        /// <summary>
        /// 需要加载的json
        /// </summary>
        /// <param name="jsonFileName"></param>
        public JsonAppConfigHelper(string jsonFileName)
        {
            jsonfile = jsonFileName;
        }
        public IConfigurationRoot GetConfiguration()
        {
            var build = new ConfigurationBuilder();
            build.AddJsonFile(jsonfile);
            return build.Build();
        }

    }
}
