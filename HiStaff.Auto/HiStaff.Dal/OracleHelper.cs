using System;
using Oracle.DataAccess.Client;
using System.Data;
using HiStaff.Dal;
using System.Collections.Generic;
using HiStaff.Domain;

//Copyright (C) 2011-2012 TinhVan Consulting Co.,Ltd All Rights Reserved.
//
//Author: Ha.LH
//Create Date: 08-Mar-11
#region *** Update Histories *******************
// 1: Halh - 08-Mar-11
#endregion
namespace HiStaff.Dal
{
    public sealed class OracleHelper
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
                HiStaff.Util.Log.Instance.WriteExceptionLog(ex, "ExcuteSelectMultiObject");
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
                HiStaff.Util.Log.Instance.WriteExceptionLog(ex, "ExcuteSelectObject");
                return new T();
            }
        }

        public static T ExcuteSelectObject<T>(string packname, string procname) where T : new()
        {
            return ExcuteSelectObject<T>(packname,procname,new Object());
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
                HiStaff.Util.Log.Instance.WriteExceptionLog(ex, "ExcuteNonQuery");
                return false;
            }
        }

        public static bool ExcuteNonQuery(string packname, string procname)
        {
            return ExcuteNonQuery(packname, procname, new Object());
        }

        #endregion

        #region Private function

        #endregion
    }
}