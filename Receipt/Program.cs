using System;
using System.IO;
using SystemRealizacjiZamowien;
using System.Data;
using System.Windows.Documents;
using System.Collections.Generic;

namespace Paragon
{
    class Program
    {
        private static byte nr_paragonu = 0;

        static public void logging(bool cashorcard)
        {
            Worker prac1 = new Worker(1, Order.user);

            Order1 transaction = new Order1(Order.name, Order.price, Order.amountOfp);                                                                           // (nazwa, cena, ilosc)

            DataTable[] PersonalInfo = new DataTable[2];
            MySqlConnector mysql = new MySqlConnector(MainWindow.loginString);

            PersonalInfo[0] = mysql.sendRequest("SELECT firstName FROM tb_workers WHERE login = " + "'"+Order.user+"'");
            //PersonalInfo[0] = mysql.sendRequest("SELECT secondName FROM tb_workers WHERE login = " + "'" + Order.user + "'");

            List<string> FirstName = new List<string>();
            //List<string> SecondName = new List<string>();


            MainWindow.loopThroughDataT(PersonalInfo[0], FirstName);
            //MainWindow.loopThroughDataT(PersonalInfo[1], SecondName);

            string[] names = { transaction.getName};
            double[] prices = { transaction.getPrice};
            int[] amounts = { transaction.getAmount};

            double total_price = 0;
            for (int i = 0; i < prices.Length; i++)
            {
                total_price += prices[i] * amounts[i];
            }
            total_price = Math.Round(total_price, 2);

            CustomerCash customer = new CustomerCash(Order.totalToPay);

            string filePath = Path.Combine (Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Receipts");              // sciezka do folderu z paragonami
            string nameFile = string.Format("Receipt-{0:yyyy-MM-dd_hh-mm-ss-tt}.txt", DateTime.Now);                                // nazwa pliku
            string completeFilePath = Path.Combine(filePath, nameFile);
            
            if (!Directory.Exists(filePath))                                                                                        // tworzy folder jezeli nie istnieje
                    Directory.CreateDirectory(filePath);

            using (StreamWriter sw = new StreamWriter(completeFilePath))                                                            // tworzy plik .txt
            {
                sw.WriteLine("     FIRMA sp. z o.o.");
                sw.WriteLine("   65-246  Zielona Góra");
                sw.WriteLine("      ul. Podgórna 50");
                sw.WriteLine("    NIP: 012-345-67-89\n");

                sw.WriteLine(String.Format("{0,-20:yyyy-MM-dd} {0,5:t}", DateTime.Now));                                            // data i godzina na paragonie
                sw.WriteLine("#{0,-4:D3} {1,18}", "Kasjer", FirstName[0]);                                                      // pracownik [id      nazwa]

                sw.WriteLine(" - - - - - - - - - - - - -");
                sw.WriteLine("     PARAGON FISKALNY");
                sw.WriteLine(" - - - - - - - - - - - - -");


                for (int ctr = 0; ctr < Order.productPrices.Count; ctr++)
                {
                    sw.WriteLine("{0,-11}\n{1,26:0.00}", Order.productNames[ctr], Order.amountOfProd[ctr] + "x " + (Order.productPrices[ctr] / Order.amountOfProd[ctr]).ToString()/*prices[ctr] * amounts[ctr]*/);
                }


                sw.WriteLine(" - - - - - - - - - - - - -");
                sw.WriteLine("{0,-5} {1,20:0.00}", "SUMA:", "PLN " + Order.totalToPay);                                     
                sw.WriteLine(" - - - - - - - - - - - - -");

                if (cashorcard)                                                                                                   
                {
                    sw.WriteLine("{0,-10} {1,15:0.00}", "Gotówka:", Order.userMoney);         
                    sw.WriteLine("{0,-10} {1,15:0.00}", "Reszta:", Order.change);                         
                }
                else
                {
                    sw.WriteLine("{0,-14} {1,11:0.00}", "Płatność kartą", Order.totalToPay);
                }
                                                                        
                sw.WriteLine("\n{0,-7} {1,13} {2:D4}", "P.fisk.", "Nr", nr_paragonu);// numer paragonu  
                sw.WriteLine("       AFN 12345678");
                sw.WriteLine("   ZAPRASZAMY PONOWNIE");
                sw.Close();
                nr_paragonu++;
            }

            Order.amountOfProd.Clear();
            Order.productPrices.Clear();
            Order.productNames.Clear();


        }
        class Order1    // zamowienie
        {
            string name;
            double price;
            int amount;

            public string getName => name;
            public double getPrice => price;
            public int getAmount => amount;

            public Order1(string name, double price, int amount)
            {
                this.name = name;
                this.price = price;
                this.amount = amount;
            }
        }

        class Worker     // kasjer
        {
            int id;
            string name;

            public int getId => id;
            public string getName => name;

            public Worker(int id, string name)
            {
                this.id = id;
                this.name = name;
            }
        }

        class CustomerCash     // gotowka od klienta
        {
            double cash;

            public double getCash => cash;
            public double setCash { set => cash = value; }

            public CustomerCash(double cash)
            {
                this.cash = cash;
            }
        }
    }
}