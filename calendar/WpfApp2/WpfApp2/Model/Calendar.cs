using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace WpfApp2.Model
{
    public class Calendar
    {
        private static Calendar instance;

        private Calendar() { }

        public static Calendar Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Calendar();
                }
                return instance;
            }
        }

        public ObservableCollection<Event> Events = new ObservableCollection<Event>();
        
        private int eventNextId = 0;
        
        public void UpsertEvent(Event Ev)
        {
            if(Ev.Id == null)
            {
                Ev.Id = Interlocked.Increment(ref eventNextId);
                Events.Add(Ev);
            } else
            {
                DeleteEventById(Ev.Id);
                Events.Add(Ev);
            }
        }

        public void DeleteEventById(int? Id)
        {
            Events.Remove(new Event(Id));
        }
    }



    public class Event
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public Event(int? id)
        {
            Id = id;
        }

        public Event(int? id, string name, DateTime date, DateTime beginTime, DateTime endTime)
        {
            this.Id = id;
            this.Name = name;
            this.Date = date;
            this.BeginTime = beginTime;
            this.EndTime = endTime;
        }

        public override bool Equals(object obj)
        {
            if(obj as Event == null)
            {
                return false;
            }
            return this.Id == (obj as Event).Id;
        }

        public override int GetHashCode()
        {
            return this.Id ?? -1;
        }
    }
}
