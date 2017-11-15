using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Model;

namespace WpfApp2.ViewModel
{
    public class EventViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        private string _name;
        private string _date;
        private string _beginTime;
        private string _endTime;

        public string Id { get; set; }
        public string Name
        {
            get { return _name; }
            set { _name = value; PropertyChanged(this, new PropertyChangedEventArgs("Name")); }
        }
        public string Date
        {
            get { return _date; }
            set { _date = value; PropertyChanged(this, new PropertyChangedEventArgs("Date")); }
        }
        public string BeginTime
        {
            get { return _beginTime; }
            set { _beginTime = value; PropertyChanged(this, new PropertyChangedEventArgs("BeginTime")); }
        }
        public string EndTime
        {
            get { return _endTime; }
            set { _endTime = value; PropertyChanged(this, new PropertyChangedEventArgs("EndTime")); }
        }

        private Boolean DateValid = false;
        private bool BeginTimeValid = false;
        private bool EndTimeValid = false;
        private bool NameValid = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Error { get; set; }

        public string this[string columnName]
        {
            get
            {
                Error = "";
                switch (columnName)
                {
                    case "Name":
                        if(Name == null || Name == "")
                        {
                            Error = "Name cannot be empty";
                            NameValid = false;
                        }
                        else
                        {
                            NameValid = true;
                        }
                        break;
                    case "BeginTime":
                    case "EndTime":
                        DateTime? b = ValidateTime(BeginTime);
                        DateTime? e = ValidateTime(EndTime);
                        bool comparationResult = true;
                        if(b != null && e != null && DateTime.Compare((DateTime)b, (DateTime)e)  == 1)
                        {
                            comparationResult = false;
                        }
                        if(columnName == "BeginTime")
                        {
                            if (b == null || !comparationResult)
                            {
                                BeginTimeValid = false;
                                Error = "Invalid format";
                            }
                            else
                            {
                                BeginTimeValid = true;
                            }
                        } else
                        {
                            if (e == null || !comparationResult)
                            {
                                EndTimeValid = false;
                                Error = "Invalid format";
                            }
                            else
                            {
                                EndTimeValid = true;
                            }
                        }
                        break;
                    case "Date":
                        Regex rd = new Regex(@"^[\d]{1,2}/[\d]{1,2}/[\d]{4}$"); 
                        if (Date == null || Date == "" || !rd.Match(Date).Success)
                        {
                            DateValid = false;
                            Error = "invalid format";
                            break;
                        }
                        else
                        {
                            DateValid = true;
                        }
                        break;
                }
                CommandManager.InvalidateRequerySuggested();
                return Error;
            }
        }

        private void DoValidate(DateTime? b, ref bool beginTimeValid, ref string Error)
        {
            throw new NotImplementedException();
        }

        private DateTime? ValidateTime(string time)
        {
            if(time == null || !new Regex(@"^[\d]{1,2}:[\d]{1,2}$").Match(time).Success)
            {
                return null;
            }
            string[] split = time.Split(':');
            int h = Int32.Parse(split[0]);
            int m = Int32.Parse(split[1]);
            if (0 <= h && h < 24 && 0 <= m && m < 60)
            {
                return DateTime.Now.AddHours(h).AddMinutes(m);
            } else
            {
                return null;
            }
        }

        public ICommand SaveCommand { get { return new RelayCommand(SaveCommandExecute, CanSaveCommandExecute);  } }
        public ICommand CancelCommand { get { return new RelayCommand(CancelCommandExecute); } }
        public ICommand DeleteCommand { get { return new RelayCommand(DeleteCommandExecute, CanDeleteCommandExecute); } }

        private bool CanSaveCommandExecute()
        {
           return DateValid && BeginTimeValid && EndTimeValid && NameValid;
        }

        private void SaveCommandExecute(object parameter)
        {
            Event Ev = GetLastEditedEventForm();
            Calendar.Instance.UpsertEvent(Ev);
            Window win = parameter as Window;
            win.Close();
        }

        private void CancelCommandExecute(object parameter)
        {
            Window win = parameter as Window;
            win.Close();
        }

        private bool CanDeleteCommandExecute()
        {
            return Id != null;
        }

        private void DeleteCommandExecute(object parameter)
        {
            Calendar.Instance.DeleteEventById(Int32.Parse(Id));
            Window win = parameter as Window;
            win.Close();
        }


        private Event GetLastEditedEventForm()
        {
            int? id = null;
            if (Id != null)
            {
                id = Int32.Parse(Id);
            }
            string[] str = _date.Split('/');
            string[] str2 = _beginTime.Split(':');
            string[] str3 = _endTime.Split(':');
            int dateYear = Int32.Parse(str[2]);
            int dateMth = Int32.Parse(str[1]);
            int dateDay= Int32.Parse(str[0]);

            int beginTimeH = Int32.Parse(str2[0]);
            int beginTimeM = Int32.Parse(str2[1]);

            int endTimeH = Int32.Parse(str3[0]);
            int endTimeM = Int32.Parse(str3[1]);

            DateTime date = new DateTime(dateYear, dateMth, dateDay);
            DateTime beginTime = date.AddHours(beginTimeH).AddMinutes(beginTimeM);
            DateTime endTime = date.AddHours(endTimeH).AddMinutes(endTimeM);
            return new Event(id, Name, date, beginTime, endTime);
        }
    }
}
