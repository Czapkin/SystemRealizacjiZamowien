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

            var wholeNames = new List<string>();
            var wholePrices = new List<string>();
            var wholeShort = new List<string>();


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


            wholeNames.AddRange(parseDataFromDataTable(iteratorFullName, 7, WholeBevereges));
            wholeNames.AddRange(parseDataFromDataTable(iteratorFullName, 7, WholeDesserts));
            wholeNames.AddRange(parseDataFromDataTable(iteratorFullName, 7, WholeSandwiches));
            wholeNames.AddRange(parseDataFromDataTable(iteratorFullName, 7, WholeSets));
            wholeNames.AddRange(parseDataFromDataTable(iteratorFullName, 7, WholeSnacks));

            wholePrices.AddRange(parseDataFromDataTable(iteratorPrice, 7, WholeBevereges));
            wholePrices.AddRange(parseDataFromDataTable(iteratorPrice, 7, WholeDesserts));
            wholePrices.AddRange(parseDataFromDataTable(iteratorPrice, 7, WholeSandwiches));
            wholePrices.AddRange(parseDataFromDataTable(iteratorPrice, 7, WholeSets));
            wholePrices.AddRange(parseDataFromDataTable(iteratorPrice, 7, WholeSnacks));

            wholeShort.AddRange(parseDataFromDataTable(iteratorShortNames, 7, WholeBevereges));
            wholeShort.AddRange(parseDataFromDataTable(iteratorShortNames, 7, WholeDesserts));
            wholeShort.AddRange(parseDataFromDataTable(iteratorShortNames, 7, WholeSandwiches));
            wholeShort.AddRange(parseDataFromDataTable(iteratorShortNames, 7, WholeSets));
            wholeShort.AddRange(parseDataFromDataTable(iteratorShortNames, 7, WholeSnacks));



            var categoriesDisplay = parseDataFromDataTable(1, 2, listOfCategories);

            this.Logout.Click += new RoutedEventHandler(
            (sendItem, args) =>
            {
                var Login = new Login(mysql);
                SystemRealizacjiZamowien.Order.user = "";
                if (SystemRealizacjiZamowien.Order.productNames.Count > 0)
                {
                    ResetEverything();
                }
                Login.Show();
                Hide();
            });


            this.Remove.Background = Brushes.Tomato;
            this.Remove.Click += new RoutedEventHandler(
            (sendItem, args) =>
            {
                if (SystemRealizacjiZamowien.Order.productNames.Count > 0)
                {
                    MessageBoxResult result = MessageBox.Show("Do you really want to reset the order?", "Warning", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                        ResetEverything();
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
            });

            categoryButton[] categoryButto = new categoryButton[categoriesDisplay.Count];

            for (int q = 0; q < categoriesDisplay.Count; ++q)
            {
                categoryButto[q] = new categoryButton(categoriesDisplay[q]);
                grid.Children.Add(categoryButto[q]);
            }

            categoryButto[0].Background = Brushes.SeaGreen;
            categoryButto[0].Foreground = Brushes.White;
            categoryButto[0].Click += new RoutedEventHandler(
            (sendItem, args) =>
            {
                onlyInstanceCat = new Categories(CashToPay, setsNames, setsCosts, setsFullNames,false);
                onlyInstanceCat.Name = "Categories";
                Hide();
            });

            categoryButto[1].Background = Brushes.SeaGreen;
            categoryButto[1].Foreground = Brushes.White;
            categoryButto[1].Click += new RoutedEventHandler(
            (sendItem, args) =>
           {
               onlyInstanceCat = new Categories(CashToPay, beveregesNames, beveregesCosts, beveregesFullNames,false);
               onlyInstanceCat.Name = "Categories";
               Hide();
           });
            categoryButto[2].Background = Brushes.SeaGreen;
            categoryButto[2].Foreground = Brushes.White;
            categoryButto[2].Click += new RoutedEventHandler(
            (sendItem, args) =>
            {
                onlyInstanceCat = new Categories(CashToPay, sandwichesNames, sandwichesCosts, sandwichesFullNames,false);
                onlyInstanceCat.Name = "Categories";
                Hide();
            });
            categoryButto[3].Background = Brushes.SeaGreen;
            categoryButto[3].Foreground = Brushes.White;
            categoryButto[3].Click += new RoutedEventHandler(
             (sendItem, args) =>
             {
                 onlyInstanceCat = new Categories(CashToPay, snacksNames, snacksCosts, snacksFullNames,false);
                 onlyInstanceCat.Name = "Categories";
                 Hide();
             });

            categoryButto[4].Background = Brushes.SeaGreen;
            categoryButto[4].Foreground = Brushes.White;
            categoryButto[4].Click += new RoutedEventHandler(
             (sendItem, args) =>
             {
                 onlyInstanceCat = new Categories(CashToPay, dessertsNames, dessertsCosts, dessertsFullNames,false);
                 onlyInstanceCat.Name = "Categories";
                 Hide();
             });


            this.Find.Background = Brushes.Teal;
            this.Find.Click += new RoutedEventHandler(
            (sendItem, args) =>
            {
                if (this.SearchFor.Text != "")
                {
                    onlyInstanceCat = new Categories(CashToPay, wholeNames, wholePrices, wholeShort, true);
                    onlyInstanceCat.Name = "Categories";
                    Hide();
                }

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

        public static void ResetEverything()
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

            SystemRealizacjiZamowien.Order.totalToPay = 0;
            SystemRealizacjiZamowien.Order.productNames.Clear();
            SystemRealizacjiZamowien.Order.amountOfProd.Clear();
            SystemRealizacjiZamowien.Order.productPrices.Clear();
            SystemRealizacjiZamowien.Order.accounts.Clear();

            //SystemRealizacjiZamowien.Order.productNames.RemoveAt(SystemRealizacjiZamowien.Order.productNames.Count - 1);
            //SystemRealizacjiZamowien.Order.amountOfProd.RemoveAt(SystemRealizacjiZamowien.Order.amountOfProd.Count - 1);
            //SystemRealizacjiZamowien.Order.productPrices.RemoveAt(SystemRealizacjiZamowien.Order.productPrices.Count - 1);



            mainWin.CashToPay.Content = "0,00";
            mainWin.CurrentOrder.Content = "";

            SystemRealizacjiZamowien.Order.total = 0;
            SystemRealizacjiZamowien.Order.totalToPay = 0;
            SystemRealizacjiZamowien.Order.sub = 0;

        }

        private void RegisterNewEmployee(object sender, RoutedEventArgs e)
        {
            var registerWindow = new Register();
            registerWindow.Show();
            Hide();

        }
    }
}


