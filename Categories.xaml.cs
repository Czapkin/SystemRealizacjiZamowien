using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SystemRealizacjiZamowien
{
    public partial class Categories : Window
    {
        public Categories(Label label,List<string> productNames, List<string> productPrices)
        {
            InitializeComponent();
            windowLoaded(label,productNames, productPrices);
            this.Show();
        }

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
            });

            for (int z = productNames.Count - 1; z >= 0; --z)
            {

                itemButto[z] = new itemButton(productNames[z], productPrices[z]);
                itemButto[z].Tag = productPrices[z];

                itemButto[z].Click += new RoutedEventHandler(
                (sendIte, arg) =>
                {
                    string zz = (string)(sendIte as itemButton).Tag;
                    Order.price += Double.Parse(zz);
                    string showZ = Order.price.ToString();
                    label.Content = showZ;//Order.price;

                    Console.WriteLine(string.Format("The price of the selected product is:  {0}.", (sendIte as itemButton).Tag));
                });

                grid.Children.Add(itemButto[z]);
            };

            grid.Children.Add(retBtn);
        }
    }
}

