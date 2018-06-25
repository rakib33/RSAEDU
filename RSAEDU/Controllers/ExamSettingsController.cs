using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RSAEDU.Models;
using RSAEDU.ViewModel;

using System.Data.Entity;
using System.Net;

namespace RSAEDU.Controllers
{
    [Authorize]
    public class ExamSettingsController : Controller
    {
        //
        // GET: /ExamSettings/
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Create()
        {
            try
            {
                View_ExamDetail model = new View_ExamDetail();

                var type = db.ExamTypeInfos.ToList();

                model.Examinfo = new ExamInfo();
                model.TypeDetails = new List<View_ExamTypeDetails>();

                foreach (var item in type)
                {

                    model.TypeDetails.Add(new View_ExamTypeDetails { chk = false, TypeId = item.ExamTypeId, TypeName = item.ExamTypeName, PassMark = null, TotalMark = null });

                }

                ViewBag.Examinfo = new SelectList(db.ExamInfoes.Where(t => t.ExamStatus == "Y").ToList(), "Id", "ExamName");

                ViewBag.Class_Id = new SelectList(db.ClassInfoes.ToList(), "Id", "ClassName");
                ViewBag.Subject = new SelectList(db.SubjectInfoes.ToList(), "Id", "SubjectName");

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
        }



        [AllowAnonymous]
        [HttpPost]
        public ActionResult LoadSubject(int examId)
        {
            string msg = "";
            List<SubjectInfo> list = new List<SubjectInfo>();
            try
            {
                list = (from einfo in db.ExamInfoes.Where(t=>t.Id == examId)                    
                        join fs in db.FacultyInfoSubjects on einfo.ClassId equals fs.ClassId
                        join si in db.SubjectInfoes on fs.SubjectId equals si.Id
                        select si
                       ).ToList();

                msg = "Ok";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                list = null;
            }
            return Json(new { msg, list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(int? ExamId, int? IsAjax) 
        {
            try
            {
                List<ExamInfoDetail> detail = new List<ExamInfoDetail>();

                var list = db.ExamInfoDetails.ToList();

                var ExamInfoes = db.ExamInfoes.ToList();

                if (ExamId.HasValue)
                {
                    detail = (from ex in list
                              join ei in ExamInfoes.Where(t=>t.Id == ExamId.Value) on ex.ExamId equals ei.Id
                              join et in db.ExamTypeInfos on ex.ExamTypeId equals et.ExamTypeId
                              join sb in db.SubjectInfoes on ex.SubjectId equals sb.Id
                              select new ExamInfoDetail
                              {
                                  Id = ex.Id,

                                  SubjectId = ex.SubjectId,
                                  SubjectName = sb.SubjectName,

                                  ExamId = ex.ExamId,
                                  ExamName = ei.ExamName,

                                  ExamTypeId = ex.ExamTypeId,
                                  ExamType = et.ExamTypeName,

                                  PassMarks = ex.PassMarks,
                                  TotalMarks = ex.TotalMarks,

                                  EntryDate = ex.EntryDate,
                                  EntryBy = ex.EntryBy

                              }).ToList();

                }

                if (IsAjax == 1)
                {
                    // return json
                    return Json(new { msg = "ok", detail }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.ExamId = new SelectList(ExamInfoes.ToList(), "Id", "ExamName");
                    ViewBag.ok = Convert.ToString(TempData["ok"]);
                    ViewBag.message = Convert.ToString(TempData["message"]);
                    return View(detail);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        public ActionResult Save(View_ExamDetail model, int? SubjectId)
        {

                if(model.TypeDetails.Count>0)
                {
                    foreach(var item in model.TypeDetails){
                        if (item.chk == true)
                        {
                            ExamInfoDetail obj = new ExamInfoDetail();
                            obj.EntryBy = User.Identity.Name;
                            obj.EntryDate = DateTime.Now;
                            obj.ExamId = model.Examinfo.Id;
                            obj.ExamTypeId = db.ExamTypeInfos.Where(t => t.ExamTypeName == item.TypeName).SingleOrDefault().ExamTypeId;
                            obj.SubjectId = SubjectId.Value;
                            obj.TotalMarks = Convert.ToDecimal(item.TotalMark.Value);
                            obj.PassMarks = Convert.ToDecimal(item.PassMark.Value);

                            //check is already exists update it
                            var isData = new ExamInfoDetail();
                            isData = db.ExamInfoDetails.Where(t => t.ExamId == obj.ExamId && t.SubjectId == obj.SubjectId && t.ExamTypeId == obj.ExamTypeId).SingleOrDefault();

                            if (isData != null)
                            {
                                isData.TotalMarks = Convert.ToDecimal(item.TotalMark.Value);
                                isData.PassMarks = Convert.ToDecimal(item.PassMark.Value);

                                db.Entry(isData).State = EntityState.Modified;
                            }
                            else
                            {
                                db.ExamInfoDetails.Add(obj);
                            }
                        }
                    }

                    db.SaveChanges();

                    TempData["ok"] = "ok";
                    TempData["message"] = "<span class=\"color-green\">Successfully Saved!</span>";
                    return RedirectToAction("Index");
                }

            

            return null;
        }


        public ActionResult Edit(int id)
        {
            try
            {
                ExamInfoDetail obj = db.ExamInfoDetails.Find(id);

                obj.ExamName = db.ExamInfoes.Find(obj.ExamId).ExamName;
                obj.SubjectName = db.SubjectInfoes.Find(obj.SubjectId).SubjectName;
                obj.ExamType = db.ExamTypeInfos.Find(obj.ExamTypeId).ExamTypeName;

                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(ExamInfoDetail model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ExamInfoDetail obj = db.ExamInfoDetails.Find(model.Id);
                    obj.TotalMarks = model.TotalMarks;
                    obj.PassMarks = model.PassMarks;


                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["ok"] = "ok";
                    TempData["message"] = "<span class=\"color-green\">" + model.ExamName +" "+ model.SubjectName +" Successfully Updated!</span>";
                    return RedirectToAction("Index");
                }
                return View(model);
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

	}
}