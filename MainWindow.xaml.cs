using System;
using System.Collections.Generic;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Windows.Media.Imaging;

namespace SystemRealizacjiZamowien
{
    //-------------------------------------DO OBSLUGI GLOWNEGO OKIENKA, MYSQL ITD-------------------------------------------
    public partial class MainWindow : Window
    {
        public double to_Pay = 0;
        public static Window onlyInstance;
        public static Window onlyInstanceCat;
        public static string employeePosition;
        public static string loginString = "SERVER=localhost;DATABASE=db_system_realizacji_zamowien_posilkow_21042020;UID=root;PASSWORD=";

        public MainWindow()
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;
            Hide();

            //string loginString =
                //"SERVER=localhost;DATABASE=db_system_realizacji_zamowien_posilkow_21042020;UID=root;PASSWORD=";

            MySqlConnector mysql = new MySqlConnector(loginString);

            if (!mysql.isConnected())
            {
                mysql = tryToConnectAgain(mysql,loginString);
            }

            //-------------------------------------OKNO LOGOWANIA-------------------------------------------
            Login loginWindow = new Login(mysql); 
            loginWindow.Show(); 
            //-------------------------------------OKNO LOGOWANIA-------------------------------------------

            const byte numOfFoodCategories = 5;

            DataTable[] n = new DataTable[numOfFoodCategories]; 
            DataTable categories = new DataTable();

            n[0] = mysql.sendRequest("SELECT * from tb_beverages");
            n[1] = mysql.sendRequest("SELECT * from tb_desserts");
            n[2] = mysql.sendRequest("SELECT * from tb_sandwiches");
            n[3] = mysql.sendRequest("SELECT * from tb_sets");
            n[4] = mysql.sendRequest("SELECT * from tb_snacks");
            categories = mysql.sendRequest("SELECT * from tb_categories");

            var WholeDesserts = new List<string>();
            var WholeBevereges = new List<string>();
            var WholeSandwiches = new List<string>();
            var WholeSets = new List<string>();
            var WholeSnacks = new List<string>();
            var listOfCategories = new List<string>();


            var WholeProduct = new List<string>(); // znajduja sie tu wszystkie elementy z bazy danych(cena, nazwa itd)

            
            for(int l=0; l<numOfFoodCategories; ++l)
            {
                loopThroughDataT(n[l], WholeProduct);
            }

            loopThroughDataT(categories, listOfCategories);
            loopThroughDataT(n[0], WholeBevereges);
            loopThroughDataT(n[1], WholeDesserts);
            loopThroughDataT(n[2], WholeSandwiches);
            loopThroughDataT(n[3], WholeSets);
            loopThroughDataT(n[4], WholeSnacks);

            int iteratorProductID = 0;
            int iteratorFullName = 1;
            int iteratorShortNames = 2;
            int iteratorCategoriesID = 3;
            int iteratorCallories = 4;
            int iteratorPrice = 5;
            int orderCount = 6;

            var dessertsNames = parseDataFromDataTable(iteratorShortNames, 7, WholeDesserts);
            var dessertsFullNames = parseDataFromDataTable(iteratorFullName, 7, WholeDesserts);
            var dessertsCosts = parseDataFromDataTable(iteratorPrice, 7, WholeDesserts);
            
            var sandwichesNames = parseDataFromDataTable(iteratorShortNames, 7, WholeSandwiches);
            var sandwichesFullNames = parseDataFromDataTable(iteratorFullName, 7, WholeSandwiches);
            var sandwichesCosts = parseDataFromDataTable(iteratorPrice , 7, WholeSandwiches);

            var setsNames = parseDataFromDataTable(iteratorShortNames, 7, WholeSets);
            var setsFullNames = parseDataFromDataTable(iteratorFullName, 7, WholeSets);
            var setsCosts = parseDataFromDataTable(iteratorPrice, 7, WholeSets);
            
            var snacksNames = parseDataFromDataTable(iteratorShortNames, 7, WholeSnacks);
            var snacksFullNames = parseDataFromDataTable(iteratorFullName, 7, WholeSnacks);
            var snacksCosts = parseDataFromDataTable(iteratorPrice, 7, WholeSnacks);
            
            var beveregesNames = parseDataFromDataTable(iteratorShortNames, 7, WholeBevereges);
            var beveregesFullNames = parseDataFromDataTable(iteratorFullName, 7, WholeBevereges);
            var beveregesCosts = parseDataFromDataTable(iteratorPrice, 7, WholeBevereges);

            var categoriesDisplay = parseDataFromDataTable(1, 2, listOfCategories);

