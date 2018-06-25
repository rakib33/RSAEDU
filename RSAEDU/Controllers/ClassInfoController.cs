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
    public class ClassInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /ClassInfo/
        public ActionResult Index()
        {
            return View(db.ClassInfoes.ToList());
        }

        // GET: /ClassInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassInfo classinfo = db.ClassInfoes.Find(id);
            if (classinfo == null)
            {
                return HttpNotFound();
            }
            return View(classinfo);
        }

        // GET: /ClassInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ClassInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,ClassName,EntryBy,EntryDate")] ClassInfo classinfo)
        {
            if (ModelState.IsValid)
            {
                db.ClassInfoes.Add(classinfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(classinfo);
        }

        // GET: /ClassInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassInfo classinfo = db.ClassInfoes.Find(id);
            if (classinfo == null)
            {
                return HttpNotFound();
            }
            return View(classinfo);
        }

        // POST: /ClassInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,ClassName,EntryBy,EntryDate")] ClassInfo classinfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(classinfo);
        }

        // GET: /ClassInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassInfo classinfo = db.ClassInfoes.Find(id);
            if (classinfo == null)
            {
                return HttpNotFound();
            }
            return View(classinfo);
        }

        // POST: /ClassInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClassInfo classinfo = db.ClassInfoes.Find(id);
            db.ClassInfoes.Remove(classinfo);
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
