using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Oracle.DataAccess.Client;


namespace DataAccess
{
    public class DalUtility
    {
        public const string ENTITY_CLASSNAME = "Entity";
        public const string ORALCE_PACKAGE_SYSTEM = "pkg_system";
        public const string ORALCE_PRS_USERARGS = "prs_sy_userargs";
        public const string ORALCE_PRS_USERARGS_ARG1 = "P_PACKAGE_NAME";
        public const string ORALCE_PRS_USERARGS_ARG2 = "P_OBJECT_NAME";
        public const string ORALCE_PRS_USERARGS_ARG3 = "P_CUR";
        public const string ORACLE_CURSOR_TYPE = "REF CURSOR";
        public const string ORACLE_DIRECTION_IN = "IN";
        public const string ORACLE_DIRECTION_OUT = "OUT";
   

        public static OracleDbType GetOracleDbType(string datatype)
        {
            switch (datatype)
            {
                case "NVARCHAR2": return OracleDbType.NVarchar2;
                case "NUMBER": return OracleDbType.Decimal;
                case "CLOB": return OracleDbType.Clob;
                case "NCLOB": return OracleDbType.NClob;
                case "CHAR": return OracleDbType.Char;
                case "DATE": return OracleDbType.Date;
                case "VARCHAR2": return OracleDbType.Varchar2;
                default: return OracleDbType.NVarchar2;
            }
        }

        public static DbType GetDbType(string datatype)
        {
            switch (datatype)
            {
                case "NVARCHAR2": return DbType.String;
                case "CLOB": return DbType.String;
                case "NUMBER": return DbType.Decimal;
                case "DATE": return DbType.DateTime;
                default: return DbType.String;
            }
        }

        public static EnumOraDbType GetEnumOraDbType(object o)
        {
            if (o is string) return EnumOraDbType.Varchar2;
            if (o is DateTime) return EnumOraDbType.Date;
            if (o is Int64) return EnumOraDbType.Int64;
            if (o is Int32) return EnumOraDbType.Int32;
            if (o is Int16) return EnumOraDbType.Int16;
            if (o is byte) return EnumOraDbType.Byte;
            if (o is decimal) return EnumOraDbType.Decimal;
            if (o is float) return EnumOraDbType.Single;
            if (o is double) return EnumOraDbType.Double;
            if (o is byte[]) return EnumOraDbType.Blob;
            return EnumOraDbType.Varchar2;
        }

        public static bool LoadObjectFromDataReader(object theInstanceType, IDataReader reader)
        {
            bool propertiesChanged = false;
            PropertyDescriptorCollection infos = TypeDescriptor.GetProperties(theInstanceType.GetType());

            //if (infos != null && infos.Count > 0)
            if (infos != null)
            {
                foreach (PropertyDescriptor info in infos)
                {
                    for (int f = 0; f < reader.FieldCount; f++)
                    {
                        string fName = reader.GetName(f);
                        string miName = info.Name;

                        if (miName.ToLower() == fName.ToLower())
                        {
                            object value = reader.GetValue(f);
                            if (value != DBNull.Value)
                            {
                                object obj = value;
                                TypeConverter typeConverter = info.Converter;
                                if (typeConverter != null)
                                {
                                    obj = typeConverter.ConvertFromString(value.ToString());
                                }
                                info.SetValue(theInstanceType, obj);
                                propertiesChanged = true;
                            }
                        }
                    }
                }
            }
            // So we can know if at least one property of the object is set
            return propertiesChanged;
        }

        public static T LoadObjectFromDataReader<T>(IDataReader reader) where T : new()
        {
            Type t = typeof(T);
            PropertyDescriptorCollection infos = TypeDescriptor.GetProperties(t);
            object theInstanceType = new T();
            LoadObjectFromDataReader(theInstanceType, reader);
            return (T)theInstanceType;
        }
        public static T LoadObjectFromDatabase<T>(OracleCommand command) where T : new()
        {
            bool isRead = false;
            T objT = new T();

            //data reader
            //Log.Instance.writeLog("Connection State:" + command.Connection.ConnectionString);
            //Log.Instance.writeLog("Connection State:" + command.Connection.State.ToString());

            IDataReader reader = command.ExecuteReader();
            isRead = reader.Read();

            if (isRead)
            {
                objT = (T)LoadObjectFromDataReader<T>(reader);

                reader.Close();
                return objT;
            }
            else
            {
                reader.Close();
                return default(T);
            }
        }
        public static List<T> LoadObjectListFromDatabase<T>(OracleCommand command) where T : new()
        {
            List<T> listT = new List<T>();

            //data reader

            //Log.Instance.writeLog("Line 138-Connection State:" + command.Connection.State.ToString());

            IDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                listT.Add((T)LoadObjectFromDataReader<T>(reader));
            }
            reader.Close();
            return listT;
        }

