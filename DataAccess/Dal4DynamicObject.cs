using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Oracle.DataAccess.Client;

namespace DataAccess
{
    public class Dal4DynamicObject
    {
        /// <summary>
        /// Get returned list objects from database based on input criteria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public static DynamicEntityList LoadObjectListFromDatabase(OracleCommand command)
        {
            bool isFirst = true;
            DynamicEntityList listT = new DynamicEntityList();

            //data reader

            //Log.Instance.writeLog("Line 138-Connection State:" + command.Connection.State.ToString());

            IDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (isFirst)
                {
                    for (int f = 0; f < reader.FieldCount; f++)
                    {
                        string fName = reader.GetName(f);
                        listT.Columns.Add(fName);
                    }
                }

                listT.Add(LoadObjectFromDataReader(reader));

                isFirst = false;
            }
            reader.Close();
            return listT;
        }

        /// <summary>
        /// Creates an object from the specified type and calls the DataReader => Object mapping function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static DynamicEntity LoadObjectFromDataReader(IDataReader reader)
        {
            // Create complex dynamic property and add child properties:
            DynamicEntity theInstanceType = new DynamicEntity();

            for (int f = 0; f < reader.FieldCount; f++)
            {
                string fName = reader.GetName(f);
                object value = reader.GetValue(f);

                theInstanceType[fName] = value;
            }

            return theInstanceType;
        }
    }
}
