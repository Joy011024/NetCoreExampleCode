using System;

namespace LogicApplication
{
    public class LogHelper
    {
        void WriteLog()
        {
            FileLogHelper fl = new FileLogHelper();
            //记录文件日志
            DBLogHelper dbfl = new DBLogHelper();
            //记录日志到数据库

        }
    }
    class FileLogHelper
    {

    }
    class DBLogHelper
    { }
}
