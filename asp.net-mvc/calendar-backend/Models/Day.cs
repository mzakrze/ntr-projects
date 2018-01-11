using System;

namespace calendar_backend.Models
{
    public class Day
    {
        public string Date { get; set; }
        public Appointment[] Appointments { get; set; }
    }
}