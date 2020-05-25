using System;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SystemRealizacjiZamowien;

//-------------------------------------KALKULATOR WYBORU ILOSC PRODUKTOW-------------------------------------------

namespace ProjektWPF
{
    public partial class Window1 : Window
    {
        string id = "";
        Label labelt;
        string name;
        string fullname;
        public Window1(Label label, string name, string fullname)
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;
            ilosc.Text = id;
            labelt = label;
            this.name = name;
            this.fullname = fullname;
            this.Show();

            ProductName.Content = fullname;
        }

        public void setCurrentOrder(string productNamesShort)
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            mainWin.CurrentOrder.Content += Order.amountOfp.ToString() + "x " + productNamesShort + ", ";
        }

        private void Cyfra(object sender, RoutedEventArgs e)
        {
           if(id == "0")
                id = (string)((Button)sender).Content;
            else
                id = id + ((Button)sender).Content;
            ilosc.Text = id;
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            if(id.Length > 0)           //Usunięto problem z usuwaniem pustego pola
                id = id.Remove(id.Length - 1, 1);
            
            ilosc.Text = id;
        }

        private void Clear_all(object sender, RoutedEventArgs e)
        {
            id = id.Remove(0,id.Length);
            ilosc.Text = id;
        }

        private void potwierdz(object sender, RoutedEventArgs e)
        {
            if (id != "" && id != "0")  //Usunięto możliwość potwierdzenia ilości 0 lub " "
            {
                var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
                Order.amountOfp = Int32.Parse(id);
                Order.sub = Order.price * Order.amountOfp;

                Order.amountOfProd.Add(Order.amountOfp);
                Order.productPrices.Add(Order.sub);
                Order.total += Order.sub;
                Console.WriteLine(Order.sub);
                Console.WriteLine(Order.total);
                Console.WriteLine(Order.amountOfp);

                labelt.Content = Order.total.ToString();
                setCurrentOrder(name);

                Order.productNames.Add(this.fullname);
                Order.sub = 0;
                mainWin.Show();
                Close();
            }
            else
                MessageBox.Show("Quantity must be greater than zero");
        }

        private void close(object sender, RoutedEventArgs e)
        {

            
            
            //Order.amountOfProd.RemoveAt(Order.amountOfProd.Count - 1);
            //Order.productPrices.RemoveAt(Order.productPrices.Count - 1);
            //Order.amountOfProd.RemoveAt(Order.amountOfProd.Count - 1);

           //Order.amountOfProd.Add(Order.amountOfp);
           //Order.productPrices.Add(Order.sub);

           var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            mainWin.Show();
            Close();
        }
    }
}
