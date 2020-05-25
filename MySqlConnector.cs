using System;
using MySql.Data.MySqlClient;
using System.Data;

//-------------------------------------OPTYMALIZACJA DO ZAPYTAN MYSQL-------------------------------------------

namespace SystemRealizacjiZamowien
{
    public class MySqlConnector {
        
        readonly public MySqlConnection databaseCon; //Zmiana na public
        readonly string inputToConnect;
        static bool connectionIndicator;

        public MySqlConnector(string _inputToConnect)
        {
            inputToConnect = _inputToConnect;
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

