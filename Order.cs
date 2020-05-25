//-------------------------------------PRZECHOWUJE INFORMACJE O ZAMOWIENIU-------------------------------------------

using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace SystemRealizacjiZamowien
{
    internal class Order
    {
        public static double price;
        public static double total;
        public static double sub;
        public static int amountOfp;
        public static List<string> productNames = new List<string>(); 
        public static List<double> productPrices = new List<double>();
        public static List<int> amountOfProd = new List<int>();
        public static string user;
        public static double userMoney;
        public static string name;
        public static double change;
        public static double totalToPay;
    }
}
