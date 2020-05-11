using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
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
                    label.Content = Order.price;

                    Console.WriteLine(string.Format("The price of the selected product is:  {0}.", (sendIte as itemButton).Tag));
                });


                grid.Children.Add(itemButto[z]);
            };

            grid.Children.Add(retBtn);
        }


    }

}

