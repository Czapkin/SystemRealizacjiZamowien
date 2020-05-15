using System;
using System.Collections.Generic;
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
using SystemRealizacjiZamowien;

namespace ProjektWPF
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        string id = "1";
        Label labelt;
        string name;
        public Window1(Label label, string name)
        {
            InitializeComponent();
            ilosc.Text = id;
            labelt = label;
            this.name = name;

            this.Show();
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
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            Order.amountOfp = Int32.Parse(id);
            Order.sub = Order.price * Order.amountOfp;
            Order.total += Order.sub;
            Console.WriteLine(Order.sub);
            Console.WriteLine(Order.total);
            Console.WriteLine(Order.amountOfp);

            labelt.Content = Order.total.ToString();
            setCurrentOrder(name);

            Order.sub = 0;
            this.Close();
            mainWin.Show();
        }
    }
}
