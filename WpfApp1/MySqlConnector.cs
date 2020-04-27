using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
/// 
/// KLASA DO OBSLUGI ZAPYTAN MYSQL  

namespace WpfApp1
{
    public class MySqlConnector {
        
        readonly MySqlConnection databaseCon;
        readonly string inputToConnect;
        static bool connectionIndicator;
        public MySqlConnector(string _inputToConnect)
        {
            this.inputToConnect = _inputToConnect;
            databaseCon = null;
            try
            {
                databaseCon = new MySqlConnection(inputToConnect);
                databaseCon.Open();
                connectionIndicator = true;
            }
            catch (MySqlException err)
            {
                connectionIndicator = false;
                Console.Write(err);
            }
            finally
            {
            }
        }
        public DataTable sendRequest(string request)
        {
            if (connectionIndicator == true)
            {
                DataTable data = new DataTable();

                MySqlCommand userCommand = new MySqlCommand(request, databaseCon);
                MySqlDataReader reader = userCommand.ExecuteReader();
                data.Load(reader);
                return data;
            }
            return new DataTable();   //Nie powiodlo sie
        }
        public bool isConnected()
        {
            if (connectionIndicator == true) return true;
            else return false;
        }
    
        ~MySqlConnector()
        {
            databaseCon.Close();
            connectionIndicator = false;
        }
    
    };
}

