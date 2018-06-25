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
    public class StudentInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /StudentInfo/
        public ActionResult Index()
        {
            try
            {
                ViewBag.ok = Convert.ToString(TempData["ok"]);
                ViewBag.message = Convert.ToString(TempData["message"]);

                return View(db.StudentInfoes.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error " + ex.Message;
                return View("Error");
            }

        }

        // GET: /StudentInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentInfo studentinfo = db.StudentInfoes.Find(id);
            if (studentinfo == null)
            {
                return HttpNotFound();
            }
            return View(studentinfo);
        }

        // GET: /StudentInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /StudentInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentInfo studentinfo)
        {
            if (ModelState.IsValid)
            {
                db.StudentInfoes.Add(studentinfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(studentinfo);
        }

        // GET: /StudentInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentInfo studentinfo = db.StudentInfoes.Find(id);
            if (studentinfo == null)
            {
                return HttpNotFound();
            }
            return View(studentinfo);
        }

        // POST: /StudentInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,ClassId,SectionId,FacultyId,StudentTypeId,AcademicSession,Regno,RollNo,StudentName,FatherName,MotherName,PresentAdd,PermanentAdd,DOB,Gender,Remarks,EntryBy,EntryDate,UpdateBy,UpdateDate,AuthorizedBy,AuthorizedDate")] StudentInfo studentinfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studentinfo);
        }

        // GET: /StudentInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentInfo studentinfo = db.StudentInfoes.Find(id);
            if (studentinfo == null)
            {
                return HttpNotFound();
            }
            return View(studentinfo);
        }

        // POST: /StudentInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentInfo studentinfo = db.StudentInfoes.Find(id);
            db.StudentInfoes.Remove(studentinfo);
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
