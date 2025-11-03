using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data;



namespace LectorMDB.MDBConexion
{
    public class MDBConexion
    {
        private string readerMDBString { get; set; }

        public void setConextionString(string theConnection)
        {
            readerMDBString = theConnection;
        }
        /// <summary>
        /// Given the query, path of the MDB to ask, params names in the query and their values, it
        /// uses OleDB to entablish connection and prepares the query with its values 
        /// and then ask the file and returns the result as a DataTable.
        /// </summary>
        private DataTable readMDB(string query, string mdbPath, List<string> paramsNames, List<string> paramsValues)
        {
            string myConnectionString = readerMDBString + mdbPath + ";";
            DataTable myDataTable = new DataTable();
            try
            {
                // Open OleDb Connection
                OleDbConnection myConnection = new OleDbConnection();
                myConnection.ConnectionString = myConnectionString;
                myConnection.Open();

                // Execute Queries
                OleDbCommand cmd = myConnection.CreateCommand();
                cmd.CommandText = query;
                for(int indexParam = 0; indexParam < paramsNames.Count; indexParam++)
                {
                    cmd.Parameters.AddWithValue(paramsNames[indexParam], paramsValues[indexParam]);
                }
                OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // close conn after complete

                // Load the result into a DataTable

                myDataTable.Load(reader);
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("OLEDB Connection FAILED: " + ex.Message);
            }
            return myDataTable;
        }
        private DataTable readMDB(string query, string mdbPath)
        {
            string myConnectionString = readerMDBString + mdbPath + ";";
            DataTable myDataTable = new DataTable();
            try
            {
                // Open OleDb Connection
                OleDbConnection myConnection = new OleDbConnection();
                myConnection.ConnectionString = myConnectionString;
                myConnection.Open();

                // Execute Queries
                OleDbCommand cmd = myConnection.CreateCommand();
                cmd.CommandText = query;
                OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // close conn after complete

                // Load the result into a DataTable

                myDataTable.Load(reader);
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("OLEDB Connection FAILED: " + ex.Message);
            }
            return myDataTable;
        }
        private DataTable readMDBWithRegex(string query, string mdbPath, List<string> paramsNames, List<string> paramsValues)
        {
            string myConnectionString = readerMDBString + mdbPath + ";";
            DataTable myDataTable = new DataTable();
            try
            {
                // Open OleDb Connection
                OleDbConnection myConnection = new OleDbConnection();
                myConnection.ConnectionString = myConnectionString;
                myConnection.Open();

                // Execute Queries
                OleDbCommand cmd = myConnection.CreateCommand();
                cmd.CommandText = query;
                for (int indexParam = 0; indexParam < paramsNames.Count; indexParam++)
                {
                    cmd.Parameters.AddWithValue(paramsNames[indexParam], "%" + paramsValues[indexParam] + "%");
                }
                OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // close conn after complete

                // Load the result into a DataTable

                myDataTable.Load(reader);
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("OLEDB Connection FAILED: " + ex.Message);
            }
            return myDataTable;
        }
        /// <summary>
        /// Given a DataTable and the name of the field to look up, return info of field in List<string>         
        /// </summary>
        private List<string> getInfoDataTable(DataTable laTabla, string nombreCampo)
        
        {
            List<string> laInfo = new List<string>();
            foreach (DataRow row in laTabla.Rows)
            {
                laInfo.Add(row[nombreCampo].ToString());
            }
            return laInfo;
        }
        /// <summary>
        /// Given the query, path of the MDB to ask, params names in the query and their values, and the filed name to search in the result,
        /// First ask the MDB the query and gets a DataTable with the raw information
        /// Then it searches the field name in each row and returns it as a List<string>.
        /// </summary>
        public List<string> getSimple(string query, string mdbPath, List<string> paramsNames, List<string> paramsValues, string campo)
        {
            var resultRaw = readMDB(query, mdbPath, paramsNames, paramsValues);
            var response = getInfoDataTable(resultRaw, campo);
            return response;
        }
        public List<string> getSimple(string query, string mdbPath, string campo)
        {
            var resultRaw = readMDB(query, mdbPath);
            var response = getInfoDataTable(resultRaw, campo);
            return response;
        }
        public List<string> getWithRegex(string query, string mdbPath, List<string> paramsNames, List<string> paramsValues, string campo)
        {
            var resultRaw = readMDBWithRegex(query, mdbPath, paramsNames, paramsValues);
            var response = getInfoDataTable(resultRaw, campo);
            return response;
        }

    }
}
