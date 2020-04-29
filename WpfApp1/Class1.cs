using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace WpfApp1
{
    
    public class Class1 : Button
    {
        public string name;
        public string price;

        public Class1(string name, string price)
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

    public class Class2 : Button
    {
        public string category;
        
        public Class2(string category)
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
