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
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Date"));
            }
        }
        public string BeginTime
        {
            get
            {
                return _beginTime;
            }
            set
            {
                _beginTime = value;
                PropertyChanged(this, new PropertyChangedEventArgs("BeginTime"));
            }
        }
        public string EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
            }
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
                Regex r = new Regex(@"^[\d]{1,2}:[\d]{1,2}$");
                Regex rd = new Regex(@"^[\d]{1,2}/[\d]{1,2}/[\d]{4}$"); 
                Error = "";
                switch (columnName)
                {
                    case "Name":
                        if(Name == null || Name == "")
                        {
                            Error = "Name cannot be empty";
                            NameValid = false;
                            CommandManager.InvalidateRequerySuggested();
                            return Error;
                        }
                        NameValid = true;
                        CommandManager.InvalidateRequerySuggested();
                        return "";
                    case "BeginTime":
                        if(BeginTime == null)
                        {
                            BeginTimeValid = false;
                            Error = "err";
                            CommandManager.InvalidateRequerySuggested();
                            return "";
                        }
                        Match m = r.Match(BeginTime);
                        if (BeginTime == "" || !m.Success)
                        {
                            Error = "err";
                            BeginTimeValid = false;
                            CommandManager.InvalidateRequerySuggested();
                            return Error;
                        }
                        BeginTimeValid = true;
                        CommandManager.InvalidateRequerySuggested();
                        return "";
                    case "EndTime":
                        if (EndTime == null)
                        {
                            EndTimeValid = false;
                            Error = "err";
                            CommandManager.InvalidateRequerySuggested();
                            return "";
                        }
                        if (EndTime == "" || !r.Match(EndTime).Success)
                        {
                            Error = "err";
                            EndTimeValid = false;
                            CommandManager.InvalidateRequerySuggested();
                            return Error;
                        }
                        EndTimeValid = true;
                        CommandManager.InvalidateRequerySuggested();
                        return "";
                    case "Date":
                        if (Date == null)
                        {
                            DateValid = false;
                            Error = "err";
                            CommandManager.InvalidateRequerySuggested();
                            return "";
                        }
                        if (Date == "" || !rd.Match(Date).Success)
                        {
                            Error = "err";
                            DateValid = false;
                            CommandManager.InvalidateRequerySuggested();
                            return Error;
                        }
                        DateValid = true;
                        CommandManager.InvalidateRequerySuggested();
                        return "";
                    default:
                        return "";
                }
            }
        }

        public ICommand SaveCommand { get { return new RelayCommand(SaveCommandExecute, CanSaveCommandExecute);  } }

        public ICommand CancelCommand { get { return new RelayCommand(CancelCommandExecute); } }

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
            DateTime date = new DateTime(Int32.Parse(str[2]), Int32.Parse(str[1]), Int32.Parse(str[0]));
            DateTime beginTime = date.AddHours(Int32.Parse(str2[1])).AddMinutes(Int32.Parse(str2[0]));
            DateTime endTime = date.AddHours(Int32.Parse(str3[1])).AddMinutes(Int32.Parse(str3[0]));
            return new Event(id, Name, date, beginTime, endTime);
        }
    }
}
