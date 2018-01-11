using calendar_backend.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace calendar_backend.Services
{
    public class WeeksService
    {
        public static Week[] generateWeeks(DateTime date)
        {
            DayOfWeek dayOfWeek = date.DayOfWeek;
            int dayNo = dayOfWeek - DayOfWeek.Monday;
            DateTime day = date.AddDays(-dayNo);

            Week[] weeks = new Week[4];
            for (int i=0; i < 4; i++)
            {
                weeks[i] = WeeksService.generateWeek(day);
                day = day.AddDays(7);
            }
            return weeks;
        }

        private static Week generateWeek(DateTime day)
        {
            Week week = new Week();
            week.No = WeeksService.getWeekNo(day);
            week.Year = day.Year;
            Day[] days = new Day[7];
            for (int i = 0; i < 7; i++)
            {
                days[i] = new Day
                {
                    Date = day.ToString(),
                    Appointments = WeeksService.getDayAppointments(day)
                };
                day = day.AddDays(1);
            }
            week.Days = days;
            return week;
        }

        private static int getWeekNo(DateTime date)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            int weekNo = cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            return weekNo;
        }

        private static Appointment[] getDayAppointments(DateTime date)
        {
            Appointment[] appointment;
            using(var db = new Storage())
            {
                appointment = db.Appointment
                        .Where(a => a.AppointmentDate == date)
                        .OrderBy(a => a.StartTime)
                        .ToArray();
            }
            return appointment;
        }
    }
}