            this.Logout.Click += new RoutedEventHandler(
            (sendItem, args) =>
            {
                var Login = new Login(mysql);
                SystemRealizacjiZamowien.Order.user = "";
                Login.Show();
                Hide();
            });


            categoryButton[] categoryButto = new categoryButton[categoriesDisplay.Count];

            for (int q = 0; q < categoriesDisplay.Count; ++q)
            {
                categoryButto[q] = new categoryButton(categoriesDisplay[q]);
                grid.Children.Add(categoryButto[q]);
            }

            categoryButto[0].Background = Brushes.LawnGreen;
            categoryButto[0].Click += new RoutedEventHandler(
            (sendItem, args) =>
            {
                onlyInstanceCat = new Categories(CashToPay, setsNames, setsCosts, setsFullNames);
                onlyInstanceCat.Name = "Categories";
                Hide();
            });

            categoryButto[1].Background = Brushes.LawnGreen;
            categoryButto[1].Click += new RoutedEventHandler(
            (sendItem, args) =>
           {
               onlyInstanceCat = new Categories(CashToPay, beveregesNames, beveregesCosts, beveregesFullNames);
               onlyInstanceCat.Name = "Categories";
               Hide();
           });
            categoryButto[2].Background = Brushes.LawnGreen;
            categoryButto[2].Click += new RoutedEventHandler(
            (sendItem, args) =>
            {
                onlyInstanceCat = new Categories(CashToPay, sandwichesNames, sandwichesCosts, sandwichesFullNames);
                onlyInstanceCat.Name = "Categories";
                Hide();
            });
            categoryButto[3].Background = Brushes.LawnGreen;
            categoryButto[3].Click += new RoutedEventHandler(
             (sendItem, args) =>
             {
                 onlyInstanceCat = new Categories(CashToPay, snacksNames, snacksCosts, snacksFullNames);
                 onlyInstanceCat.Name = "Categories";
                 Hide();
             });

            categoryButto[4].Background = Brushes.LawnGreen;
            categoryButto[4].Click += new RoutedEventHandler(
             (sendItem, args) =>
             {
                 onlyInstanceCat = new Categories(CashToPay, dessertsNames, dessertsCosts, dessertsFullNames);
                 onlyInstanceCat.Name = "Categories";
                 Hide();
             });
        }
        public void generateTextBlock()
        {
            TextBlock txBlock = new TextBlock();
        }

        //-------------------------------------DO OBSLUGI FINALIZACJI ZAMOWIENIA-------------------------------------------
        private void CompleteTheOrderClick(object sender, RoutedEventArgs e)
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            var cashToPay = Convert.ToDouble(mainWin.CashToPay.Content);

            try
            {
                if (cashToPay <= 0)
                {
                    throw new ArgumentException();
                }

                onlyInstance = new ChoosePaymentMethod();
                onlyInstance.Name = "ChoosePaymentMethod";
                onlyInstance.Show();
                Hide();
            }
            catch (ArgumentException)
            {
                MessageBox.Show("At least one product must be selected");
            }
        }

        //-------------------------------------WYJSCIE Z PROGRAMU-------------------------------------------
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

        //-------------------------------------PARSING DANYCH DO INDYWIDUALYCH LIST-------------------------------------------
        public List<string> parseDataFromDataTable(int indexOfProdInDB, int iterator, List<string> WholeProducts)
        {
            List<string> temporaryArr = new List<string>();

            for (; indexOfProdInDB <= WholeProducts.Count; indexOfProdInDB += iterator)
            {
                temporaryArr.Add((WholeProducts[indexOfProdInDB]));
            }
            return temporaryArr;
        }

        //-------------------------------------PARSING DANYCH Z BD DO LISTY-------------------------------------------
        static public void loopThroughDataT(DataTable element, List<string> lista)
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

        //-------------------------------------SPRAWDZA CZY ISTNIEJE POLACZENIE-------------------------------------------
        public MySqlConnector tryToConnectAgain(MySqlConnector mysql, string loginString) 
        {
            while (!mysql.isConnected())  
            {
                MessageBoxResult result = MessageBox.Show("Failed connecting to database\nTry again?", "ERROR"
                    , MessageBoxButton.YesNo);
                mysql = new MySqlConnector(loginString);
                switch (result)
                {
                    case MessageBoxResult.No:
                        {
                            System.Environment.Exit(0);
                            break;
                        }
                    case MessageBoxResult.Yes:
                        {
                            continue;
                        }

                }
            }
            return mysql;
        }

        private void RegisterNewEmployee(object sender, RoutedEventArgs e)
        {
            var registerWindow = new Register();
            registerWindow.Show();
            Hide();

        }
    }
}


