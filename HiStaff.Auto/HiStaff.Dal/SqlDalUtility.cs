using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using HiStaff.Domain;
namespace HiStaff.Dal
{
    public class SqlDalUtility
    {
        private const string PRS_PRO_PARAMETERS = "SELECT A.SPECIFIC_NAME, A.ORDINAL_POSITION, A.PARAMETER_MODE, A.PARAMETER_NAME, A.DATA_TYPE, A.CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.PARAMETERS A WHERE A.SPECIFIC_NAME = '{0}'";
        private const string MODE_IN = "IN";
        private const string MODE_OUT = "OUT";
        public static List<PARAMETERS> LoadParametersFromCommandText(string sqlCommand)
        {
            List<PARAMETERS> lstParameters = new List<PARAMETERS>();
            int index = 0;
            int end = 0;
            string[] a = sqlCommand.Split(',');
            foreach (var x in a)
            {
                index = x.IndexOf("@");
                if (index != -1)
                {
                    end = x.IndexOf(")", index);
                    PARAMETERS obj = new PARAMETERS();
                    if (end != -1)
                        obj.PARAMETER_NAME = x.Substring(index, end - index - 1).Trim();
                    else obj.PARAMETER_NAME = x.Substring(index).Trim();

                    lstParameters.Add(obj);
                }
            }
            return lstParameters;
        }
        public static void LoadParametersFromObjectCommandText(SqlCommand command, Object obj, List<PARAMETERS> lstParameter)
        {
            SqlParameter param;
            if (command == null) return;

            Type t = obj.GetType();
            PropertyDescriptorCollection infos = TypeDescriptor.GetProperties(t);

            if (infos != null)
            {
                foreach (PARAMETERS p in lstParameter)
                {
                    foreach (PropertyDescriptor info in infos)
                    {
                        if (p.PARAMETER_NAME.ToUpper() == ("@" + info.Name).ToUpper())
                        {
                            param = new SqlParameter();
                            param.ParameterName = p.PARAMETER_NAME;
                            param.Value = info.GetValue(obj);

                            command.Parameters.Add(param);
                        }
                    }
                }
            }
        }
        public static List<T> LoadObjectListFromDatabase<T>(SqlCommand command) where T : new()
        {
            List<T> listT = new List<T>();
            IDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                listT.Add((T)LoadObjectFromDataReader<T>(reader));
            }
            reader.Close();
            return listT;
        }
        public static T LoadObjectFromDataReader<T>(IDataReader reader) where T : new()
        {
            Type t = typeof(T);
            PropertyDescriptorCollection infos = TypeDescriptor.GetProperties(t);
            object theInstanceType = new T();
            LoadObjectFromDataReader(theInstanceType, reader);
            return (T)theInstanceType;
        }
        public static bool LoadObjectFromDataReader(object theInstanceType, IDataReader reader)
        {
            bool propertiesChanged = false;
            PropertyDescriptorCollection infos = TypeDescriptor.GetProperties(theInstanceType.GetType());
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
                                    if (value.GetType().Name == "Byte[]")
                                    {
                                        obj = value;
                                    }
                                    else
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
        public static List<PARAMETERS> SelectParameters(string procname)
        {
            SqlConnection conn = DbSqlConnection.GetConnection();
            SqlCommand command = conn.CreateCommand();
            command.CommandText = string.Format(PRS_PRO_PARAMETERS, procname);
            command.CommandType = CommandType.Text;

            return SqlDalUtility.LoadObjectListFromDatabase<PARAMETERS>(command);
        }

        public static SqlDbType GetSqlDbType(string datatype)
        {
            switch (datatype)
            {
                case "nvarchar": return SqlDbType.NVarChar;
                case "int": return SqlDbType.Int;
                case "bigint": return SqlDbType.BigInt;
                case "bit": return SqlDbType.Bit;
                case "ntext": return SqlDbType.NText;
                case "nchar": return SqlDbType.NChar;
                case "date": return SqlDbType.Date;
                case "datetime": return SqlDbType.DateTime;
                case "varchar": return SqlDbType.VarChar;
                case "binary": return SqlDbType.Binary;
                case "image": return SqlDbType.Image;
                case "float": return SqlDbType.Float;
                case "decimal": return SqlDbType.Decimal;
                default: return SqlDbType.NVarChar;
            }
        }
        public static DbType GetDbType(string datatype)
        {
            switch (datatype)
            {
                case "nvarchar": return DbType.String;
                case "bigint": return DbType.Int64;
                case "int": return DbType.Int32;
                case "decimal": return DbType.Decimal;
                case "date": return DbType.DateTime;
                case "datetime": return DbType.DateTime;
                case "bit": return DbType.Boolean;
                default: return DbType.String;
            }
        }
        public static void LoadParametersFromObject(SqlCommand command, Object obj, List<PARAMETERS> lstParameter)
        {
            SqlParameter param;
            if (command == null) return;

            Type t = obj.GetType();
            PropertyDescriptorCollection infos = TypeDescriptor.GetProperties(t);

            if (infos != null)
            {
                foreach (PARAMETERS p in lstParameter)
                {
                    if (p.PARAMETER_NAME != string.Empty && p.ORDINAL_POSITION > 0)
                    {
                        foreach (PropertyDescriptor info in infos)
                        {
                            if (p.PARAMETER_NAME.ToUpper() == ("@" + info.Name).ToUpper())
                            {
                                param = new SqlParameter();
                                param.ParameterName = p.PARAMETER_NAME;
                                param.Value = info.GetValue(obj);

                                if (p.PARAMETER_MODE == MODE_IN)
                                    param.Direction = ParameterDirection.Input;
                                else if (p.PARAMETER_MODE == MODE_OUT)
                                    param.Direction = ParameterDirection.Output;
                                else param.Direction = ParameterDirection.InputOutput;

                                param.SqlDbType = GetSqlDbType(p.DATA_TYPE);
                                param.DbType = GetDbType(p.DATA_TYPE);

                                command.Parameters.Add(param);
                            }
                        }
                    }
                    else
                    {
                        param = new SqlParameter();
                        param.Direction = ParameterDirection.ReturnValue;
                        param.SqlDbType = GetSqlDbType(p.DATA_TYPE);
                        command.Parameters.Add(param);
                    }
                }
            }
        }
        public static T LoadObjectFromDatabase<T>(SqlCommand command) where T : new()
        {
            bool isRead = false;
            T objT = new T();

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
        public static void SetOutputValueToObject(SqlCommand command, Object obj)
        {
            if (command == null) return;
            Type t = obj.GetType();
            PropertyDescriptorCollection infos = TypeDescriptor.GetProperties(t);
            if (infos != null)
            {
                foreach (SqlParameter param in command.Parameters)
                {
                    if (param.Direction == ParameterDirection.Output ||
                        param.Direction == ParameterDirection.InputOutput ||
                        param.Direction == ParameterDirection.ReturnValue)
                    {
                        foreach (PropertyDescriptor info in infos)
                        {
                            if (param.ParameterName.ToUpper() == ("@" + info.Name).ToUpper())
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
