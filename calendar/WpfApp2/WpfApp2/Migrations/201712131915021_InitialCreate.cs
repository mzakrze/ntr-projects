namespace WpfApp2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Guid(nullable: false),
                        Title = c.String(),
                        AppointmentDate = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AppointmentId);
            
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        AttendanceId = c.Guid(nullable: false),
                        accepted = c.Boolean(nullable: false),
                        Appointment_AppointmentId = c.Guid(),
                        Person_PersonId = c.Guid(),
                    })
                .PrimaryKey(t => t.AttendanceId)
                .ForeignKey("dbo.Appointments", t => t.Appointment_AppointmentId)
                .ForeignKey("dbo.People", t => t.Person_PersonId)
                .Index(t => t.Appointment_AppointmentId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonId = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserID = c.String(),
                    })
                .PrimaryKey(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendances", "Person_PersonId", "dbo.People");
            DropForeignKey("dbo.Attendances", "Appointment_AppointmentId", "dbo.Appointments");
            DropIndex("dbo.Attendances", new[] { "Person_PersonId" });
            DropIndex("dbo.Attendances", new[] { "Appointment_AppointmentId" });
            DropTable("dbo.People");
            DropTable("dbo.Attendances");
            DropTable("dbo.Appointments");
        }
    }
}
