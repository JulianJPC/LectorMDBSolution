using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;

namespace LectorMDB.Clases
{
    public class BaseMDB
    {
        public string path { get; set; }
        public string hojaActual { get; set; }
        public int numeroHojaActual { get; set; }
        public int numeroHojaMaxima { get; set; }
        public int fontSize { get; set; }
        public int largoHojaActual { get; set; }
        private string queryMAXHoja { get; set; }
        private string queryGetOneHoja { get; set; }
        private string queryCampoHoja { get; set; }
        private string readerMDBString { get; set; }


        public void getDefaultsInts(List<int> values)
        {
            fontSize = values[0];
            numeroHojaMaxima = values[1];
        }
        public void getDefaultStrings(List<string> values)
        {
            queryMAXHoja = values[0];
            queryGetOneHoja = values[1];
            queryCampoHoja = values[2];
            readerMDBString = values[3];
        }


        public int searchHojaText(string textToSearch)
        {
            var response = 0;            
            //var query = queryGetOneHoja + counter.ToString();
            var query = $"SELECT HojaNro FROM Libro WHERE Hoja LIKE '%{textToSearch}%';";
            var result = readMDB(query);
            if(result.Rows.Count > 0)
            {
                response = Int32.Parse(getInfoDataTable(result, "HojaNro")[0]);
            }
            return response;
        }
        public DataTable readMDB(string query)
        /*
         * Given a query, execute it in the MDB file. If a pathQuery is given changes the location of MDB file.
         */
        {
            string myConnectionString = readerMDBString + path + ";";
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
        private List<string> getInfoDataTable(DataTable laTabla, string nombreCampo)
        /*
         *  Given a DataTable and the name of the field to look up, return info of field in List<string>
         */
        {
            List<string> laInfo = new List<string>();
            foreach (DataRow row in laTabla.Rows)
            {
                laInfo.Add(row[nombreCampo].ToString());
            }
            return laInfo;
        }
    }
}
