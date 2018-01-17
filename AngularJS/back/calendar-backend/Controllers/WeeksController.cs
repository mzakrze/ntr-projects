using calendar_backend.Models;
using calendar_backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace calendar_backend.Controllers
{
    public class WeeksController : ApiController
    {
        // GET api/weeks
        public Week[] Get(DateTime? date = null)
        {
            if(date == null)
            {
                date = DateTime.Now.Date;
            }
            return WeeksService.generateWeeks((DateTime)date);
        }
    }
}
