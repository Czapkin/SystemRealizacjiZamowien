using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using MySql.Data.MySqlClient;

namespace SystemRealizacjiZamowien
{
    public partial class Login : Window
    {
        MySqlConnector mysql;
        string employeePosition;
        public Login(MySqlConnector mysql)
        {
            this.mysql = mysql;
            InitializeComponent();
            WindowStyle = WindowStyle.None;
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

        private void btnZaloguj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ZapytanieSQL = "SELECT login, password FROM tb_workers;";
                MySqlDataAdapter AdapterSQL = new MySqlDataAdapter();
                AdapterSQL.SelectCommand = new MySqlCommand(ZapytanieSQL, mysql.databaseCon);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(AdapterSQL);

                DataTable n1 = new DataTable();
                AdapterSQL.Fill(n1);

                var Users = new List<string>(); // znajduja sie tu wszystkie elementy z bazy danych(cena, nazwa itd)
                loopThroughDataT(n1, Users);


                for (int i = 0; i < Users.Count; i += 2)
                {
                    if (txtUsername.Text == Users[i])
                    {
                        if (txtPassword.Password == Users[i + 1])
                        {

                            Order.user = txtUsername.Text.ToString();
                            MainWindow.employeePosition = "";

                            DataTable[] PersonalInfo = new DataTable[1];
                            List<string> loginString = new List<string>();
                            MySqlConnector mysql = new MySqlConnector(MainWindow.loginString);

                            PersonalInfo[0] = mysql.sendRequest("SELECT firstName FROM tb_workers WHERE login = " + "'" + Order.user + "'");
                            MainWindow.loopThroughDataT(PersonalInfo[0], loginString);

                            MessageBox.Show("Hello, " + loginString[0] + "!");  //txtUsername.Text
                            Application.Current.MainWindow.Show();
                            Close();
                            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
                            mainWin.User.Content = "Logged as ";
                            mainWin.User.Content += txtUsername.Text;
                            string login = "'" + txtUsername.Text + "'";
                            string positionIdQuery = "SELECT position_id FROM tb_workers WHERE tb_workers.login = " + login + ";";
                            AdapterSQL.SelectCommand = new MySqlCommand(positionIdQuery, mysql.databaseCon);
                            DataTable id = new DataTable();
                            AdapterSQL.Fill(id);
                            MainWindow.employeePosition = id.Rows[0][0].ToString();

                            if (MainWindow.employeePosition.Equals("3") || MainWindow.employeePosition.Equals("4"))
                            {
                                mainWin.RegisterBtn.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                mainWin.RegisterBtn.Visibility = Visibility.Hidden;
                            }


                            break;
                        }
                    }
                    if (i > Users.Count - 3)
                        MessageBox.Show("Incorrect login or password. Try again.");
                }

                mysql.databaseCon.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void ExitProgram(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to close the application?", "Warning", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}