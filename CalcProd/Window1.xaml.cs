using System;
using System.Collections.Generic;
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
        int iloscTemp;
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

        public void setCurrentOrderZero()
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            mainWin.CurrentOrder.Content += "";
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

            if (id == "")
            {
                id = "1";
                ilosc.Text = id;
            }
                
            Order.amountOfp = Int32.Parse(id);
            bool repeatedFlag = false;
            int special = 0;
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

            mainWin.CurrentOrder.Content += "";
            string test = mainWin.CurrentOrder.Content.ToString();

            if (id != "")
            {
                int temp = 0;


                if (test.Contains(this.name))
                {
                    repeatedFlag = true;
                }

                if (repeatedFlag == false)
                {
                    Order.accounts.Add(this.name, Order.amountOfp);

                }


                if (repeatedFlag == true)
                {
                    if (Order.accounts.ContainsKey(this.name)) {

                        iloscTemp = Order.accounts[this.name];
                        Order.accounts[this.name] = Order.amountOfp;

                    }
                }


                if ((id == "0") && test.Contains(this.name))
                {
                    mainWin.CurrentOrder.Content = test.Replace(iloscTemp + "x " + this.name + ", ", "");
                    Order.productNames.Remove(this.fullname);
                    Order.amountOfProd.Remove(iloscTemp);
                    Order.productPrices.Remove(iloscTemp * Order.price);
                    Order.accounts.Remove(this.name);
                    Order.individualPrice.Add(Order.price);

                    Order.total = Order.total - (iloscTemp * Order.price);
                    Order.sub = 0;
                    temp = 1;
                }
                else if (test.Contains(this.name))
                {

                    mainWin.CurrentOrder.Content = test.Replace(iloscTemp + "x " + this.name + ", ", "");
                    Order.productNames.Remove(this.fullname);
                    Order.amountOfProd.Remove(iloscTemp);
                    Order.productPrices.Remove(iloscTemp * Order.price);
                    Order.individualPrice.Add(Order.price);
                    temp = 0;
                    Order.total = Order.total - (iloscTemp * Order.price);

                }


                if ((id == "0") && test.Contains(this.name)==false)
                {
                    Order.accounts.Remove(this.name);
                    //mainWin.CurrentOrder.Content = test.Replace(iloscTemp + "x " + this.name + ", ", "");
                    //Order.amountOfProd.Remove(iloscTemp);
                    //Order.productPrices.Remove(iloscTemp * Order.price);
                    //Order.accounts.Remove(this.name);

                    //Order.total = Order.total - (iloscTemp * Order.price);
                    Order.sub = 0;
                    temp = 1;
                }


                if (temp == 0) { 
                Order.sub = Order.price * Order.amountOfp;
                Order.amountOfProd.Add(Order.amountOfp);
                Order.productPrices.Add(Order.sub);
                Order.productNames.Add(this.fullname);
                Order.individualPrice.Add(Order.price);
                setCurrentOrder(name);
            }

                Order.total += Order.sub;
                labelt.Content = Order.total.ToString();


                if (mainWin.CurrentOrder.Content.ToString() == "")
                {
                    labelt.Content = "0";
                }

                Order.sub = 0;
                mainWin.SearchFor.Text = "";
                mainWin.Show();
                Close();
            }
            else
                MessageBox.Show("Quantity must be greater than zero");
        }

        private void close(object sender, RoutedEventArgs e)
        {      

           var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            mainWin.SearchFor.Text = "";
            mainWin.Show();
            Close();
        }
    }
}
