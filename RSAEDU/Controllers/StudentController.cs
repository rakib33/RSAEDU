using RSAEDU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RSAEDU.Controllers
{
    public class StudentController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: /Student/
        public ActionResult Index()
        {

            ViewBag.ExamId = new SelectList(db.ExamInfoes.ToList().Where(t => t.Publish == "P"), "Id", "ExamName");
            ViewBag.Faculty = new SelectList(db.FacultyInfoes.ToList(), "Id", "FacultyName");

            return View();
     
        }
	}
}