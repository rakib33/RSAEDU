using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RSAEDU.Models;

namespace RSAEDU.Controllers
{
    public class SubjectInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SubjectInfo/
        public ActionResult Index()
        {
            try
            {
                ViewBag.ok = Convert.ToString(TempData["ok"]);
                ViewBag.message = Convert.ToString(TempData["message"]);

                return View(db.SubjectInfoes.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Server Error.err " + ex.Message;
                return View("Error");
            }
        }

        // GET: /SubjectInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectInfo subjectinfo = db.SubjectInfoes.Find(id);
            if (subjectinfo == null)
            {
                return HttpNotFound();
            }
            return View(subjectinfo);
        }

        // GET: /SubjectInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /SubjectInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubjectInfo subjectinfo)
        {
            if (ModelState.IsValid)
            {
                try {
                db.SubjectInfoes.Add(subjectinfo);
                db.SaveChanges();
                TempData["ok"] = "ok";
                TempData["message"] = "<span class=\"color-green\">Successfully Saved!</span>";
                  }
                 catch (Exception ex)
                {
                    TempData["message"] = "<span class=\"color-red\">" + ex.InnerException.Message + "</span>";
                }
                return RedirectToAction("Index");
    
            }

            return View(subjectinfo);
        }

        // GET: /SubjectInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                SubjectInfo subjectinfo = db.SubjectInfoes.Find(id);
                return View(subjectinfo);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.InnerException.Message + "</span>";
            }
           return RedirectToAction("Index");
        }

        // POST: /SubjectInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubjectInfo subjectinfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(subjectinfo).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["ok"] = "ok";
                    TempData["message"] = "<span class=\"color-green\">Successfully Saved!</span>";
                }
                catch (Exception ex)
                {
                    TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                }


                return RedirectToAction("Index");
            }
            return View(subjectinfo);
        }

        // GET: /SubjectInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectInfo subjectinfo = db.SubjectInfoes.Find(id);
            if (subjectinfo == null)
            {
                return HttpNotFound();
            }
            return View(subjectinfo);
        }

        // POST: /SubjectInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubjectInfo subjectinfo = db.SubjectInfoes.Find(id);
            db.SubjectInfoes.Remove(subjectinfo);
            db.SaveChanges();
            TempData["ok"] = "ok";
            TempData["message"] = "<span class=\"color-green\">Successfully Saved!</span>";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
