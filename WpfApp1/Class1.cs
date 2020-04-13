using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Data;

namespace WpfApp1
{
    public class MySqlConnector
    {
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
                if (databaseCon != null)
                {
                    //databaseCon.Close();
                    //connectionIndicator = false;
                }
            }
        }
        public DataTable sendRequest(string request)
        {
            if(connectionIndicator == true)
            {
                DataTable data = new DataTable();
                
                MySqlCommand userCommand = new MySqlCommand(request, databaseCon);
                MySqlDataReader reader = userCommand.ExecuteReader();
                data.Load(reader);
                Console.WriteLine("Dasdsadsa");
                return data;
            }
            return new DataTable();   //Nie powiodlo sie
        }
        public bool isConnected()
        {
            if (connectionIndicator == true) return true;
            else return false;
        }

    }
    
    public class Class1 : Button
    {
        public string name;
        public string price;

        public Class1(string name, string price)
        {
            this.name = name;
            this.price = price;
            setContent(this.name);
        }

        public void setContent(string name)
        {
            Content = name;
        }
    }
}
