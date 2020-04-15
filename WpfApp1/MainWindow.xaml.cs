using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Data;

namespace WpfApp1
{

    public partial class MainWindow : Window
    {

        public MySqlConnector tryToConnectAgain(MySqlConnector mysql,string loginString) // ponowne laczenie z baza
        {
            while (!mysql.isConnected())   // sprawdza czy istnieje polaczenie z baza 
            {
                MessageBoxResult result = MessageBox.Show("Failed connecting to database\nTry again?", "ERROR"
                    , MessageBoxButton.YesNo);
                mysql = new MySqlConnector(loginString);
                switch (result)
                {
                    case MessageBoxResult.No:
                        {
                            System.Environment.Exit(0);
                            break;
                        }
                    case MessageBoxResult.Yes:
                        {
                            continue;
                        }

                }
            }
            return mysql;
        }

        public void loopThroughDataT(DataTable element, List<string> lista)
        {
            foreach (DataRow dbRow in element.Rows)
            {
                foreach (DataColumn dbColumns in element.Columns)
                {
                    var field1 = dbRow[dbColumns].ToString();
                    //Console.WriteLine(field1);
                    lista.Add(field1);
                }
            }
        }

        public List<string> parseDataFromDataTable(int indexOfProdInDB, List<string>WholeProducts)
        {
            List<string> temporaryArr = new List<string>();
            
            for (; indexOfProdInDB <= WholeProducts.Count; indexOfProdInDB += 7) 
            {                                                
                temporaryArr.Add((WholeProducts[indexOfProdInDB]));          
            }
            return temporaryArr;
        }

        public MainWindow()
        {
            InitializeComponent();
            string loginString =
                "SERVER=localhost;DATABASE=db_system_realizacji_zamowien_posilkow_12042020;UID=root;PASSWORD=das";

                // TU WYZEJ WPISUJCIE HASLO

            MySqlConnector mysql = new MySqlConnector(loginString);

            if (!mysql.isConnected())
            {
                mysql = tryToConnectAgain(mysql,loginString);  // Jesli nie nawiazano polaczenia z baza sproboj ponownie
            }


            DataTable n1 = new DataTable();
            DataTable n2 = new DataTable();
            DataTable n3 = new DataTable(); // kontenery dla wynikow zapytan
            DataTable n4 = new DataTable();
            DataTable n5 = new DataTable();

            n1 = mysql.sendRequest("SELECT * from tb_beverages");
            n2 = mysql.sendRequest("SELECT * from tb_desserts");
            n3 = mysql.sendRequest("SELECT * from tb_sandwiches");
            n4 = mysql.sendRequest("SELECT * from tb_sets");
            n5 = mysql.sendRequest("SELECT * from tb_snacks");

            var WholeProduct = new List<string>(); // znajduja sie tu wszystkie elementy z bazy danych(cena, nazwa itd)
            loopThroughDataT(n1, WholeProduct);
            loopThroughDataT(n2, WholeProduct);    // zapelniamy liste stringow WholeProduct wszystkimi elementami z bazy
            loopThroughDataT(n3, WholeProduct);    // implementacja funkcji wyzej.
            loopThroughDataT(n4, WholeProduct);
            loopThroughDataT(n5, WholeProduct);

            int iterator = 0;                
            List<string>productName = parseDataFromDataTable(iterator = 1, WholeProduct);
            List<string>productCost = parseDataFromDataTable(iterator = 5, WholeProduct);
                
            // nazwa produktu w bazie danych jest druga (liczac od 0)
            // iterujemy po tablicy ktora zawiera wszystkie elementy bazy 
            // (lacznie 7) dzieki czemu w tej petli dodajemy tylko nazwy produkt.
            // np pierwsze (nie liczac 0) pole w bazie danych to nazwa produktu 
            // piąte to jego koszt, cyklicznie zmieniają się zawsze o 7 (wszystkie pola)

            var name = productName;   //przypisujemy elementy listy do zmiennych name i price
            var price = productCost;

            for (int i = 0; i < name.Count; i++)
                {
                    Class1 button = new Class1(name[i], price[i])       // dalsza czesc to automatyczna generacja okienek
                    {
                        Tag = price[i]
                    };

                    button.Click += new RoutedEventHandler(button_Click);
                    grid.Children.Add(button);
                }  
        }




        void button_Click(object sender, RoutedEventArgs e)         // co sie stanie gdy nacisniemy na element
        {
                Console.WriteLine(string.Format("The price of the selected product is:  {0}.", (sender as Class1).Tag));
            }
        }
}


