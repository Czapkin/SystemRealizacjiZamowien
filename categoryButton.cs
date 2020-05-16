using System.Windows.Controls;

//-------------------------------------DO GENEROWANIA PRZYCISKOW KATEGORII-------------------------------------------

namespace SystemRealizacjiZamowien
{
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

