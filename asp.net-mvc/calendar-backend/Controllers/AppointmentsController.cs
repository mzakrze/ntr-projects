using calendar_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Globalization;

namespace calendar_backend.Controllers
{
    public class AppointmentsController : ApiController
    {
        // GET api/appointments/5
        public Appointment Get(string id)
        {
            Appointment appointment;
            using (var db = new Storage())
            {
                appointment = db.Appointment.Where(a => a.AppointmentID == new Guid(id)).First();
            }
            return appointment;
        }

        // POST api/appointments
        public Appointment Post([FromBody]AppointmentWrapper postedAppointment)
        {
            DateTime d1 = DateTime.ParseExact(postedAppointment.AppointmentDate, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            Appointment appointment;
            if (ModelState.IsValid)
            {
                using (var db = new Storage())
                {
                    appointment = new Appointment
                    {
                        Title = postedAppointment.Title,
                        Description = postedAppointment.Description,
                        AppointmentDate = d1, //DateTime.Now,  //postedAppointment.AppointmentDate,
                        StartTime = postedAppointment.StartTime,
                        EndTime = postedAppointment.EndTime
                    };
                    appointment.AppointmentID = Guid.NewGuid();
                    db.Appointment.Add(appointment);
                    db.SaveChanges();
                }
                return appointment;
            } 
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        // PUT api/appointments/5
        public Appointment Put(string id, [FromBody]AppointmentWrapper postedAppointment)
        {
            Appointment appointment;
            if (ModelState.IsValid)
            {
                using (var db = new Storage())
                {
                    appointment = db.Appointment.Where(a => a.AppointmentID == new Guid(id)).First();
                    if (!appointment.timestamp.SequenceEqual(postedAppointment.timestamp))
                    {
                        throw new HttpResponseException(HttpStatusCode.Conflict);
                    }
                    appointment.Title = postedAppointment.Title;
                    appointment.Description = postedAppointment.Description;
                    //appointment.AppointmentDate = DateTime.ParseExact(postedAppointment.AppointmentDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    appointment.AppointmentDate = DateTime.Parse(postedAppointment.AppointmentDate, CultureInfo.InvariantCulture);
                    appointment.StartTime = postedAppointment.StartTime;
                    appointment.EndTime = postedAppointment.EndTime;
                    db.SaveChanges();
                }
                return appointment;
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/appointments/5
        public void Delete(string id)
        {
            Appointment appointment;
            using (var db = new Storage())
            {
                appointment = db.Appointment.Where(a => a.AppointmentID == new Guid(id)).First();
                db.Appointment.Remove(appointment);
                db.SaveChanges();
            }
        }
    }
}
