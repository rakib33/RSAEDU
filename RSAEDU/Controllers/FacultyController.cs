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

    [Authorize]
    public class FacultyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Faculty/
        public ActionResult Index()
        {
            try
            {
                ViewBag.ok = Convert.ToString(TempData["ok"]);
                ViewBag.message = Convert.ToString(TempData["message"]);
                return View(db.FacultyInfoes.ToList());
            }
            catch (Exception ex) {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        // GET: /Faculty/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacultyInfo facultyinfo = db.FacultyInfoes.Find(id);
            if (facultyinfo == null)
            {
                return HttpNotFound();
            }
            return View(facultyinfo);
        }

        // GET: /Faculty/Create
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch(Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
        }

        // POST: /Faculty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FacultyInfo facultyinfo)
        {
            try
            {
           
                facultyinfo.EntryBy = User.Identity.Name;
                facultyinfo.EntryDate = DateTime.Now;
              

                if (ModelState.IsValid)
                {
                    db.FacultyInfoes.Add(facultyinfo);
                    db.SaveChanges();

                    TempData["ok"] = "ok";
                        
                    TempData["message"] = "<span class=\"color-green\">" + facultyinfo.FacultyName + " Successfully Saved!</span>";
                    return RedirectToAction("Index");
                }

                ViewBag.Message = "<span class=\"color-red\">One more validation Error.</span>";
                return View(facultyinfo);
            }
            catch (Exception ex)
            {

                TempData["message"] = "<span class=\"color-red\">" + ex.InnerException.Message+ "</span>";
                return RedirectToAction("Index");
            }
        }

        // GET: /Faculty/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                FacultyInfo facultyinfo = db.FacultyInfoes.Find(id);

                return View(facultyinfo);
            }
            catch (Exception ex)
            {

                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
        }

        // POST: /Faculty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FacultyInfo facultyinfo)
        {
            try
            {
            if (ModelState.IsValid)
            {
                db.Entry(facultyinfo).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ok"] = "ok";
                TempData["message"] = "<span class=\"color-green\">" + facultyinfo.FacultyName + " Edited Successfully Saved!</span>";
                return RedirectToAction("Index");

            }

            ViewBag.Message = "<span class=\"color-red\">One more validation Error.</span>";
            return View(facultyinfo);

             }
            catch (Exception ex)
            {

                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
        }

        // GET: /Faculty/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacultyInfo facultyinfo = db.FacultyInfoes.Find(id);
            if (facultyinfo == null)
            {
                return HttpNotFound();
            }
            return View(facultyinfo);
        }

        // POST: /Faculty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FacultyInfo facultyinfo = db.FacultyInfoes.Find(id);
            db.FacultyInfoes.Remove(facultyinfo);
            db.SaveChanges();
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
