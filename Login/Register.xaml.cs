using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace SystemRealizacjiZamowien
{
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;
            pass_error.Visibility = Visibility.Hidden;
            fill_error.Visibility = Visibility.Hidden;
            pass_error2.Visibility = Visibility.Hidden;
            login_error.Visibility = Visibility.Hidden;
        }

        public void loopThroughDataT(DataTable element, List<string> lista)
        {
            foreach (DataRow dbRow in element.Rows)
            {
                foreach (DataColumn dbColumns in element.Columns)
                {
                    var field1 = dbRow[dbColumns].ToString();
                    lista.Add(field1);
                }
            }
        }

        public int check_login(List<string> nazwy)
        {
            int log_ok = 0;
            String log = login.Text;
            for (int i = 0; i < nazwy.Count; i++)
            {

                if (log == nazwy[i])
                {
                    log_ok = 0;
                    login_error.Visibility = Visibility.Visible;
                    break;
                }
                else
                {
                    log_ok = 1;
                    login_error.Visibility = Visibility.Hidden;
                }

            }

            return log_ok;
        }

        public List<string> parseDataFromDataTable(int indexOfProdInDB, int iterator, List<string> WholeProducts)
        {
            List<string> temporaryArr = new List<string>();

            for (; indexOfProdInDB < WholeProducts.Count; indexOfProdInDB += 1)
            {
                temporaryArr.Add((WholeProducts[indexOfProdInDB]));
            }
            return temporaryArr;
        }

        private void register(object sender, RoutedEventArgs e)
        {
            string loginString =
               "SERVER=localhost;DATABASE=db_system_realizacji_zamowien_posilkow_21042020;UID=root;PASSWORD=";


            MySqlConnector mysql = new MySqlConnector(loginString);

            String imie = first_name.Text;
            String nazwisko = last_name.Text;
            String miasto = city.Text;
            String ulica = street.Text;
            String nr_domu = house_number.Text;
            String kod = postcode.Text;
            String log = login.Text;
            String pass = password.Password;
            String pass2 = r_password.Password;
            int stanowisko = position.SelectedIndex + 1;
            DataTable n1 = new DataTable();
            n1 = mysql.sendRequest("SELECT login FROM tb_workers");
            int rows = n1.Rows.Count;
            var logins = new List<string>();
            loopThroughDataT(n1, logins);
            List<string> temporaryArr = new List<string>();
            int iterator = 0;
            var nazwy = parseDataFromDataTable(iterator = 0, 7, logins);
            int log_ok = 0;

            if (imie == "" || nazwisko == "" || miasto == "" || ulica == "" || nr_domu == "" || kod == "" || log == "" || pass == "" || stanowisko < 1)
            {
                fill_error.Visibility = Visibility.Visible;
                if (pass != pass2)
                    pass_error.Visibility = Visibility.Visible;
                else if (pass == pass2)
                    pass_error.Visibility = Visibility.Hidden;
                log_ok = check_login(nazwy);
                if (log_ok == 1)
                    login_error.Visibility = Visibility.Hidden;
                else
                    login_error.Visibility = Visibility.Visible;
            }
            else
            {
                fill_error.Visibility = Visibility.Hidden;
                log_ok = check_login(nazwy);
                if (log_ok == 1)
                {
                    if (pass == pass2)
                    {
                        bool capital = false;
                        fill_error.Visibility = Visibility.Hidden;
                        pass_error.Visibility = Visibility.Hidden;
                        for (int i = 0; i < pass.Length; i++)
                        {
                            if (char.IsUpper(pass, i) == true)
                                capital = true;
                        }

                        if (capital == true && pass.Length > 5 && log_ok == 1)
                        {
                            mysql.sendRequest("INSERT INTO tb_workers (firstName, secondName, street, houseNumber, postCode, cityOFResidence, login, password, position_id) " +
                            "VALUES ('" + imie + "', '" + nazwisko + "','" + ulica + "','" + nr_domu + "','" + kod + "','" + miasto + "','" + log + "','" + pass + "', " + stanowisko + ")");
                            MessageBox.Show("A new employee has been registered successfuly");
                            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
                            mainWin.Show();
                            Close();
                        }
                        else
                        {
                            pass_error2.Visibility = Visibility.Visible;
                        }
                    }
                    else
                        pass_error.Visibility = Visibility.Visible;
                }
            }
        }

        private void anuluj(object sender, RoutedEventArgs e)
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            mainWin.Show();
            Close();
        }
    }
}
