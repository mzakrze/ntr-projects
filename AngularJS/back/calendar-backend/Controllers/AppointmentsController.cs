using calendar_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Globalization;

namespace calendar_backend.Controllers
{
    public class AppointmentsController : ApiController
    {

        //private static readonly log4net.ILog log =
        //        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private static readonly log4net.ILog log =
        //    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            DateTime d1;
            if (postedAppointment.timestamp != null) // FIXME
            {
                d1 = DateTime.ParseExact(postedAppointment.AppointmentDate, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            else
            {
                d1 = DateTime.ParseExact(postedAppointment.AppointmentDate, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }

            Appointment appointment;
            if (ModelState.IsValid)
            {
                using (var db = new Storage())
                {
                    appointment = new Appointment
                    {
                        Title = postedAppointment.Title,
                        Description = postedAppointment.Description,
                        AppointmentDate = d1,
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
        public object Put(string id, [FromBody]AppointmentWrapper postedAppointment)
        {
            Appointment appointment;
            if (ModelState.IsValid)
            {
                using (var db = new Storage())
                {
                    appointment = db.Appointment.Where(a => a.AppointmentID == new Guid(id)).FirstOrDefault();
                    if(appointment == null)
                    {
                        return Json(JsonResponseFactory.ErrorResponse(null));
                    }
                    if (!appointment.timestamp.SequenceEqual(postedAppointment.timestamp))
                    {
                        Appointment concurrentChange = db.Appointment.Where(a => a.AppointmentID == new Guid(id)).First();
                        return Json(JsonResponseFactory.ErrorResponse(concurrentChange));
                    }
                    appointment.Title = postedAppointment.Title;
                    appointment.Description = postedAppointment.Description;
                    appointment.AppointmentDate = DateTime.Parse(postedAppointment.AppointmentDate, CultureInfo.InvariantCulture);
                    appointment.StartTime = postedAppointment.StartTime;
                    appointment.EndTime = postedAppointment.EndTime;
                    appointment.timestamp = postedAppointment.timestamp;
                    db.SaveChanges();
                }
                return Json(JsonResponseFactory.SuccessResponse(appointment));
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

        public class JsonResponseFactory
        {
            public static object ErrorResponse(object referenceObject)
            {
                return new { Success = false, Object = referenceObject };
            }

            public static object SuccessResponse()
            {
                return new { Success = true };
            }

            public static object SuccessResponse(object referenceObject)
            {
                return new { Success = true, Object = referenceObject };
            }

        }
    }
}
