using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
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


        public Categories(List<string> productNames, List<string> productPrices)
        {
            InitializeComponent();
            windowLoaded(productNames, productPrices);
            this.Show();

        }

        public void windowLoaded(List<string> productNames, List<string> productPrices)
        {

            Class1[] buttonA = new Class1[productNames.Count];

            System.Windows.Controls.Button retBtn = new Button();
            retBtn.Name = "Back";
            retBtn.Content = "Back";
            retBtn.Background = Brushes.LawnGreen;
            retBtn.Click += new RoutedEventHandler(
            (sendIte, arg) =>
            {
                this.Close();
            });

            System.Windows.Controls.Button finBtn = new Button();
            finBtn.Name = "Finalize";
            finBtn.Content = "Finalize";
            finBtn.Background = Brushes.LawnGreen;
            finBtn.Click += new RoutedEventHandler(
            (sendIte, arg) =>
            {
                // TU MATI OGARNIAJ 
            });


            for (int z = productNames.Count - 1; z >= 0; --z)
            {

                buttonA[z] = new Class1(productNames[z], productPrices[z]);
                buttonA[z].Tag = productPrices[z];


                buttonA[z].Click += new RoutedEventHandler(
                (sendIte, arg) =>
                {

                    Console.WriteLine(string.Format("The price of the selected product is:  {0}.", (sendIte as Class1).Tag));

                });


                grid.Children.Add(buttonA[z]);
            };

            grid.Children.Add(retBtn);
            grid.Children.Add(finBtn);
        }


    }

}

