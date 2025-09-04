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

        public BaseMDB(int fs)
        /*
         * Constructor, set fontsize and numeroHojaMaxima.
         */
        {
            fontSize = fs;
            numeroHojaMaxima = -1;
        }

        public void darHoja(int numeroDeHoja)
        /*
         * Given a number of sheet changes hojaActual, numeroHojaActual and largoHojaActual.
         */
        {
            numeroHojaActual = numeroDeHoja;
            DataTable infoHoja = readMDB("SELECT Hoja FROM Libro WHERE HojaNro = " + numeroDeHoja.ToString());
            hojaActual = getInfoDataTable(infoHoja, "Hoja")[0];
            largoHojaActual = 0;
            foreach(string linea in hojaActual.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                if (linea.Length > largoHojaActual)
                {   
                    largoHojaActual = linea.Length;
                }
            }
        }
        public void setHojaMaxima()
        /*
         *  Set numeroHojaMaxima from MDB.
         */
        {
            DataTable infoHoja = readMDB("SELECT MAX(HojaNro) FROM Libro");
            numeroHojaMaxima = Convert.ToInt32(getInfoDataTable(infoHoja, "Expr1000")[0]);
        }
        public int newSizeRichBox()
        /*
         *  Return large of richBox, taking into account fontsize and largoHojaActual.
         */
        {
            float cantidad = Convert.ToSingle(4.9) + (Convert.ToSingle(0.6) * (Convert.ToSingle(fontSize) - 8));
            return Convert.ToInt32(largoHojaActual * cantidad);
        }      
        public DataTable readMDB(string query, string pathQuery = "")
        /*
         * Given a query, execute it in the MDB file. If a pathQuery is given changes the location of MDB file.
         */
        {
            if (pathQuery == "")
            {
                pathQuery = path;
            }
            string myConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                        @"Data Source=" + pathQuery + ";";
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
        public bool isMastracho(string fileName)
        /*
         * Test if fileName have a table called Libro with a field HojaNro.
         */
        {
            DataTable testHoja = readMDB("SELECT * FROM Libro WHERE HojaNro = 1", fileName);
            if (testHoja.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public List<string> searchLines(string clave, string regexString = "")
        /*
         *  Given a clave searches sheet by sheet and line by line if it contains that clave. If regex
         *  is given searches regex.
         */
        {
            string pattern = "";
            Regex rgx = new Regex("");
            List<string> laInfo = new List<string>();
            if (regexString != "")
            {
                pattern = regexString ;
                rgx = new Regex(pattern);
            }


            for (int i = 1; i <= numeroHojaMaxima; i++)
            {
                DataTable hojaTabla = readMDB("SELECT Hoja FROM Libro WHERE HojaNro = " + i.ToString());
                string hoja = getInfoDataTable(hojaTabla, "Hoja")[0];
                
                foreach (string linea in hoja.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    if (linea.Contains(clave) & regexString == "")
                    {
                        laInfo.Add("Número de Hoja: " + i.ToString() + "|-=-|" +linea);
                    }
                    else if (regexString != "")
                    {
                        if (rgx.IsMatch(linea))
                        {
                            laInfo.Add("Número de Hoja: " + i.ToString() + "|-=-|" + linea);
                        }
                    }
                }
            }
            return laInfo;
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
