using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp2.ViewModel
{
    public class SetPreferencesWindowViewModel : INotifyPropertyChanged
    {
        private string _font = "Arial";
        public object Font
        {
            get { return _font; }
            set {
                var comboBoxItem = (value as ComboBoxItem);
                if(comboBoxItem == null) { _font = (value as String); }
                else { _font = comboBoxItem.Content.ToString();  }
                
                NotifyPropertyChanged("Font");
            }
        }

        private string _color = "Green";
        public object Color
        {
            get { return _color; }
            set
            {
                var comboBoxItem = (value as ComboBoxItem);
                if (comboBoxItem == null) { _color = (value as String); }
                else { _color = comboBoxItem.Content.ToString(); }
                NotifyPropertyChanged("Color");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
