using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Threading;
using Infrastructure.ExtService;
using Domain.CommonData;
namespace NetCoreMvcExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            bg.DoWork += new DoWorkEventHandler(BackTodo);
        }
        static int ExcuteCur = 0;
        static BackgroundWorker bg = new BackgroundWorker();

        private static async void ExcuteLog()
        {
           await Task.Run(()=> LogHelper.WriteLogProcess($"执行次数{ExcuteCur++}"));
        }
        static void BackTodo(object sender,DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(3 * 1000);
                ExcuteLog();
            }
        }
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
    public interface SampleRepository<T> where T:class
    {
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(object id);
        T Get(object id);
        string ConnString { get; set; }
    }
    public interface BatchRepository<T> where T : class
    {
        List<bool> BatchAdd(List<T> data);
        List<bool> BatchDelete(List<object> ids);
        string ConnString { get; set; }
    }
    public class BaseRepository<T>: SampleRepository<T>,BatchRepository<T> where T:class
    {
        public bool Add(T entity) { return false; }
        public bool Update(T entity) { return false; }
        public bool Delete(object id) { return false; }
        public T Get(object id) { return null; }
        public string ConnString { get; set; }
        public List<bool> BatchAdd(List<T> data) { return new List<bool>(); }
        public List<bool> BatchDelete(List<object> ids) { return new List<bool>(); }
    }
    public class LogHelper
    {
        public  static  bool WriteLogProcess(string log)
        {
            string logDir = new AppDirHelper().GetAppDir(AppCategory.WebApp);
            DateTime time = DateTime.Now;
            string logFormat = Common.Data.CommonFormat.DateToHourIntFormat;//这里会切换使用的日志文件存储大小
            //如果不是当天作为文件名，需要在键一个目录
            logDir += "/" + time.ToString(Common.Data.CommonFormat.YearMonth);
            string file = time.ToString(logFormat) + ".log";
            LoggerWriter.CreateLogFile(log + time.ToString(Common.Data.CommonFormat.DateTimeMilFormat), logDir,
               ELogType.HeartBeatLine, file, true);
            //这里调用日志数据库存储
            return true;
        }
    }
}
