using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Data;
using System.Windows.Controls;
using System.Windows.Media;

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
                    lista.Add(field1);
                }
            }
        }

        public List<string> parseDataFromDataTable(int indexOfProdInDB, int iterator, List<string>WholeProducts)
        {
            List<string> temporaryArr = new List<string>();
            
            for (; indexOfProdInDB <= WholeProducts.Count; indexOfProdInDB += iterator) 
            {                                                
                temporaryArr.Add((WholeProducts[indexOfProdInDB]));          
            }
            return temporaryArr;
        }

        public MainWindow()
        {
            InitializeComponent();
            string loginString =
                "SERVER=localhost;DATABASE=db_system_realizacji_zamowien_posilkow_21042020;UID=root;PASSWORD=";

            MySqlConnector mysql = new MySqlConnector(loginString);

            if (!mysql.isConnected())
            {
                mysql = tryToConnectAgain(mysql,loginString);  // Jesli nie nawiazano polaczenia z baza sproboj ponownie
            }

            const byte numOfFoodCategories = 5;

            DataTable[] n = new DataTable[numOfFoodCategories]; 
            DataTable categories = new DataTable();

            n[0] = mysql.sendRequest("SELECT * from tb_beverages");
            n[1] = mysql.sendRequest("SELECT * from tb_desserts");
            n[2] = mysql.sendRequest("SELECT * from tb_sandwiches");
            n[3] = mysql.sendRequest("SELECT * from tb_sets");
            n[4] = mysql.sendRequest("SELECT * from tb_snacks");
            categories = mysql.sendRequest("SELECT * from tb_categories");

            var WholeDesserts = new List<string>();
            var WholeBevereges = new List<string>();
            var WholeSandwiches = new List<string>();
            var WholeSets = new List<string>();
            var WholeSnacks = new List<string>();
            var listOfCategories = new List<string>();


            var WholeProduct = new List<string>(); // znajduja sie tu wszystkie elementy z bazy danych(cena, nazwa itd)

            
            for(int l=0; l<numOfFoodCategories; ++l)
            {
                loopThroughDataT(n[l], WholeProduct);
            }

            loopThroughDataT(categories, listOfCategories);
            loopThroughDataT(n[0], WholeBevereges);
            loopThroughDataT(n[1], WholeDesserts);
            loopThroughDataT(n[2], WholeSandwiches);
            loopThroughDataT(n[3], WholeSets);
            loopThroughDataT(n[4], WholeSnacks);


            int iterator = 0;

            var dessertsNames = parseDataFromDataTable(iterator = 1, 7, WholeDesserts);
            var dessertsCosts = parseDataFromDataTable(iterator = 5, 7, WholeDesserts);
            
            var sandwichesNames = parseDataFromDataTable(iterator = 1, 7, WholeSandwiches);
            var sandwichesCosts = parseDataFromDataTable(iterator = 5, 7, WholeSandwiches);

            var setsNames = parseDataFromDataTable(iterator = 1, 7, WholeSets);
            var setsCosts = parseDataFromDataTable(iterator = 5, 7, WholeSets);
            
            var snacksNames = parseDataFromDataTable(iterator = 1, 7, WholeSnacks);
            var snacksCosts = parseDataFromDataTable(iterator = 5, 7, WholeSnacks);
            
            var beveregesNames = parseDataFromDataTable(iterator = 1, 7, WholeBevereges);
            var beveregesCosts = parseDataFromDataTable(iterator = 5, 7, WholeBevereges);


            //var productName = parseDataFromDataTable(iterator = 1, 7, WholeProduct);
            //var productCost = parseDataFromDataTable(iterator = 5,7, WholeProduct);
            var categoriesDisplay = parseDataFromDataTable(iterator = 1, 2, listOfCategories);

            // nazwa produktu w bazie danych jest druga (liczac od 0)
            // iterujemy po tablicy ktora zawiera wszystkie elementy bazy 
            // (lacznie 7) dzieki czemu w tej petli dodajemy tylko nazwy produkt.
            // np pierwsze (nie liczac 0) pole w bazie danych to nazwa produktu 
            // piąte to jego koszt, cyklicznie zmieniają się zawsze o 7 (wszystkie pola)


            Class2[] button2 = new Class2[categoriesDisplay.Count];        


            for (int q = 0; q < categoriesDisplay.Count; ++q)
            {
                button2[q] = new Class2(categoriesDisplay[q]);
                grid.Children.Add(button2[q]);
            }

            button2[0].Background = Brushes.LawnGreen;
            button2[0].Click += new RoutedEventHandler(
            (sendItem, args) =>
            {
                var categoriesWindow = new Categories(setsNames, setsCosts);
            });

            button2[1].Background = Brushes.LawnGreen;
            button2[1].Click += new RoutedEventHandler(
       
           (sendItem, args) =>
           {
                var categoriesWindow = new Categories(beveregesNames, beveregesCosts);

            });
            button2[2].Background = Brushes.LawnGreen;
            button2[2].Click += new RoutedEventHandler(
            (sendItem, args) =>
            {
                var categoriesWindow = new Categories(sandwichesNames, sandwichesCosts);
            });
            button2[3].Background = Brushes.LawnGreen;
            button2[3].Click += new RoutedEventHandler(
             (sendItem, args) =>
             {
                 var categoriesWindow = new Categories(snacksNames, snacksCosts);
             });

            button2[4].Background = Brushes.LawnGreen;
            button2[4].Click += new RoutedEventHandler(
             (sendItem, args) =>
             {
                 var categoriesWindow = new Categories(dessertsNames, dessertsCosts);
             });

        }

    }
}


