using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiStaff.Domain
{
    /// <summary>
    ///    CREATE PROC [dbo].[PRS_PRO_PARAMETERS]
	///     @ProcedureName nvarchar(500)
    ///    AS
    ///    BEGIN
	///        SELECT 
	///	        A.SPECIFIC_NAME,
	///	        A.ORDINAL_POSITION, 
	///	        A.PARAMETER_MODE,
	///	        A.PARAMETER_NAME,
	///	        A.DATA_TYPE,
	///	        A.CHARACTER_MAXIMUM_LENGTH
	///        FROM INFORMATION_SCHEMA.PARAMETERS A
	///        WHERE A.SPECIFIC_NAME = @ProcedureName
    ///    END
    ///    GO
    /// </summary>
    public class PARAMETERS
    {
        public string SPECIFIC_NAME { get; set; }
        public int ORDINAL_POSITION { get; set; }
        public string PARAMETER_MODE { get; set; }
        public string PARAMETER_NAME { get; set; }
        public string DATA_TYPE { get; set; }
        public string CHARACTER_MAXIMUM_LENGTH { get; set; }
    }
}
