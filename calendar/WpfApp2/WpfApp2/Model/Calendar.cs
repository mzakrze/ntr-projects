using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Threading;

namespace WpfApp2.Model
{
    public class Attendance
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid AttendanceId { get; set; }
        public virtual Appointment Appointment { get; set; }
        public virtual Person Person { get; set; }
        public bool Accepted { get; set; }
    }

    public class Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserID { get; set; }
        public virtual List<Attendance> Attendances { get; set; }
    }

    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public string Title { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public virtual List<Attendance> Attendances { get; set; }
    }

    public class StorageContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
    }

    public class Storage
    {
        public List<Person> getPersons()
        {
            using (var db = new StorageContext())
                return db.Persons.ToList();
        }
    }
}
