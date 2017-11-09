using System;
using System.Windows;

namespace WpfApp2.View
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.MaxHeight = 900;
            this.MaxWidth = 1600;

            this.MinHeight = 299;
            this.MinWidth = 532;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }
    }
}
