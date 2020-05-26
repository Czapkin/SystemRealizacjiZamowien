using System;
using System.IO;
using SystemRealizacjiZamowien;

namespace WpfApp1.Logs
{
    class Logs
    {
        public static void logi(bool cashorcard)
        {

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Logs");              // sciezka do folderu z logami
            string nameFile = string.Format("{0:MMMM-yyyy}.log", DateTime.Now);                                                // nazwa pliku
            string completeFilePath = Path.Combine(filePath, nameFile);

            if (!Directory.Exists(filePath))                                                                                        // tworzy folder jezeli nie istnieje
                Directory.CreateDirectory(filePath);


            StreamWriter sw;
            sw = File.AppendText(completeFilePath);
            sw.Write("[{0:dd-MMM-yyyy hh:mm:ss tt}]", DateTime.Now);
            sw.Write(" -- " + Order.user + " -- ");
            sw.Write("[");

            for (int ctr = 0; ctr < Order.productNames.Count; ctr++)
            {
                sw.Write(Order.productNames[ctr] + " " + Order.amountOfProd[ctr] + "x " + Order.price + ", " );
            }

            if (cashorcard)
            {
                sw.Write("] -- [Payment: Cash] -- ");
                sw.Write("[AmountToPay: " + Order.totalToPay + "] -- ");
                sw.Write("[CustomerCash: " + Order.userMoney + "] -- ");
                sw.Write("[Change: " + Order.change + "] ");
            }
            else
            {
                sw.Write("] -- [Payment: Card] -- ");
                sw.Write("[AmountToPay: " + Order.totalToPay + "] ");
            }

            sw.WriteLine();
            sw.Close();
        }
    }
}
