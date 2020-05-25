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
        public Categories(Label label,List<string> productNames, List<string> productPrices, List<string> fullname, bool indicator)
        {
            InitializeComponent();
            windowLoaded(label,productNames, productPrices,fullname,indicator);
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

        public void windowLoaded(Label label, List<string> productNames, List<string> productPrices, List<string> fullname, bool indicator)
        {

            System.Windows.Controls.Button retBtn = new Button();
            retBtn.Name = "Back";
            retBtn.Content = "Back";
            retBtn.Background = Brushes.LawnGreen;
            retBtn.Click += new RoutedEventHandler(
            (sendIte, arg) =>
            {
                var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
                mainWin.SearchFor.Text = "";
                mainWin.Show();
                Close();
                label.Content = Order.total.ToString();
            });

            grid.Children.Add(retBtn);

            if(indicator == true)
            {
                int i = 0;
                int inc = 0;
                
                var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
                string startsWith = mainWin.SearchFor.Text.ToString();

                for (int j = 0; j <productNames.Count; ++j)
                {
                    if (productNames[j].StartsWith(startsWith))
                    {
                        itemButton[] itemButto = new itemButton[productNames.Count];
                        itemButto[i] = new itemButton(productNames[i], productPrices[i], fullname[i]);
                        itemButto[i].Tag = productPrices[i];
                        grid.Children.Add(itemButto[i]);
                        inc++;

                        itemButto[i].Click += new RoutedEventHandler(
                   (sendIte, arg) =>
                   {


                       string productName = (string)(sendIte as itemButton).name;
                       string stringPrice = (string)(sendIte as itemButton).Tag;
                       string fullName = (string)(sendIte as itemButton).fullName;

                        Order.price = Double.Parse(stringPrice);
                        Order.name = productName;

                        onlyInstance = new Window1(label, fullName, productName);
                        onlyInstance.Name = "CalculateOrder";
                        onlyInstance.Show();
                        Hide();
                        Console.WriteLine(string.Format("The price of the selected product is:  {0}.", (sendIte as itemButton).Tag));
                   });

                    }
                   
                    i++;
                }


            }

            if (indicator == false)
            {

                itemButton[] itemButto = new itemButton[productNames.Count];
                for (int z = productNames.Count - 1; z >= 0; --z)
                {

                    itemButto[z] = new itemButton(productNames[z], productPrices[z], fullname[z]);
                    itemButto[z].Tag = productPrices[z];

                    itemButto[z].Click += new RoutedEventHandler(
                    (sendIte, arg) =>
                    {

                    //string amountOfProd = Order.amountOfp.ToString();

                        string productName = (string)(sendIte as itemButton).name;
                        string stringPrice = (string)(sendIte as itemButton).Tag;
                        string fullName = (string)(sendIte as itemButton).fullName;
                    //string final = amountOfProd + " " + productName;

                        Order.price = Double.Parse(stringPrice);
                        Order.name = productName;
                    //Order.productNames.Add(fullName);
                    onlyInstance = new Window1(label, productName, fullName);
                        onlyInstance.Name = "CalculateOrder";
                        onlyInstance.Show();
                        Hide();
                        Console.WriteLine(string.Format("The price of the selected product is:  {0}.", (sendIte as itemButton).Tag));
                        var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
                        mainWin.SearchFor.Text = "";
                    });

                    grid.Children.Add(itemButto[z]);
                };

                //grid.Children.Add(retBtn);
            }
        }
    
            
    
    }
}

