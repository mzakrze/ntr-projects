using calendar_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        public Appointment Post([FromBody]Appointment postedAppointment)
        {
            Appointment appointment;
            if (ModelState.IsValid)
            {
                using (var db = new Storage())
                {
                    appointment = new Appointment
                    {
                        Title = postedAppointment.Title,
                        Description = postedAppointment.Description,
                        AppointmentDate = postedAppointment.AppointmentDate,
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
        public Appointment Put(string id, [FromBody]Appointment postedAppointment)
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
                    appointment.AppointmentDate = postedAppointment.AppointmentDate;
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
