using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProjektWPF;

//-------------------------------------KLASA DO OBSLUGI WYSWIETLANIA PRODUKTOW-------------------------------------------

namespace SystemRealizacjiZamowien
{
    public partial class Categories : Window
    {
        public static Window onlyInstance;
        public Categories(Label label,List<string> productNames, List<string> productPrices)
        {
            InitializeComponent();
            windowLoaded(label,productNames, productPrices);
            Show();
            WindowStyle = WindowStyle.None;
        }

        //-------------------------------------WYSWIETLA TEKST W LABELU - ZAMOWIENIE ILOSC-------------------------------------------
        public void setCurrentOrder(string productNamesShort)
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            mainWin.CurrentOrder.Content += Order.amountOfp.ToString() + " " + productNamesShort + ", ";
        }

        //-------------------------------------INICJUJE OKIENKA Z PRODUKTAMI I PRZYCISK BACK-------------------------------------------

        public void windowLoaded(Label label,List<string> productNames, List<string> productPrices)
        {

            itemButton [] itemButto = new itemButton[productNames.Count];

            System.Windows.Controls.Button retBtn = new Button();
            retBtn.Name = "Back";
            retBtn.Content = "Back";
            retBtn.Background = Brushes.LawnGreen;
            retBtn.Click += new RoutedEventHandler(
            (sendIte, arg) =>
            {
                this.Close();
                label.Content = Order.total.ToString();
                var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
                mainWin.Show();
                //onlyInstance.Show();
            });

            for (int z = productNames.Count - 1; z >= 0; --z)
            {

                itemButto[z] = new itemButton(productNames[z], productPrices[z]);
                itemButto[z].Tag = productPrices[z];

                itemButto[z].Click += new RoutedEventHandler(
                (sendIte, arg) =>
                {

                    //string amountOfProd = Order.amountOfp.ToString();
                    
                    string productName = (string)(sendIte as itemButton).name;
                    string stringPrice = (string)(sendIte as itemButton).Tag;
                    //string final = amountOfProd + " " + productName;

                    Order.price = Double.Parse(stringPrice);
                    Hide();
                    onlyInstance = new Window1(label,productName);
                    onlyInstance.Name = "CalculateOrder";
                    onlyInstance.Show();

                    Console.WriteLine(string.Format("The price of the selected product is:  {0}.", (sendIte as itemButton).Tag));
                });

                grid.Children.Add(itemButto[z]);
            };

            grid.Children.Add(retBtn);
        }
    }
}

