using System;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Text;
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

        private Appointment currentAppointment;
        private Guid _appointmentId;
        public Guid AppointmentId
        {
            get { return _appointmentId; }
            set
            {
                _appointmentId = value;
                using(var db = new StorageContext())
                {
                    Appointment a = db.Appointments.Find(_appointmentId);
                    currentAppointment = a;
                    Name = a.Title;
                    Date = a.AppointmentDate.ToString(@"dd\/MM\/yyyy");
                    BeginTime = a.StartTime.ToString("HH:mm");
                    EndTime = a.EndTime.ToString("HH:mm");
                }
                PropertyChanged(this, new PropertyChangedEventArgs("AppointmentId"));
            }
        }

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

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string this[string columnName]
        {
            get
            {
                log.Debug("Validating form ...");
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
            Boolean can = DateValid && BeginTimeValid && EndTimeValid && NameValid;
            if (can)
            {
                log.Debug("SaveCommandExecute command is cant be executed");
            } else
            {
                log.Debug("SaveCommandExecute command can be executed");
            }
            
           return can;
        }

        private void SaveCommandExecute(object parameter)
        {
            log.Debug("In SaveCommand...");
            try
            {
                Boolean closeWin = true;
                Appointment Ev = GetLastEditedEventForm();
                Boolean add = false;
                if (Ev.AppointmentId == null)
                {
                    Ev.AppointmentId = Guid.NewGuid();
                    add = true;
                }
                using (var ctx = new StorageContext())
                {
                    if (add)
                    {
                        ctx.Appointments.Add(Ev);
                        ctx.SaveChanges();
                    } else
                    {
                        ctx.Database.BeginTransaction();

                        Appointment original = ctx.Appointments.Find(currentAppointment.AppointmentId);
                        original.AppointmentDate = DateTime.Now;
                        Boolean eq = AreEauql(original, currentAppointment);
                        if (eq)
                        {
                            original.Title = Ev.Title;
                            original.EndTime = Ev.EndTime;
                            original.StartTime = Ev.StartTime;
                            ctx.SaveChanges();
                            ctx.Database.CurrentTransaction.Commit();
                        } else
                        {
                            log.Info("Appointment Has changed");
                            MessageBoxResult result = MessageBox.Show("Ups, someone else edited this before you. I'll fill present data to form", "Concurrency", MessageBoxButton.OK, MessageBoxImage.Question);
                            Name = original.Title;
                            BeginTime = original.StartTime.ToString("HH:MM");
                            EndTime = original.EndTime.ToString("HH:MM");
                            closeWin = false;
                        }
                    }
                    
                }
                if (closeWin)
                {
                    Window win = parameter as Window;
                    win.Close();
                }
                log.Debug("SaveCommand succeded");
            } catch(Exception e)
            {
                log.Error("SaveCommand Failed because:" + e.GetType().FullName);
            }
            
        }

        Boolean AreEauql(Appointment a1, Appointment a2)
        {
            if (a1.AppointmentDate.CompareTo(a2.AppointmentDate) != 0)
                return false;
            if (a1.EndTime.CompareTo(a2.EndTime) != 0)
                return false;
            if (a1.StartTime.CompareTo(a2.StartTime) != 0)
                return false;
            if (a1.Title.Equals(a2.Title) == false)
                return false;
            return true;
        }

        private void CancelCommandExecute(object parameter)
        {
            Window win = parameter as Window;
            win.Close();
        }

        private bool CanDeleteCommandExecute()
        {
            return Guid.Empty.Equals(AppointmentId) == false;
        }

        private void DeleteCommandExecute(object parameter)
        {
            log.Debug("In DeleteCommand...");
            try
            {
                using (var db = new StorageContext())
                {
                    Guid guid = AppointmentId;
                    var original = db.Appointments.Find(guid);
                    if (original != null /* && wersja ta sama */)
                    {
                        db.Appointments.Remove(original);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception E)
                        {
                            log.Error("Cant delete Appointment : " + E);
                            throw E;
                        }
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Ups, someone else deleted this before you", "Concurrency", MessageBoxButton.OK, MessageBoxImage.Question);
                    }
                }
                Window win = parameter as Window;
                win.Close();
                log.Debug("DeleteCommand succeded");
            } catch(Exception e)
            {
                log.Error("DeleteCommand failed because: " + e.GetType().FullName);
            }
        }

        private Appointment GetLastEditedEventForm()
        {

            Guid guid = AppointmentId;
            
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
            return new Appointment() { AppointmentId = guid, AppointmentDate = date, Title = Name, StartTime = beginTime, EndTime = endTime };
        }
    }
}
