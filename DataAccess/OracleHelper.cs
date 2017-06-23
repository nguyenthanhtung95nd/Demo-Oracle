using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Domain;
using DataAccess;

namespace DataAccess
{
    public class OracleHelper
    {
        #region Public function

        public static List<T> ExcuteSelectMultiObject<T>(string packname, string procname, Object parameter) where T : new()
        {
            try
            {
                List<USER_ARGUMENTS> lstUserArg = DalUtility.SelectUserArgs(packname, procname);

                OracleConnection conn = DBConnection.Instance.GetConnection();
                OracleCommand command = conn.CreateCommand();

                if (!string.IsNullOrEmpty(packname))
                    command.CommandText = packname + "." + procname;
                else command.CommandText = procname;

                command.CommandType = CommandType.StoredProcedure;
                command.BindByName = true;

                DalUtility.LoadParametersFromObject(command, parameter, lstUserArg);

                return DalUtility.LoadObjectListFromDatabase<T>(command);
            }
            catch (Exception ex)
            {
                DBConnection.Instance.Close();
                Util.Log.Instance.WriteExceptionLog(ex, "ExcuteSelectMultiObject");
                return new List<T>();
            }
        }

        public static List<T> ExcuteSelectMultiObject<T>(string packname, string procname) where T : new()
        {
            return ExcuteSelectMultiObject<T>(packname, procname, new Object());
        }

        public static T ExcuteSelectObject<T>(string packname, string procname, Object parameter) where T : new()
        {
            try
            {
                List<USER_ARGUMENTS> lstUserArg = DalUtility.SelectUserArgs(packname, procname);

                OracleConnection conn = DBConnection.Instance.GetConnection();
                OracleCommand command = conn.CreateCommand();

                if (!string.IsNullOrEmpty(packname))
                    command.CommandText = packname + "." + procname;
                else command.CommandText = procname;

                command.CommandType = CommandType.StoredProcedure;
                command.BindByName = true;

                DalUtility.LoadParametersFromObject(command, parameter, lstUserArg);

                return DalUtility.LoadObjectFromDatabase<T>(command);
            }
            catch (Exception ex)
            {
                DBConnection.Instance.Close();
                Util.Log.Instance.WriteExceptionLog(ex, "ExcuteSelectObject");
                return new T();
            }
        }

        public static T ExcuteSelectObject<T>(string packname, string procname) where T : new()
        {
            return ExcuteSelectObject<T>(packname, procname, new Object());
        }

        public static bool ExcuteNonQuery(string packname, string procname, Object parameter)
        {
            try
            {
                int resutl;

                List<USER_ARGUMENTS> lstUserArg = DalUtility.SelectUserArgs(packname, procname);

                OracleConnection conn = DBConnection.Instance.GetConnection();
                OracleCommand command = conn.CreateCommand();

                if (!string.IsNullOrEmpty(packname))
                    command.CommandText = packname + "." + procname;
                else command.CommandText = procname;

                command.CommandType = CommandType.StoredProcedure;
                command.BindByName = true;

                DalUtility.LoadParametersFromObject(command, parameter, lstUserArg);

                resutl = command.ExecuteNonQuery();

                DalUtility.SetOutputValueToObject(command, parameter);

                return true;
            }
            catch (Exception ex)
            {
                DBConnection.Instance.Close();
                Util.Log.Instance.WriteExceptionLog(ex, "ExcuteNonQuery");
                return false;
            }
        }

        public static bool ExcuteNonQuery(string packname, string procname)
        {
            return ExcuteNonQuery(packname, procname, new Object());
        }

        #endregion
        #region Dataset
        /// <summary>
        /// Just for report
        /// </summary>
        /// <param name="packname"></param>
        /// <param name="procname"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static DataSet ExcuteSelectDataSet(string packname, string procname, Object parameter)
        {
            try
            {
                List<USER_ARGUMENTS> lstUserArg = DalUtility.SelectUserArgs(packname, procname);

                OracleConnection conn = DBConnection.Instance.GetConnection();
                if (conn.State != ConnectionState.Open)
                    return new DataSet();

                OracleCommand command = conn.CreateCommand();

                if (!string.IsNullOrEmpty(packname))
                    command.CommandText = packname + "." + procname;
                else command.CommandText = procname;

                command.CommandType = CommandType.StoredProcedure;
                command.BindByName = true;

                DalUtility.LoadParametersFromObject(command, parameter, lstUserArg);

                DataSet dsReturn = new DataSet();
                OracleDataAdapter adap = new OracleDataAdapter(command);
                adap.Fill(dsReturn);

                List<USER_ARGUMENTS> lstRefCur = lstUserArg.FindAll(obj => obj.DATA_TYPE == DalUtility.ORACLE_CURSOR_TYPE);

                for (int idx = 0; idx < lstRefCur.Count; idx++)
                {
                    dsReturn.Tables[idx].TableName = lstRefCur[idx].ARGUMENT_NAME;
                }

                return dsReturn;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// just for report
        /// </summary>
        /// <param name="packname"></param>
        /// <param name="procname"></param>
        /// <returns></returns>
        public static DataSet ExcuteSelectDataSet(string packname, string procname)
        {
            return ExcuteSelectDataSet(packname, procname, new Object());
        }
        #endregion
        #region DynamicObject
        public static DynamicEntityList ExcuteSelectMultiDynamicObject(string packname, string procname, Object parameter)
        {
            List<USER_ARGUMENTS> lstUserArg = DalUtility.SelectUserArgs(packname, procname);

            OracleConnection conn = DBConnection.Instance.GetConnection();
            if (conn.State != ConnectionState.Open)
                return new DynamicEntityList();

            OracleCommand command = conn.CreateCommand();

            if (!string.IsNullOrEmpty(packname))
                command.CommandText = packname + "." + procname;
            else command.CommandText = procname;

            command.CommandType = CommandType.StoredProcedure;
            command.BindByName = true;

            DalUtility.LoadParametersFromObject(command, parameter, lstUserArg);

            return Dal4DynamicObject.LoadObjectListFromDatabase(command);
        }

        public static DynamicEntityList ExcuteSelectMultiDynamicObject(string packname, string procname)
        {
            return ExcuteSelectMultiDynamicObject(packname, procname, new Object());
        }
        #endregion
    }
}
