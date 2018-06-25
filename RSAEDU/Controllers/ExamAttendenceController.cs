using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RSAEDU.Models;
using RSAEDU.ViewModel;

namespace RSAEDU.Controllers
{
    [Authorize]
    public class ExamAttendenceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: /ExamAttendence/
        public ActionResult Index(int? ClassId, int? FacultyId, int? SubjectId, int? fromRoll, int? toRoll,string Present, int? IsAjax)
        {

            string msg = "";
            string bredcum = "";
            try
            {
                List<ExamAttendance> list = new List<ExamAttendance>();
                

                if (ClassId.HasValue && FacultyId.HasValue && SubjectId.HasValue)
                {
                    list = db.ExamAttendances.Where(t => t.ClassId == ClassId.Value && t.FacultyId == FacultyId.Value && t.SubjectId == SubjectId.Value).ToList();
                  
                    list = (from atd in list
                            join stu in db.StudentInfoes.AsNoTracking().ToList() on atd.StudentId equals stu.Id
                            join sub in db.SubjectInfoes.AsNoTracking().ToList() on atd.SubjectId equals sub.Id
                            join Fac in db.FacultyInfoes.AsNoTracking().ToList() on atd.FacultyId equals Fac.Id
                            join cls in db.ClassInfoes.AsNoTracking().ToList() on atd.ClassId equals cls.Id
                            select new ExamAttendance
                            {
                                Id = atd.Id,
                                Present = atd.Present,
                                StudentName = stu.StudentName,
                                SubjectName = sub.SubjectName,
                                FacultyName = Fac.FacultyName,
                                ClassName = cls.ClassName,
                                RollNo = stu.RollNo,

                            }).OrderBy(t => t.RollNo).ToList();

                    var item = list.FirstOrDefault();
                    if (item != null)
                        bredcum = item.ClassName + " / " + item.FacultyName + " / " + item.SubjectName;

                    if (fromRoll.HasValue)
                    {
                        //get RollNo form Student Info
                        list = list.Where(t => t.RollNo >= fromRoll.Value).ToList();
                    }

                    if (toRoll.HasValue)
                    {
                        list = list.Where(t => t.RollNo <= toRoll.Value).ToList();
                    }

                    if (Present == null)
                    {
                        list = list.Where(t => t.Present == "Y").ToList();
                    }
                    else if (Present != "All" && Present != null)
                    {
                        list = list.Where(t => t.Present == Present).ToList();
                    }
                    else
                    {
                      
                    }
                
                }

                if (IsAjax == 1)
                {
                    // return json
                    return Json(new { msg = "ok", list, bredcum }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.ok = Convert.ToString(TempData["ok"]);
                    ViewBag.message = Convert.ToString(TempData["message"]);

                    ViewBag.Class_Id = new SelectList(db.ClassInfoes.ToList(), "Id", "ClassName");
                    ViewBag.Faculty_Id = new SelectList(db.FacultyInfoes.ToList(), "Id", "FacultyName");
                    ViewBag.Present = new SelectList(PresentList(), "Id", "Text");

                    return View(list);
                }
            }
            catch (Exception ex)
            {
                if (IsAjax == 1)
                {
                    // return json
                    return Json(new { msg = "exp", Error= ex.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.Error = "Server Error.err " + ex.Message;
                    return View("Error");
                }
            }
        }

        public IEnumerable<ExamAttendance> getList()
        {

           return (from es in db.ExamAttendances
                        join clss in db.ClassInfoes   on es.ClassId   equals clss.Id
                        join fc   in db.FacultyInfoes on es.FacultyId equals fc.Id
                        select new ExamAttendance
                        {
                            FacultyId   = es.FacultyId,
                            FacultyName = fc.FacultyName,
                            ClassId     = es.ClassId,
                            ClassName   = clss.ClassName,
                            Present     = es.Present,
                            EntryBy     = es.EntryBy,
                            EntryDate   = es.EntryDate
                        }
                    ).AsNoTracking().ToList();
        
        }

        // GET: /ExamAttendence/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamAttendance examattendance = db.ExamAttendances.Find(id);
            if (examattendance == null)
            {
                return HttpNotFound();
            }
            return View(examattendance);
        }

        // GET: /ExamAttendence/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Class_Id = new SelectList(db.ClassInfoes.ToList(), "Id", "ClassName");
                ViewBag.Faculty_Id = new SelectList(db.FacultyInfoes.ToList(), "Id", "FacultyName");
                ViewBag.Present = new SelectList(PresentList(), "Id", "Text");

                View_Attendence model = new View_Attendence();
                model.StuList = new List<View_StudentPresent>();

                string uname = User.Identity.Name;

                string Id = db.Users.Where(t => t.UserName == uname).SingleOrDefault().Id;

                ViewBag.UserId = Id;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.InnerException.Message + "</span>";
            }
            return RedirectToAction("Index");
        }

