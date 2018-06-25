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
    public class ExamTypeInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /ExamTypeInfo/
        public ActionResult Index()
        {
            return View(db.ExamTypeInfos.ToList());
        }

        // GET: /ExamTypeInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamTypeInfo examtypeinfo = db.ExamTypeInfos.Find(id);
            if (examtypeinfo == null)
            {
                return HttpNotFound();
            }
            return View(examtypeinfo);
        }

        // GET: /ExamTypeInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ExamTypeInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ExamTypeId,ExamTypeName,EntryBy,EntryDate")] ExamTypeInfo examtypeinfo)
        {
            if (ModelState.IsValid)
            {
                db.ExamTypeInfos.Add(examtypeinfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(examtypeinfo);
        }

        // GET: /ExamTypeInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamTypeInfo examtypeinfo = db.ExamTypeInfos.Find(id);
            if (examtypeinfo == null)
            {
                return HttpNotFound();
            }
            return View(examtypeinfo);
        }

        // POST: /ExamTypeInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ExamTypeId,ExamTypeName,EntryBy,EntryDate")] ExamTypeInfo examtypeinfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examtypeinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(examtypeinfo);
        }

        // GET: /ExamTypeInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamTypeInfo examtypeinfo = db.ExamTypeInfos.Find(id);
            if (examtypeinfo == null)
            {
                return HttpNotFound();
            }
            return View(examtypeinfo);
        }

        // POST: /ExamTypeInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamTypeInfo examtypeinfo = db.ExamTypeInfos.Find(id);
            db.ExamTypeInfos.Remove(examtypeinfo);
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
