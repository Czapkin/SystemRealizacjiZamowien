using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace WpfApp1
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

    public class categoryButton : Button
    {
        public string category;

        public categoryButton(string category)
        {
            this.category = category;
            setContent(this.category);
        }
        public void setContent(string name)
        {
            Content = name;
        }

    }


}
