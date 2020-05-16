using System;
using System.Linq;
using System.Windows;

//-------------------------------------DO OBSLUGI WYBORU KARTA CZY FIZYCZNY PIENIADZ-------------------------------------------

namespace SystemRealizacjiZamowien
{
    public partial class ChoosePaymentMethod : Window
    {
        public ChoosePaymentMethod()
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;
        }

        private void ButtonClickOpenCardPaymentWindow(object sender, RoutedEventArgs e)
        {
            var cardPaymentWindow = new CardPayment();
            cardPaymentWindow.Show();
            Hide();
        }

        private void ButtonClickOpenCashPaymentWindow(object sender, RoutedEventArgs e)
        {
            var cashPaymentWindow = new CashPayment();
            cashPaymentWindow.Show();
            Hide();

        }

        private void ButtonClickCloseCurrentWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            Close();
        }
    }
}
