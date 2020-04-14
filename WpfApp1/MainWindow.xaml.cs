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
        public void loopThroughDataT(DataTable element, List<string>lista)
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

        public MainWindow()
        {
            InitializeComponent();

            MySqlConnector mysql = new MySqlConnector
                  ("SERVER=localhost;DATABASE=db_system_realizacji_zamowien_posilkow_12042020;UID=root;PASSWORD=WAASZE");
            DataTable n1 = new DataTable();
            DataTable n2 = new DataTable();
            DataTable n3 = new DataTable(); // kontenery dla wynikow zapytan
            DataTable n4 = new DataTable();
            DataTable n5 = new DataTable();

            if (mysql.isConnected())
            {
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

                var productName = new List<string>();

                for (int i = 1; i <= WholeProduct.Count; i += 7) // nazwa produktu w bazie danych jest pierwsza (liczac od 0)
                {                                                // iterujemy po tablicy ktora zawiera wszystkie elementy bazy 
                    productName.Add((WholeProduct[i]));          // (lacznie 7) dzieki czemu w tej petli dodajemy tylko nazwy produkt.
                }

                var productCost = new List<string>();       // robimy to samo tylko dla cen

                for (int i = 5; i <= WholeProduct.Count; i += 7)
                {
                    productCost.Add(WholeProduct[i]);
                }


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
            else
            {
                MessageBox.Show("Brak połączenia z bazą danych");
                this.Close();
            }
        }


            void button_Click(object sender, RoutedEventArgs e)         // co sie stanie gdy nacisniemy na element
            {
                Console.WriteLine(string.Format("The price of the selected product is:  {0}.", (sender as Class1).Tag));
            }     
        
    }
}
