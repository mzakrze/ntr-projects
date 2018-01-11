using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace calendar_backend
{
    public class AppointmentWrapper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AppointmentWrapper()
        {
        }

        public Guid AppointmentID { get; set; }


        public string Title { get; set; }

        public string Description { get; set; }

        public String AppointmentDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public byte[] timestamp { get; set; }
    }
}