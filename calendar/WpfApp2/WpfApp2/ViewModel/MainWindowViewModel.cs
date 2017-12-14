using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WpfApp2.Model;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EventView _eventView;
        private int _windowWidth = 600;
        public int WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; FontSize = value / 50; NotifyPropertyChanged("WindowWidth"); }
        }

        private int _fontSize = 12;
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; NotifyPropertyChanged("FontSize"); }
        }

        private string _fontFamily = "Times new roman";
        public string FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; NotifyPropertyChanged("FontFamily"); }
        }

        private string _themeColor = "Blue";
        public string ThemeColor
        {
            get { return _themeColor; }
            set { _themeColor = value; NotifyPropertyChanged("ThemeColor"); }
        }

        public ObservableCollection<DayCard> DaysCards { get; set; }

        public ObservableCollection<LabelWrapper> LabelWrappers { get; set; }

        public ICommand AddEventCommand { get { return new RelayCommand(AddEventCommandExecute); } }
        public ICommand EditEventCommand { get { return new RelayCommand(EditEventCommandExecute); } }
        public ICommand PrevWeekCommand { get { return new RelayCommand(PrevWeekCommandExecute); } }
        public ICommand NextWeekCommand { get { return new RelayCommand(NextWeekCommandExecute); } }
        public ICommand ShowSetPreferencesWindow { get { return new RelayCommand(ShowSetPreferencesWindowExecute); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private DateTime FirstDayInCalendar;

        private void ShowSetPreferencesWindowExecute(object parameter)
        {
            SetPreferencesWindow preferencesWindow = new SetPreferencesWindow();
            SetPreferencesWindowViewModel setPreferencesWindowViewModel = preferencesWindow.setPreferencesWindowViewModel;
            setPreferencesWindowViewModel.Font = _fontFamily;
            setPreferencesWindowViewModel.Color = _themeColor;
            setPreferencesWindowViewModel.PropertyChanged += PreferencesChanged;
            preferencesWindow.Show();
        }

        private void PreferencesChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Font")
            {
                string font = ((sender as SetPreferencesWindowViewModel).Font as String);
                this.FontFamily = font;
            }
            else if (e.PropertyName == "Color")
            {
                string color = ((sender as SetPreferencesWindowViewModel).Color as String);
                this.ThemeColor = color;
            }
        }

        private void PrevWeekCommandExecute(object parameter)
        {
            this.FirstDayInCalendar = this.FirstDayInCalendar.AddDays(-7);
            this.FillCallendar();
        }

        private void NextWeekCommandExecute(object parameter)
        {
            this.FirstDayInCalendar = this.FirstDayInCalendar.AddDays(7);
            this.FillCallendar();
        }

        private void EditEventCommandExecute(object parameter)
        {
            Guid appointmentGuid = //new Guid(parameter as string);
                (Guid) parameter;
            EventView eventView = new EventView();
            EventViewModel eventViewModel = eventView.eventViewModel;
            eventViewModel.AppointmentId = appointmentGuid;
            eventView.Show();
        }

        private void AddEventCommandExecute(object parameter)
        {
            log.Info("In AddEventCommand ...");
            if (_eventView == null || !_eventView.IsActive)
            {
                _eventView = new EventView();
                EventViewModel eventViewModel = _eventView.eventViewModel;
                DateTime dateTime = (DateTime)parameter;
                eventViewModel.Date = dateTime.ToString(@"dd\/MM\/yyyy");
                _eventView.ShowDialog();
            }
        }

        private void FillCallendar()
        {
            DateTime dateTime = this.FirstDayInCalendar.AddDays(0); //copy
            DaysCards.Clear();
            for (int row = 1; row <= 4; row++)
            {
                for (int column = 1; column <= 7; column++)
                {
                    DayCard dayCard = new DayCard(dateTime, dateTime.ToString("MMMM dd"), row, column);
                    DaysCards.Add(dayCard);
                    dateTime = dateTime.AddDays(1);
                }
            }

            int week = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(FirstDayInCalendar, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            int year = FirstDayInCalendar.Year;

            LabelWrappers.Clear();
            LabelWrappers.Add(new LabelWrapper("w" + week + "\n" + year, 1, 0));
            LabelWrappers.Add(new LabelWrapper("w" + week + "\n" + year, 1, 8));
            LabelWrappers.Add(new LabelWrapper("w" + (week + 1) + "\n" + year, 2, 0));
            LabelWrappers.Add(new LabelWrapper("w" + (week + 1) + "\n" + year, 2, 8));
            LabelWrappers.Add(new LabelWrapper("w" + (week + 2) + "\n" + year, 3, 0));
            LabelWrappers.Add(new LabelWrapper("w" + (week + 2) + "\n" + year, 3, 8));
            LabelWrappers.Add(new LabelWrapper("w" + (week + 3) + "\n" + year, 4, 0));
            LabelWrappers.Add(new LabelWrapper("w" + (week + 3) + "\n" + year, 4, 8));

            FillCallendarDaysCards();
        }

        public MainWindowViewModel()
        {
            DaysCards = new ObservableCollection<DayCard>();

            this.FirstDayInCalendar = DateTime.Now;
            while (this.FirstDayInCalendar.DayOfWeek != DayOfWeek.Monday)
            {
                this.FirstDayInCalendar = this.FirstDayInCalendar.AddDays(-1);
            }

            LabelWrappers = new ObservableCollection<LabelWrapper>();

            this.FillCallendar();
        }

        private void FillCallendarDaysCards()
        {
            foreach (DayCard card in DaysCards)
            {
                card.Appointments.Clear();
            }
            AddEventsToCalendar();
        }

        private void AddEventsToCalendar()
        {
            log.Debug("In AddEventsToCalendar...");
            int counter = 0;
            try
            {
                using (var ctx = new StorageContext())
                {
                    foreach (Appointment a in ctx.Appointments)
                    {
                        if (a.AppointmentDate.DayOfYear - FirstDayInCalendar.DayOfYear < 0 || a.AppointmentDate.DayOfYear - FirstDayInCalendar.DayOfYear > 28)
                        {
                            return;
                        }
                        counter++;
                        int column = (int)a.AppointmentDate.DayOfWeek;
                        if (column == 0) column = 7;
                        int row = (a.AppointmentDate.DayOfYear - FirstDayInCalendar.DayOfYear) / 7 + 1;
                        this.DaysCards.First((e) => e.Row == row && column == e.Column).Appointments.Add(a);
                        log.Info("fetched appointment from db: " + a.AppointmentId + ", " + a.AppointmentDate);
                    }
                }
                log.Debug("added to calendar " + counter + " events");
            }
            catch (Exception ex)
            {
                log.Error("Exception in AddEventsToCalender: " + ex.GetType().FullName);
            }
        }

        private void RemoveEventsFromCalendar()
        {
            foreach (DayCard d in DaysCards)
            {
                d.Appointments.Clear();
            }
        }

        private void MyHandleEventsChangedFunction(object sender, NotifyCollectionChangedEventArgs s)
        {
            if (s.NewItems != null)
            {
                this.AddEventsToCalendar();
            }
            if (s.OldItems != null)
            {
                this.RemoveEventsFromCalendar();
            }
        }
    }

    public class DayCardTemplateSelection : System.Windows.Controls.DataTemplateSelector
    {
        public DataTemplate LabelWrapperTemplate { get; set; }
        public DataTemplate CalendarCardTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item as DayCard != null)
            {
                return CalendarCardTemplate;
            }
            return LabelWrapperTemplate;
        }
    }

    public class ViewEventIdConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type type, object o, System.Globalization.CultureInfo cultureInfo)
        {
            if(1== 1)
            {
                return "change plz";
            }
            String a = values[0] as string;
            String b = values[1] as string;
            String c = values[2] as string;
            String d = values[3] as string;
            String e = values[4] as string;

            if (1 == 1)
                return null;

            Guid guid = new Guid(values[0] as string);
            string name = values[1] as string;
            DateTime date = (DateTime)values[2];
            DateTime beginTime = (DateTime)values[3];
            DateTime endTime = (DateTime)values[4];
            /*
            Guid guid = Guid.NewGuid();
            string name = "testname";
            DateTime date = DateTime.Now;
            DateTime beginTime = DateTime.Now.AddHours(3);
            DateTime endTime = DateTime.Now.AddHours(5);
            */
            //return new Appointment() { AppointmentId = guid, Title = name, AppointmentDate = date, StartTime = beginTime, EndTime = endTime };
            return "xd";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class LabelWrapper
    {
        public String Text { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public LabelWrapper(string text, int row, int column)
        {
            this.Text = text;
            this.Row = row;
            this.Column = column;
        }
    }

    public class DayCard
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public int Row { get; set; } /*TODO - zamienić na week, dayEnum, albo w widoku w code behind jakoś rzutować */
        public int Column { get; set; }
        public ObservableCollection<Appointment> Appointments { get; set; } = new ObservableCollection<Appointment>();

        public DayCard(DateTime dateTime, string name, int row, int column)
        {
            this.Date = dateTime;
            this.Name = name;
            this.Row = row;
            this.Column = column;
            this.Appointments.Clear();
            //Calendar.Instance.Events.Where(e => e.Date.Equals(dateTime)).ToList().ForEach(e => this.Events.Add(e));
        }

        public override string ToString()
        {
            return Date.ToString();
        }
    }

    public class DontChangeGuidConverted : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return value.ToString();
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsTodayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime)
            {
                return ((DateTime)value).Date.Equals(DateTime.Now.Date);
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
