using calendar_backend.Models;
using calendar_backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace calendar_backend.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(DateTime? date = null)
        {
            ViewBag.Title = "Calendar";
            if (date == null)
            {
                date = DateTime.Now.Date;
            }
            Week[] weeks = WeeksService.generateWeeks((DateTime)date);
            return View(weeks);
        }
    }
}
