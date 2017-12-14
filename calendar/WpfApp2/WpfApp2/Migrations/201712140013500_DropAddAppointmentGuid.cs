namespace WpfApp2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropAddAppointmentGuid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attendances", "Appointment_AppointmentId", "dbo.Appointments");
            DropPrimaryKey("dbo.Appointments");
            AlterColumn("dbo.Appointments", "AppointmentId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Appointments", "AppointmentId");
            AddForeignKey("dbo.Attendances", "Appointment_AppointmentId", "dbo.Appointments", "AppointmentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendances", "Appointment_AppointmentId", "dbo.Appointments");
            DropPrimaryKey("dbo.Appointments");
            AlterColumn("dbo.Appointments", "AppointmentId", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Appointments", "AppointmentId");
            AddForeignKey("dbo.Attendances", "Appointment_AppointmentId", "dbo.Appointments", "AppointmentId");
        }
    }
}
