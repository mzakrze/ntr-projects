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

                    DateTime d = new DateTime(2017, 11, 7);
                    Event Ev;
                    Ev = new Event(null, "n1", d, d.AddHours(1), d.AddHours(2));
                    instance.UpsertEvent(Ev);
                    /*
                    Ev = new Event(null, "n2", new DateTime(2017, 11, 4), "12:12", "13:13");
                    instance.UpsertEvent(Ev);
                    Ev = new Event(null, "n3", new DateTime(2017, 11, 5), "12:12", "13:13");
                    instance.UpsertEvent(Ev);
                    Ev = new Event(null, "n4", new DateTime(2017, 11, 5), "12:12", "13:13");
                    instance.UpsertEvent(Ev);
                    Ev = new Event(null, "n5", new DateTime(2017, 11, 5), "12:12", "13:13");
                    instance.UpsertEvent(Ev);
                    */
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
                int index = Events.IndexOf(Ev);
                Events.RemoveAt(index);
                Events.Add(Ev);
            }
            
        }

        public void DeleteEventById(int Id)
        {
            // TODO
        }
    }



    public class Event
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

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
