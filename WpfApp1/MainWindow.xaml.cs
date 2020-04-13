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
        public MainWindow()
        {
            InitializeComponent();

            MySqlConnector mysql = new MySqlConnector
                  ("SERVER=localhost;DATABASE=db_system_realizacji_zamowien_posilkow_12042020;UID=root;PASSWORD=HASLO");
            DataTable n1 = new DataTable();
            n1 = mysql.sendRequest("SELECT * from tb_beverages");

            var WholeProduct = new List<string>();

            foreach (DataRow dbRow in n1.Rows)
            {
                foreach (DataColumn dbColumns in n1.Columns)
                {
                    var field1 = dbRow[dbColumns].ToString();
                    //Console.WriteLine(field1);
                    WholeProduct.Add(field1);
                }
            }

            var productName = new List<string>();

            for (int i = 1; i <= WholeProduct.Count; i += 7)
            {
                productName.Add((WholeProduct[i]));
            }

            var productCost = new List<string>();

            for(int i = 5; i <= WholeProduct.Count; i += 7)
            {
                productCost.Add(WholeProduct[i]);
            }


            var name = productName;  //new List<string>()
            /*{
                "Ham",
                "Cheese",
                "NugBg",
                "CrsChk",
                "ChiCheese",
                "dasdsadsa",
            };
            */
            var price = productCost;
            /*{
                "5",
                "10",
                "15",
                "20",
                "25",
                "33",
            };
            */
            for(int i=0; i<name.Count; i++)
            {
                Class1 button = new Class1(name[i], price[i])
                {
                    Tag = price[i]
                };

                button.Click += new RoutedEventHandler(button_Click);
                grid.Children.Add(button);
            }
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(string.Format("The price of the selected product is:  {0}.", (sender as Class1).Tag));
        }
    }
}
