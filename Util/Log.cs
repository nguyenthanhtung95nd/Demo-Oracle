using System;
using System.IO;
using System.Text;

namespace Util
{
    public class Log
    {
        private static Log _instance;

        public static Log Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Log();
                }
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
            WriteLog(err.ToString());
        }

        public void WriteLog(string message)
        {
            try
            {
                string logFile = string.Empty;
                StreamWriter logWriter = null;
                string logPath = AppDomain.CurrentDomain.BaseDirectory + "\\log";
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                logFile = AppDomain.CurrentDomain.BaseDirectory +
                          string.Format("\\Log\\{0:yyyyMMdd}.log", DateTime.Today);
                if (File.Exists(logFile))
                {
                    logWriter = File.AppendText(logFile);

                }
                else
                {
                    logWriter = File.CreateText(logFile);
                }
                logWriter.WriteLine(message);
                logWriter.Close();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Error in writting log :" + message);
            }
        }
    }
}