        public static List<USER_ARGUMENTS> SelectUserArgs(string packname, string procname)
        {
            OracleParameter param;
            OracleConnection conn = DBConnection.Instance.GetConnection();
            OracleCommand command = conn.CreateCommand();
            command.CommandText = ORALCE_PACKAGE_SYSTEM + "." + ORALCE_PRS_USERARGS;
            command.CommandType = CommandType.StoredProcedure;

            //Log.Instance.writeLog("Line 178-Connection State:" + command.Connection.State.ToString());

            param = new OracleParameter();
            param.ParameterName = ORALCE_PRS_USERARGS_ARG1;
            param.OracleDbType = OracleDbType.NVarchar2;
            param.Value = packname;
            param.Direction = ParameterDirection.Input;

            command.Parameters.Add(param);
            param = new OracleParameter();
            param.ParameterName = ORALCE_PRS_USERARGS_ARG2;
            param.OracleDbType = OracleDbType.NVarchar2;
            param.Value = procname;
            param.Direction = ParameterDirection.Input;
            command.Parameters.Add(param);

            param = new OracleParameter();
            param.ParameterName = ORALCE_PRS_USERARGS_ARG3;
            param.OracleDbType = OracleDbType.RefCursor;
            param.Direction = ParameterDirection.Output;
            command.Parameters.Add(param);

            return DalUtility.LoadObjectListFromDatabase<USER_ARGUMENTS>(command);
        }

        public static void LoadParametersFromObject(OracleCommand command, Object obj, List<USER_ARGUMENTS> lstUserArgs)
        {
            OracleParameter param;
            if (command == null) return;

            Type t = obj.GetType();
            PropertyDescriptorCollection infos = TypeDescriptor.GetProperties(t);
            //if (infos != null && infos.Count > 0)
            if (infos != null)
            {
                foreach (USER_ARGUMENTS UserArg in lstUserArgs)
                {
                    if (UserArg.DATA_TYPE == ORACLE_CURSOR_TYPE)
                    {
                        param = new OracleParameter();
                        param.ParameterName = UserArg.ARGUMENT_NAME;
                        if (UserArg.IN_OUT == ORACLE_DIRECTION_IN)
                            param.Direction = ParameterDirection.Input;
                        else if (UserArg.IN_OUT == ORACLE_DIRECTION_OUT)
                            param.Direction = ParameterDirection.Output;
                        else param.Direction = ParameterDirection.InputOutput;
                        param.OracleDbType = OracleDbType.RefCursor;
                        command.Parameters.Add(param);
                        continue;
                    }
                    else
                    {
                        if (UserArg.ARGUMENT_NAME != string.Empty && UserArg.POSITION > 0)
                        {
                            foreach (PropertyDescriptor info in infos)
                            {
                                if (UserArg.ARGUMENT_NAME.ToUpper() == ("P_" + info.Name).ToUpper())
                                {
                                    param = new OracleParameter();
                                    param.ParameterName = UserArg.ARGUMENT_NAME;
                                    param.Value = info.GetValue(obj);

                                    if (UserArg.IN_OUT == ORACLE_DIRECTION_IN)
                                        param.Direction = ParameterDirection.Input;
                                    else if (UserArg.IN_OUT == ORACLE_DIRECTION_OUT)
                                        param.Direction = ParameterDirection.Output;
                                    else param.Direction = ParameterDirection.InputOutput;

                                    param.OracleDbType = GetOracleDbType(UserArg.DATA_TYPE);
                                    param.DbType = GetDbType(UserArg.DATA_TYPE);

                                    command.Parameters.Add(param);
                                }
                                //if (info.GetValue(obj) != null){}
                            }
                        }
                        else
                        {
                            param = new OracleParameter();
                            param.Direction = ParameterDirection.ReturnValue;
                            param.OracleDbType = GetOracleDbType(UserArg.DATA_TYPE);
                            command.Parameters.Add(param);
                        }
                    }
                }
            }
        }

        public static void SetOutputValueToObject(OracleCommand command, Object obj)
        {
            if (command == null) return;

            Type t = obj.GetType();
            PropertyDescriptorCollection infos = TypeDescriptor.GetProperties(t);
            //if (infos != null && infos.Count > 0)
            if (infos != null)
            {
                foreach (OracleParameter param in command.Parameters)
                {
                    if (param.Direction == ParameterDirection.Output ||
                        param.Direction == ParameterDirection.InputOutput ||
                        param.Direction == ParameterDirection.ReturnValue)
                    {
                        foreach (PropertyDescriptor info in infos)
                        {
                            if (param.ParameterName.ToUpper() == ("P_" + info.Name).ToUpper())
                            {
                                info.SetValue(obj, param.Value);
                            }
                        }
                    }
                }
            }
        }
    }

   
}