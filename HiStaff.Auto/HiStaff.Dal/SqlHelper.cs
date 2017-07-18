using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiStaff.Domain;
using System.Data;
using System.Data.SqlClient;
namespace HiStaff.Dal
{
    public class SqlHelper
    {
        public static bool ExcuteScript(string scriptText)
        {
            SqlConnection conn = DbSqlConnection.GetConnection();
            SqlCommand command = conn.CreateCommand();
            command.CommandText = scriptText;
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();

            return true;
        }
        public static List<T> ExcuteSelectMultiObject<T>(string procname, Object parameter) where T : new()
        {
            List<PARAMETERS> lstParameter = SqlDalUtility.SelectParameters(procname);

            SqlConnection conn = DbSqlConnection.GetConnection();
            SqlCommand command = conn.CreateCommand();
            command.CommandText = procname;
            command.CommandType = CommandType.StoredProcedure;

            SqlDalUtility.LoadParametersFromObject(command, parameter, lstParameter);

            return SqlDalUtility.LoadObjectListFromDatabase<T>(command);
        }
        public static List<T> ExcuteSelectMultiObject<T>(string procname) where T : new()
        {
            return ExcuteSelectMultiObject<T>(procname, new Object());
        }
        public static T ExcuteSelectObject<T>(string procname, Object parameter) where T : new()
        {
            List<PARAMETERS> lstParameter = SqlDalUtility.SelectParameters(procname);

            SqlConnection conn = DbSqlConnection.GetConnection();
            SqlCommand command = conn.CreateCommand();

            command.CommandText = procname;
            command.CommandType = CommandType.StoredProcedure;

            SqlDalUtility.LoadParametersFromObject(command, parameter, lstParameter);

            return SqlDalUtility.LoadObjectFromDatabase<T>(command);
        }
        public static T ExcuteSelectObject<T>(string procname) where T : new()
        {
            return ExcuteSelectObject<T>(procname, new Object());
        }
        public static bool ExcuteNonQuery(string procname, Object parameter)
        {
            int resutl;

            List<PARAMETERS> lstParameter = SqlDalUtility.SelectParameters(procname);

            SqlConnection conn = DbSqlConnection.GetConnection();
            SqlCommand command = conn.CreateCommand();

            command.CommandText = procname;

            command.CommandType = CommandType.StoredProcedure;
            SqlDalUtility.LoadParametersFromObject(command, parameter, lstParameter);
            try
            {
                resutl = command.ExecuteNonQuery();

                SqlDalUtility.SetOutputValueToObject(command, parameter);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool ExcuteNonQuery(string procname)
        {
            return ExcuteNonQuery(procname, new Object());
        }

        public static bool CheckExsistStoreProc(string procname)
        {
           List<PARAMETERS> data = SqlDalUtility.SelectParameters(procname);
           if (data == null) return false;
           if (data.Count > 0)
               return true;
           else return false;
        }

        public static bool ExcuteCommandText(string commandText, Object parameter)
        {
            int resutl;
            try
            {
                List<PARAMETERS> lstParameter = SqlDalUtility.LoadParametersFromCommandText(commandText);

                SqlConnection conn = DbSqlConnection.GetConnection();
                SqlCommand command = conn.CreateCommand();

                command.CommandText = commandText;

                command.CommandType = CommandType.Text;
                SqlDalUtility.LoadParametersFromObjectCommandText(command, parameter, lstParameter);

                resutl = command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                HiStaff.Util.Log.Instance.WriteExceptionLog(ex, "ExcuteCommandText");
                return false;
            }
        }
    }
}
