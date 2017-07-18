using System;
using System.Data.SqlClient;
using System.Configuration;
using HiStaff.Util;
namespace HiStaff.Dal
{
    public sealed class DbSqlConnection
    {
        private static SqlConnection con = new SqlConnection();
        private static SqlTransaction txn;

        private DbSqlConnection()
        { 
        }
        public static bool TestConnection()
        {
            try
            {
                DbSqlConnection.GetConnection();
                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteExceptionLog(ex, "DBConnection");
                return false;
            }
        }
        public static bool TestConnection(string server, string user, string pass, string database)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection();
                string constring;
                constring = string.Format(COMMON.FORMAT_CONNECTION_SQL, server, user, pass, database);

                sqlCon.ConnectionString = constring;
                sqlCon.Open();

                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteExceptionLog(ex, "TestConnection");
                return false;
            }
        }
        internal static SqlConnection GetConnection()
        {
            string constring;
            if (con.State == System.Data.ConnectionState.Closed ||
                con.State == System.Data.ConnectionState.Broken)
            {
                string pathFile = AppDomain.CurrentDomain.BaseDirectory + "\\setting.ini";
                IniFile iniFile = new IniFile(pathFile);

                constring = string.Format(COMMON.FORMAT_CONNECTION_SQL,
                    iniFile.IniReadValue(SQL.NAME, SQL.SQLSERVER),
                    iniFile.IniReadValue(SQL.NAME, SQL.SQLUSER),
                    iniFile.IniReadValue(SQL.NAME, SQL.SQLPASSWORD),
                    iniFile.IniReadValue(SQL.NAME, SQL.SQLDATABASE)
                    );
                con.ConnectionString = constring;
                con.Open();
            }
            return con;
        }
        public static void Close()
        {
            con.Close();
        }

        public static void BeginTransaction()
        {
            txn = GetConnection().BeginTransaction();
        }

        public static void Rollback()
        {
            txn.Rollback();
            txn.Dispose();
        }

        public static void Commit()
        {
            txn.Commit();
            txn.Dispose();
        }
    }
}
