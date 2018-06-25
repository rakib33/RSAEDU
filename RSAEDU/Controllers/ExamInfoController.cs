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
    public class ExamInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GradeIndex()
        {
            try
            {
                var GradeResult = db.GradeInfoes.ToList();
                return View(GradeResult);
            }
            catch (Exception ex) {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
          
        }


        // GET: /ExamInfo/
        public ActionResult Index()
        {
            try
            {

                ViewBag.ok = Convert.ToString(TempData["ok"]);
                ViewBag.message = Convert.ToString(TempData["message"]);
                return View(db.ExamInfoes.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        // GET: /ExamInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamInfo examinfo = db.ExamInfoes.Find(id);
            if (examinfo == null)
            {
                return HttpNotFound();
            }
            return View(examinfo);
        }

        // GET: /ExamInfo/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Class_Id = new SelectList(db.ClassInfoes.ToList(), "Id", "ClassName");

                ViewBag.Status = StatusList();
                return View();
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                return RedirectToAction("EditIndex");
            }
        }

        public static List<SelectListItem> StatusList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "Active",
                Value = "Y"
            });

            items.Add(new SelectListItem
            {
                Text = "InActive",
                Value = "N"
                //Selected = true
            });

            return items;
        }


        public static List<SelectListItem> PublistStatus()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "Publish",
                Value = "P"
            });

            items.Add(new SelectListItem
            {
                Text = "Not Publish",
                Value = ""
            });  

            return items;
        }

        // POST: /ExamInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExamInfo examinfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //int maxId =db.ExamInfoes.Max(t => t.Id);

                    //if (maxId == null)
                    //    maxId = 1;
                    //else
                    //    maxId += 1;

                    //examinfo.Id =Convert.ToInt32(maxId);

                    examinfo.EntryBy = User.Identity.Name;
                    examinfo.EntryDate = DateTime.Today;

                    examinfo.AuthorizedBy = User.Identity.Name;
                    examinfo.AuthorizedDate = DateTime.Today;

                    db.ExamInfoes.Add(examinfo);
                    db.SaveChanges();

                    TempData["ok"] = "ok";
                    TempData["message"] = "<span class=\"color-green\">Successfully Saved!</span>";

                  
                }

                return View(examinfo);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
            }

            return RedirectToAction("Index");

        }

        // GET: /ExamInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                ExamInfo examinfo = db.ExamInfoes.Find(id);
                ViewBag.Class_Id = new SelectList(db.ClassInfoes.ToList(), "Id", "ClassName", examinfo.ClassId);

                ViewBag.Status = StatusList();
                ViewBag.Publish = PublistStatus();

                return View(examinfo);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
        }

        // POST: /ExamInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExamInfo examinfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(examinfo).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["ok"] = "ok";
                    TempData["message"] = "<span class=\"color-green\">Successfully Saved!</span>";
                    return RedirectToAction("Index");
                }

                ViewBag.Class_Id = new SelectList(db.ClassInfoes.ToList(), "Id", "ClassName", examinfo.ClassId);
                ViewBag.Status = StatusList();
                ViewBag.Publish = PublistStatus();

                return View(examinfo);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
        }

        // GET: /ExamInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamInfo examinfo = db.ExamInfoes.Find(id);
            if (examinfo == null)
            {
                return HttpNotFound();
            }
            return View(examinfo);
        }

        // POST: /ExamInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamInfo examinfo = db.ExamInfoes.Find(id);
            db.ExamInfoes.Remove(examinfo);
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
