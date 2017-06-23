using System;
using System.Data;
using Oracle.DataAccess.Client;
using Util;

namespace DataAccess
{
    public class DBConnection
    {
        private static OracleConnection con = new OracleConnection();
        private static OracleTransaction txn;
        public string EncryptConnectionString { get; set; }
        private static DBConnection _instance;

        public static DBConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DBConnection();
                }
                return _instance;
            }
        }

        public static DBConnection New
        {
            get { return new DBConnection(); }
        }

        private DBConnection()
        {
        }

        public bool TestConnection()
        {
            try
            {
                GetConnection();
                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteExceptionLog(ex, "TestConnection");
                return false;
            }
        }

        public OracleConnection GetConnection()
        {
            string constring;
            if (con.State == ConnectionState.Closed ||
                con.State == ConnectionState.Broken)
            {
                //Data Source=localhost/orcl;Persist Security Info=True;User ID=VATTU;Unicode=True
                //constring = String.Format(@"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=orcl)(PORT=localhost))(CONNECT_DATA=(SID=1521)));User Id=VATTU;Password=123");
                //constring = String.Format(@"Data Source=localhost/orcl;Persist Security Info=True;User ID=VATTU;Unicode=True");
                constring = String.Format(@"Data Source=(DESCRIPTION=
                (ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)
                (PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)
                (SERVICE_NAME=ORCL)));
                User Id=QLNS;Password=123");
                con.ConnectionString = constring;
                con.Open();
            }
            return con;
        }

        public void Close()
        {
            try
            {
                con.Close();
            }
            catch
            {
                con = new OracleConnection();
            }
        }

        public void BeginTransaction()
        {
            txn = GetConnection().BeginTransaction();
        }

        public void Rollback()
        {
            txn.Rollback();
            txn.Dispose();
        }

        public void Commit()
        {
            txn.Commit();
            txn.Dispose();
        }
    }
}