using System;
using System.Windows.Controls;

//-------------------------------------DO GENEROWANIA PRZYCISKOW PRODUKTOW-------------------------------------------

namespace SystemRealizacjiZamowien
{
    public class itemButton : Button
    {
        public string name;
        public string price;
        public string fullName;

        public itemButton(string name, string price, string fullName)
        {
            this.name = name;
            this.price = price;
            this.fullName = fullName;
            setContent(this.name);
        }

        public double getPrice()
        {
            //return Double.Parse(Convert.ToString(price));
            return Double.Parse(price);
        }

        public void setContent(string name)
        {
            Content = name;
        }
    }
}
