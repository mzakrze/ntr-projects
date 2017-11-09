using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.ViewModel
{
    public class SetPreferencesWindowViewModel : INotifyPropertyChanged
    {
        private string _font = "Arial";
        public string Font
        {
            get { return _font; }
            set {
                int length = "System.Windows.Controls.ComboBoxItem: ".Length;
                _font = value.Substring(length); ;
                NotifyPropertyChanged("Font");
            }
        }

        private string _color = "Green";
        public string Color
        {
            get { return _color; }
            set { _color = value; NotifyPropertyChanged("Color"); }
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
