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

        public void setContent(string name)
        {
            Content = name;
        }
    }
}
