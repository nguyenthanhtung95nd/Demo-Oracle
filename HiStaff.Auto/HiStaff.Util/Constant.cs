using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiStaff.Util
{
    public class ORCL
    {
        public const string NAME = "Orcl";
        public const string ORCLUSER = "OrclUser";
        public const string ORCLPASSWORD = "OrclPassword";
        public const string ORCLSERVER = "OrclServer";
        public const string ORCLPORT = "OrclPort";
        public const string ORCLSERVICE = "OrclService";
    }
    public class SQL
    {
        public const string NAME = "Sql";
        public const string SQLUSER = "SqlUser";
        public const string SQLPASSWORD = "SqlPassword";
        public const string SQLDATABASE = "SqlDatabase";
        public const string SQLSERVER = "SqlServer";
    }
    public class INOUT
    {
        public const string NAME = "InOut";
        public const string STARTHOUR = "StartHour";
        public const string ENDHOUR = "EndHour";
        public const string PATHSAVE = "PathSave";
        public const string PATHBACKUP = "PathBackup";
        public const string REGET = "ReGet";
        public const string DELFILE = "DelFile";
        public const string BACKDATE = "Backdate";
    }
    public class COMMON
    {
        public const string NAME = "Common";
        public const string CYCLEGETDATA = "CycleGetData";
        public const string FORMATHOUR = "HH:mm:ss";

        public const string SAVENOTE = "Yêu cầu nhập đầy đủ thông tin cấu hình.";
        public const string SAVESUCESS = "Cấu hình đã được cập nhật thành công.";
        public const string SAVEFAIL = "Có lỗi xảy ra. Lưu không thành công.";
        public const string FORMAT_CONNECTION_ORCL = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3}))(CONNECT_DATA=(SID={4})));User Id={0};Password={1};";
        public const string FORMAT_CONNECTION_SQL = "server={0};uid={1};pwd={2};database={3}";
        public const string PROC_TVC_PRI_SAL = "TVC_PRI_SAL";
    }
}