        public static List<ExamPresent> PresentList()
        {
            List<ExamPresent> items = new List<ExamPresent>();

            items.Add(new ExamPresent { Id = "Y", Text="Present" });
            items.Add(new ExamPresent { Id = "N", Text = "Absent" });        
            items.Add(new ExamPresent { Id = "All", Text = "All Student" });

            return items;
        }

        public static List<ExamPresent> EditList()
        {
            List<ExamPresent> items = new List<ExamPresent>();

            items.Add(new ExamPresent { Id = "Y", Text = "Present" });
            items.Add(new ExamPresent { Id = "N", Text = "Absent" });
           
            return items;
        }


        // POST: /ExamAttendence/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(View_Attendence model, string SubjectId)
        {
            //if isCheck On that mean its check
         try            
          {

            if (model.ClassId > 0 && model.FacultyId > 0 && model.SubjectId > 0 && model.StuList.Count > 0)
            {
                foreach (var item in model.StuList)
                {
                    ExamAttendance newObj = new ExamAttendance();
                    if (item.IsCheck == "on")
                    {
                        newObj.Present = "Y";

                    }
                    else {
                        newObj.Present = "N";
                    }

                    newObj.ClassId = model.ClassId;
                    newObj.EntryBy = User.Identity.Name;
                    newObj.EntryDate = DateTime.Now;
                    newObj.FacultyId = model.FacultyId;
                    newObj.SubjectId = model.SubjectId;

                    newObj.StudentId = item.StudentId;

                    //first try to get duplicate if found the update else save

                    ExamAttendance prvObj = db.ExamAttendances.Where(t => t.ClassId == model.ClassId && t.FacultyId == model.FacultyId && t.SubjectId == model.SubjectId
                                            && t.StudentId == item.StudentId).SingleOrDefault();

                    if (prvObj != null)
                    {
                        prvObj.Present = newObj.Present;
                        db.Entry(prvObj).State = EntityState.Modified;
                    }
                    else
                    {
                        db.ExamAttendances.Add(newObj);
                    }
             
                }
               
                db.SaveChanges();
                TempData["ok"] = "ok";
                TempData["message"] = "<span class=\"color-green\">Successfully Saved!</span>";
            }
            else {

                TempData["ok"] = "";
                ViewBag.Message = "<span class=\"color-red\">One more validation Error.</span>";
            }

           }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.InnerException.Message+ "</span>";                
            }
           return RedirectToAction("Index");
        }
        //public ActionResult Create(ExamAttendance examattendance)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        examattendance.EntryBy = User.Identity.Name;
        //        examattendance.EntryDate = DateTime.Now;

        //        db.ExamAttendances.Add(examattendance);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(examattendance);
        //}

        // GET: /ExamAttendence/Edit/5
        public ActionResult Edit(int? id)
        {
           try{

            ExamAttendance examattendance = db.ExamAttendances.Find(id);        
            examattendance.StudentName = db.StudentInfoes.Find(examattendance.StudentId).StudentName;

            ViewBag.Class_Id = new SelectList(db.ClassInfoes.ToList(), "Id", "ClassName",examattendance.ClassId);
            ViewBag.Faculty_Id = new SelectList(db.FacultyInfoes.ToList(), "Id", "FacultyName",examattendance.FacultyId);

            ViewBag.Status = new SelectList(EditList(), "Id", "Text", examattendance.Present);
       
            return View(examattendance);
              }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
        }

        // POST: /ExamAttendence/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExamAttendance examattendance)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ExamAttendance update = db.ExamAttendances.Where(t => t.Id == examattendance.Id).SingleOrDefault();

                    update.Present = examattendance.Present;

                    db.Entry(update).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["ok"] = "ok";
                    TempData["message"] = "<span class=\"color-green\">Successfully Saved!</span>";
                    return RedirectToAction("Index");
                }
                return View(examattendance);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
        }

        // GET: /ExamAttendence/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamAttendance examattendance = db.ExamAttendances.Find(id);
            if (examattendance == null)
            {
                return HttpNotFound();
            }
            return View(examattendance);
        }

        // POST: /ExamAttendence/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamAttendance examattendance = db.ExamAttendances.Find(id);
            db.ExamAttendances.Remove(examattendance);
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


    public class ExamPresent {
        public string Id { get; set; }
        public string Text { get; set; }
    }

}
