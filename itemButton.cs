using System;
using System.Windows.Controls;

namespace SystemRealizacjiZamowien
{
    public class itemButton : Button
    {
        public string name;
        public string price;

        public itemButton(string name, string price)
        {
            this.name = name;
            this.price = price;
            setContent(this.name);
        }

        public int getPrice()
        {
            return Int32.Parse(Convert.ToString(price));
        }

        public void setContent(string name)
        {
            Content = name;
        }
    }
}
