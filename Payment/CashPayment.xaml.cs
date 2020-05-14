using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SystemRealizacjiZamowien
{
    public partial class CashPayment : Window
    {
        private string gotowka = "0,00";
        private int kropka = 0;
        private double kasa = 0;
        private double cashToPay;
        private double do_wydania;

        public void setCashToPay()
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            cashToPay = Convert.ToDouble(mainWin.CashToPay.Content);
        }

        public CashPayment()
        {
            WindowStyle = WindowStyle.None;
            InitializeComponent();
            setCashToPay();
            zaplacone.Text = gotowka;
            do_zaplaty.Text = cashToPay.ToString();
            do_wydania = Convert.ToDouble(value: gotowka) - cashToPay;
            change.Text = do_wydania.ToString();
        }

        private void Cyfra(object sender, RoutedEventArgs e)
        {
            if (gotowka == "0,00" && kropka == 0)
            {
                gotowka = (string)((Button)sender).Content;

                if (gotowka == "," || gotowka == "0")
                {
                    gotowka = "0,";
                    kropka = 1;
                }
            }

            else
            {
                if (kropka == 0)
                {
                    gotowka = gotowka + ((Button)sender).Content;

                    if ((string)((Button)sender).Content == ",")
                        kropka = 1;
                }

                else if (kropka > 0 && kropka < 3)
                {
                    if ((string)((Button)sender).Content != ",")
                    {
                        gotowka = gotowka + ((Button)sender).Content;
                        kropka++;
                    }
                }
            }

            do_wydania = Math.Round((Convert.ToDouble(value: gotowka) - cashToPay), 2);
            change.Text = do_wydania.ToString();
            zaplacone.Text = gotowka;
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            if (gotowka.Length > 1)
            {
                gotowka = gotowka.Remove(gotowka.Length - 1, 1);
                zaplacone.Text = gotowka;
                do_wydania = Convert.ToDouble(value: gotowka) - cashToPay;
                change.Text = do_wydania.ToString();
                if (kropka > 0)
                    kropka--;
            }

            else
            {
                gotowka = "0,00";
                kropka = 0;
                zaplacone.Text = gotowka;
                do_wydania = Convert.ToDouble(value: gotowka) - cashToPay;
                change.Text = do_wydania.ToString();
            }
        }

        private void Clear_all(object sender, RoutedEventArgs e)
        {
            gotowka = "0,00";
            zaplacone.Text = gotowka;
            kropka = 0;
            do_wydania = Convert.ToDouble(value: gotowka) - cashToPay;
            change.Text = do_wydania.ToString();
        }

        private void potwierdz(object sender, RoutedEventArgs e)
        {
            try
            {
                if (do_wydania < 0)
                {
                    throw new ArgumentException();
                }
                Close();
                Application.Current.MainWindow.Show();
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "ChoosePaymentMethod");
                win.Close();
                kasa = Convert.ToDouble(gotowka);
                var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
                mainWin.CashToPay.Content = "0,00";
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Change must be greater or equal 0\nCurrent change value is: " + do_wydania);
            }

        }

        private void anuluj(object sender, RoutedEventArgs e)
        {
            Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "ChoosePaymentMethod");
            win.Show();
            Close();
        }
    }
}
