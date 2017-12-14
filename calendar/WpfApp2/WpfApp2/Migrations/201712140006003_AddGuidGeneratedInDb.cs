namespace WpfApp2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGuidGeneratedInDb : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attendances", "Appointment_AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Attendances", "Person_PersonId", "dbo.People");
            DropPrimaryKey("dbo.Appointments");
            DropPrimaryKey("dbo.Attendances");
            DropPrimaryKey("dbo.People");
            AlterColumn("dbo.Appointments", "AppointmentId", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Attendances", "AttendanceId", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.People", "PersonId", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Appointments", "AppointmentId");
            AddPrimaryKey("dbo.Attendances", "AttendanceId");
            AddPrimaryKey("dbo.People", "PersonId");
            AddForeignKey("dbo.Attendances", "Appointment_AppointmentId", "dbo.Appointments", "AppointmentId");
            AddForeignKey("dbo.Attendances", "Person_PersonId", "dbo.People", "PersonId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendances", "Person_PersonId", "dbo.People");
            DropForeignKey("dbo.Attendances", "Appointment_AppointmentId", "dbo.Appointments");
            DropPrimaryKey("dbo.People");
            DropPrimaryKey("dbo.Attendances");
            DropPrimaryKey("dbo.Appointments");
            AlterColumn("dbo.People", "PersonId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Attendances", "AttendanceId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Appointments", "AppointmentId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.People", "PersonId");
            AddPrimaryKey("dbo.Attendances", "AttendanceId");
            AddPrimaryKey("dbo.Appointments", "AppointmentId");
            AddForeignKey("dbo.Attendances", "Person_PersonId", "dbo.People", "PersonId");
            AddForeignKey("dbo.Attendances", "Appointment_AppointmentId", "dbo.Appointments", "AppointmentId");
        }
    }
}
