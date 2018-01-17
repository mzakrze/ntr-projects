namespace calendar_backend
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Storage : DbContext
    {
        public Storage()
            : base("name=Storage")
        {
        }

        public virtual DbSet<Appointment> Appointment { get; set; }
        //public virtual DbSet<Attendance> Attendance { get; set; }
        //public virtual DbSet<Person> Person { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .Property(e => e.timestamp)
                .IsFixedLength();

            /*

            modelBuilder.Entity<Appointment>()
                .HasMany(e => e.Attendance)
                .WithRequired(e => e.Appointment)
                .WillCascadeOnDelete(false);

            
            modelBuilder.Entity<Attendance>()
                .Property(e => e.timestamp)
                .IsFixedLength();

            modelBuilder.Entity<Person>()
                .Property(e => e.timestamp)
                .IsFixedLength();

            modelBuilder.Entity<Person>()
                .HasMany(e => e.Attendance)
                .WithRequired(e => e.Person)
                .WillCascadeOnDelete(false);
            */
        }
    }
}
