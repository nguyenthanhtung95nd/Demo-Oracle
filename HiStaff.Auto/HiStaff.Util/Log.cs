using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HiStaff.Util
{
    public class Log
    {
        private static Log _instance;
        public static Log Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Log();
                return _instance;
            }
        }

        public void WriteExceptionLog(Exception objErr, string sFunc)
        {
            StringBuilder err = new StringBuilder();
            err.AppendLine("Function: " + sFunc);
            err.AppendLine("Datetime: " + DateTime.Now);
            err.AppendLine("Error message: " + objErr.Message.ToString());
            err.AppendLine("Stack trace: " + objErr.StackTrace);
            writeLog(err.ToString());
        }

        public void writeLog(string message)
        {
            try
            {
                string logFile = string.Empty;
                System.IO.StreamWriter logWriter = null;
                string logPath = AppDomain.CurrentDomain.BaseDirectory + "\\log\\";
                if (!System.IO.Directory.Exists(logPath))
                {
                    System.IO.Directory.CreateDirectory(logPath);
                }
                logFile = AppDomain.CurrentDomain.BaseDirectory + string.Format("\\Log\\{0:yyyyMMdd}.log", DateTime.Today);
                if (System.IO.File.Exists(logFile))
                {
                    logWriter = System.IO.File.AppendText(logFile);
                }
                else
                {
                    logWriter = System.IO.File.CreateText(logFile);
                }
                logWriter.WriteLine(message);
                logWriter.Close();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Error in writing log :" + e.Message);
            }
        }
    }
}
