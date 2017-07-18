using System;
using Oracle.DataAccess.Client;
using System.Data;
using System.Configuration;
using HiStaff.Util;
using Microsoft.VisualBasic;

//Copyright (C) 2011-2012 TinhVan Consulting Co.,Ltd All Rights Reserved.
//
//Author: Ha.LH
//Create Date: 08-Mar-11
#region *** Update Histories *******************
// 1: Halh - 08-Mar-11
#endregion
namespace HiStaff.Dal
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
        public bool NewConnection()
        {
            try
            {
                con = new OracleConnection();
                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteExceptionLog(ex, "NewConnection");
                return false;
            }
        }
        public bool TestConnection(string user, string pass, string server, string port, string service)
        {
            try
            {
                OracleConnection orclCon = new OracleConnection();
                string constring;
                constring = string.Format(COMMON.FORMAT_CONNECTION_ORCL, user, pass, server, port, service);

                orclCon.ConnectionString = constring;
                orclCon.Open();

                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteExceptionLog(ex, "TestConnection");
                return false;
            }
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

        internal OracleConnection GetConnection()
        {
            string constring;
            if (con.State == ConnectionState.Closed ||
                con.State == ConnectionState.Broken)
            {
                string pathFile = AppDomain.CurrentDomain.BaseDirectory + "\\setting.ini";
                IniFile iniFile = new IniFile(pathFile);

                constring = string.Format(COMMON.FORMAT_CONNECTION_ORCL, 
                    iniFile.IniReadValue(ORCL.NAME, ORCL.ORCLUSER),
                    iniFile.IniReadValue(ORCL.NAME, ORCL.ORCLPASSWORD),
                    iniFile.IniReadValue(ORCL.NAME, ORCL.ORCLSERVER),
                    iniFile.IniReadValue(ORCL.NAME,ORCL.ORCLPORT),
                    iniFile.IniReadValue(ORCL.NAME,ORCL.ORCLSERVICE)
                    );

                con.ConnectionString = constring;
                con.Open();

                //Log.Instance.writeLog("Open connection");
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
