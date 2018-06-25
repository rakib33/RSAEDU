using RSAEDU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RSAEDU.ViewModel;
using System.Data.Entity;
using clslib.mddl;

namespace RSAEDU.Controllers
{
    [Authorize]
    public class ExamResultController : Controller
    {
        //List<ExamInfo> ExamInfoes = new List<ExamInfo>();
        //List<SubjectInfo> SubjectInfoes = new List<SubjectInfo>();
        //public ExamResultController() 
        //{
          
        //    ExamInfoes = db.ExamInfoes.ToList();
        //    SubjectInfoes = db.SubjectInfoes.ToList();
        
        //}
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /ExamResult/
        public ActionResult Index(int? ExamId, int? DeparmentId, int? Subject, int? fromRoll, int? toRoll, string MarkOption, int? IsAjax)
        {
     
            try
            {
                List<ExamResult> result = new List<ExamResult>();
                List<ResultType> type = new List<ResultType>();
                string bredcum = "";
                ViewBag.Type = null;
                if (ExamId.HasValue && DeparmentId.HasValue && Subject.HasValue)
                {
                    ExamInfo examInfo = db.ExamInfoes.Find(ExamId.Value);

                    var ExamResult = db.ExamResults.Where(t => t.ExamId == ExamId.Value && t.SubjectId == Subject.Value).ToList();
                    var StudentInfoes = db.StudentInfoes.Where(t => t.FacultyId == DeparmentId.Value).ToList();
                    var ExamInfoes = db.ExamInfoes.ToList();
                    var SubjectInfoes = db.SubjectInfoes.ToList();

                    if (MarkOption == "M")
                    {
                        result = (from r in ExamResult
                                  group r by r.StudentId into g
                                  join stu in StudentInfoes on g.FirstOrDefault().StudentId equals stu.Id // .Where(t=>t.FacultyId == DeparmentId.Value) 11/06/18
                                  join exam in ExamInfoes on g.FirstOrDefault().ExamId equals exam.Id
                                  join sub in SubjectInfoes on g.FirstOrDefault().SubjectId equals sub.Id                                
                                  select new ExamResult
                                  {
                                      StudentId = g.FirstOrDefault().StudentId,
                                      SubjectId = g.FirstOrDefault().SubjectId,
                                      RollNo = stu.RollNo,
                                      StudentName = stu.StudentName,
                                      ExamName = exam.ExamName,
                                      SubjectName = sub.SubjectName,
                                      ResultType = DDL.sp_ExamResult(ExamId.Value, DeparmentId.Value, g.FirstOrDefault().StudentId)
                                  }).ToList();
                        
                    }
                    else
                    {
                      

                        result = (from r in DDL.sp_ExamResultUnmarked(ExamId.Value, Subject.Value, examInfo.ClassId, DeparmentId.Value)
                                  select new ExamResult { 
                                   StudentId = r.StudentId,
                                   SubjectId = r.SubjectId,
                                   RollNo = r.RollNo,
                                   StudentName = r.StudentName,
                                   ExamName = r.ExamName,
                                   SubjectName = r.SubjectName,
                                   ResultType = DDL.sp_ExamResult(ExamId.Value, DeparmentId.Value,r.StudentId)                                  
                                  }
                                ).ToList();
                    
                    }

                    var item = result.FirstOrDefault();
                 
                   
                    if (item != null){
                        bredcum = item.ExamName + " / " + item.SubjectName;
                        type = item.ResultType.ToList();
                    }

                   if (fromRoll.HasValue)

                       result = result.Where(t => t.RollNo >= fromRoll.Value).ToList();

                   if (toRoll.HasValue)
                       result = result.Where(t => t.RollNo <= toRoll.Value).ToList();


                }

                if (IsAjax == 1)
                {
                    // return json
                    return Json(new { msg = "ok", result, type= type, bredcum }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    ViewBag.ExamId = new SelectList(db.ExamInfoes.ToList().Where(t => t.ExamStatus == "Y"), "Id", "ExamName");
                    ViewBag.Faculty = new SelectList(db.FacultyInfoes.ToList(), "Id", "FacultyName");
                    ViewBag.ok = Convert.ToString(TempData["ok"]);
                    ViewBag.message = Convert.ToString(TempData["message"]);
                    return View(result);
                }
               
            }
            catch (Exception ex)
            {
                if (IsAjax == 1)
                {
                    // return json
                    return Json(new { msg = "exp", Error = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.Error = "Server Error." + ex.Message;
                    return View("Error");
                }    
            }
          
        }


        public ActionResult EditIndex(int? ExamId, int? DeparmentId, int? Subject, int? fromRoll, int? toRoll, string MarkOption)
        {
            try
            {
                List<ExamResult> result = new List<ExamResult>();



                if (ExamId.HasValue && DeparmentId.HasValue && Subject.HasValue)
                {
                    ExamInfo examInfo = db.ExamInfoes.Find(ExamId.Value);
                    
                   // result = db.ExamResults.Where(t => t.ExamId == ExamId.Value && t.SubjectId == Subject.Value).ToList();
                    if (MarkOption == "M")
                    {
                        result = (from r in db.ExamResults.Where(t => t.ExamId == ExamId.Value && t.SubjectId == Subject.Value).ToList()
                                  join exm in db.ExamInfoes.ToList() on r.ExamId equals exm.Id
                                  join etyp in db.ExamTypeInfos.ToList() on r.ExamTypeId equals etyp.ExamTypeId
                                  join stu in db.StudentInfoes.ToList() on r.StudentId equals stu.Id
                                  join sbj in db.SubjectInfoes.ToList() on r.SubjectId equals sbj.Id

                                  select new ExamResult
                                  {
                                      StudentId = r.StudentId,
                                      RollNo = stu.RollNo,
                                      SubjectName = sbj.SubjectName,
                                      StudentName = stu.StudentName,
                                      QuestionType = etyp.ExamTypeName,
                                      ExamName = exm.ExamName,
                                      MarksObtain = r.MarksObtain,
                                      Id = r.Id
                                  }
                                  ).OrderBy(t => t.RollNo).ToList();

                    }
                    else
                    {


                        result = (from r in DDL.sp_ExamResultUnmarked(ExamId.Value, Subject.Value, examInfo.ClassId, DeparmentId.Value)
                                  select new ExamResult
                                  {
                                      StudentId = r.StudentId,
                                      SubjectId = r.SubjectId,
                                      RollNo = r.RollNo,
                                      StudentName = r.StudentName,
                                      ExamName = r.ExamName,
                                      SubjectName = r.SubjectName,
                                      QuestionType ="UnMarked"
                                      //ResultType = DDL.sp_ExamResult(ExamId.Value, DeparmentId.Value, r.StudentId)
                                  }
                                ).ToList();

                        ViewBag.Unmarked = "U";
                    }

                    //List<ExamResult> newResult = new List<ExamResult>();
                    //List<ExamType> ExamType = new List<Models.ExamType>();



                    //var item = result.FirstOrDefault();
                    //if (item != null)
                    //    ViewBag.Data = item.ExamName + " / " + item.SubjectName;

                    if (fromRoll.HasValue)

                        result = result.Where(t => t.RollNo >= fromRoll.Value).ToList();

                    if (toRoll.HasValue)
                        result = result.Where(t => t.RollNo <= toRoll.Value).ToList();


                }


                ViewBag.ExamId = new SelectList(db.ExamInfoes.ToList().Where(t => t.ExamStatus == "Y"), "Id", "ExamName");
                ViewBag.Faculty = new SelectList(db.FacultyInfoes.ToList(), "Id", "FacultyName");

                ViewBag.ok = Convert.ToString(TempData["ok"]);
                ViewBag.message = Convert.ToString(TempData["message"]);
                return View(result);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }

        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.ExamId = new SelectList(db.ExamInfoes.ToList().Where(t => t.ExamStatus == "Y"), "Id", "ExamName");
                ViewBag.Faculty = new SelectList(db.FacultyInfoes.ToList(), "Id", "FacultyName");

                View_Result model = new View_Result();
                model.StuList = new List<View_ResultDetails>();

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
      
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(View_Result model)
        {
            try
            {
                if (model.DeparmentId > 0 && model.ExamId > 0 && model.Subject > 0 && model.StuList.Count > 0)
                {
                    ExamResult update;
                    foreach (var item in model.StuList)
                    {

                        //if (item.IsCheck == "on")
                        //{

                            if (item.CQ != null)
                            {

                                ExamResult newResult = new ExamResult();

                                newResult.EntryBy = User.Identity.Name;
                                newResult.EntryDate = DateTime.Now;

                                newResult.ExamId = model.ExamId;
                                newResult.StudentId = item.StudentId;
                                newResult.SubjectId = model.Subject;
                                newResult.ExamTypeId = db.ExamTypeInfos.Where(t => t.ExamTypeName == "CQ").SingleOrDefault().ExamTypeId;
                                newResult.MarksObtain = item.CQ.Value;

                                //find if previous record found then delete 
                                update = new ExamResult();
                                update = db.ExamResults.Where(t => t.ExamId == newResult.ExamId &&
                                                                         t.StudentId == newResult.StudentId &&
                                                                         t.SubjectId == newResult.SubjectId &&
                                                                         t.ExamTypeId == newResult.ExamTypeId
                                                                         ).SingleOrDefault();
                                if (update != null)
                                {
                                    update.MarksObtain = item.CQ.Value;
                                    db.Entry(update).State = EntityState.Modified;
                               
                                }
                                else
                                {
                                    db.ExamResults.Add(newResult);
                                }
                                
                            }
                            if (item.MCQ != null)
                            {
                                ExamResult newResult = new ExamResult();

                                newResult.EntryBy = User.Identity.Name;
                                newResult.EntryDate = DateTime.Now;

                                newResult.ExamId = model.ExamId;
                                newResult.StudentId = item.StudentId;
                                newResult.SubjectId = model.Subject;
                                newResult.ExamTypeId = db.ExamTypeInfos.Where(t => t.ExamTypeName == "MCQ").SingleOrDefault().ExamTypeId;
                                newResult.MarksObtain = item.MCQ.Value;

                                //find if previous record found then delete 
                                update = new ExamResult();
                                update = db.ExamResults.Where(t => t.ExamId == newResult.ExamId &&
                                                                         t.StudentId == newResult.StudentId &&
                                                                         t.SubjectId == newResult.SubjectId &&
                                                                         t.ExamTypeId == newResult.ExamTypeId
                                                                         ).SingleOrDefault();
                                if (update != null)
                                {
                                    update.MarksObtain = item.CQ.Value;
                                    db.Entry(update).State = EntityState.Modified;

                                }
                                else
                                {
                                    db.ExamResults.Add(newResult);
                                }
                              
                            }
                            if (item.Practical != null)
                            {
                                ExamResult newResult = new ExamResult();

                                newResult.EntryBy = User.Identity.Name;
                                newResult.EntryDate = DateTime.Now;

                                newResult.ExamId = model.ExamId;
                                newResult.StudentId = item.StudentId;
                                newResult.SubjectId = model.Subject;
                                newResult.ExamTypeId = db.ExamTypeInfos.Where(t => t.ExamTypeName == "Practical").SingleOrDefault().ExamTypeId;
                                newResult.MarksObtain = item.Practical.Value;

                                //find if previous record found then delete 
                                update = new ExamResult();
                                update = db.ExamResults.Where(t => t.ExamId == newResult.ExamId &&
                                                                         t.StudentId == newResult.StudentId &&
                                                                         t.SubjectId == newResult.SubjectId &&
                                                                         t.ExamTypeId == newResult.ExamTypeId
                                                                         ).SingleOrDefault();
                                if (update != null)
                                {
                                    update.MarksObtain = item.CQ.Value;
                                    db.Entry(update).State = EntityState.Modified;

                                }
                                else
                                {
                                    db.ExamResults.Add(newResult);
                                }
                            }
                       // }
                    }//end loop

                    db.SaveChanges();
                    TempData["ok"] = "ok";
                    TempData["message"] = "<span class=\"color-green\">Successfully Saved!</span>";
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var result = (from r in db.ExamResults.Where(t => t.Id == id).ToList()                            

                              select new ExamResult
                              {
                                  Id = r.Id,
                                  MarksObtain = r.MarksObtain,
                                  ExamName = db.ExamInfoes.Where(t => t.Id == r.ExamId).SingleOrDefault().ExamName,
                                  QuestionType = db.ExamTypeInfos.Where(t => t.ExamTypeId == r.ExamTypeId).SingleOrDefault().ExamTypeName,
                                  StudentName = db.StudentInfoes.Where(t => t.Id == r.StudentId).SingleOrDefault().StudentName,
                                  SubjectName = db.SubjectInfoes.Where(t => t.Id == r.SubjectId).SingleOrDefault().SubjectName,
                                  TotalMarks =  db.ExamInfoDetails.Where(t => t.ExamId == r.ExamId && t.ExamTypeId == r.ExamTypeId && t.SubjectId == r.SubjectId).SingleOrDefault().TotalMarks

                              }).SingleOrDefault();

                return View(result);
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
        
        }

        [HttpPost]
        public ActionResult Edit(ExamResult model)
        {
            try {


             
               ExamResult result = db.ExamResults.Where(t => t.Id == model.Id).SingleOrDefault();

               var getMarks = db.ExamInfoDetails.Where(t => t.ExamId == result.ExamId && t.ExamTypeId == result.ExamTypeId && t.SubjectId == result.SubjectId).SingleOrDefault();


                if (model.MarksObtain >= 0 && model.MarksObtain <= getMarks.TotalMarks)
                {
                    result.MarksObtain = model.MarksObtain;

                    db.Entry(result).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["ok"] = "ok";
                    TempData["message"] = "<span class=\"color-green\">"+ model.StudentName +"Marks Obtain Successfully Updated to!"+ model.MarksObtain+"</span>";
                    return RedirectToAction("Index");
                }
                else {

                    var data = (from r in db.ExamResults.Where(t => t.Id == model.Id).ToList()

                                  select new ExamResult
                                  {
                                      Id = r.Id,
                                      MarksObtain = r.MarksObtain,
                                      ExamName = db.ExamInfoes.Where(t => t.Id == r.ExamId).SingleOrDefault().ExamName,
                                      QuestionType = db.ExamTypeInfos.Where(t => t.ExamTypeId == r.ExamTypeId).SingleOrDefault().ExamTypeName,
                                      StudentName = db.StudentInfoes.Where(t => t.Id == r.StudentId).SingleOrDefault().StudentName,
                                      SubjectName = db.SubjectInfoes.Where(t => t.Id == r.SubjectId).SingleOrDefault().SubjectName
                                  }).SingleOrDefault();
                    
                    ModelState.AddModelError("", "Marks exceed Total Marks "+getMarks.TotalMarks);
                    return View(data);
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "<span class=\"color-red\">" + ex.Message + "</span>";
                return RedirectToAction("Index");
            }
          
         
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LoadSubject(int depId)
        {
            string msg = "";
            List<SubjectInfo> list = new List<SubjectInfo>();
            try
            {
                list = (from fs in db.FacultyInfoSubjects.Where(t => t.FacultyId == depId)
                        join si in db.SubjectInfoes on fs.SubjectId equals si.Id
                        select si
                       ).ToList();

                //now check Is this User has this Subject from UserSubject

                var userId = db.Users.Where(t => t.UserName == User.Identity.Name).SingleOrDefault().Id;

                if (User.Identity.Name != "Admin")
                {

                    list = (from user_sub in db.UserSubjects.Where(t=>t.UserId == userId).ToList() 
                             join sub in list on user_sub.SubjectId equals sub.Id
                             select sub).ToList();
                }

                msg = "Ok";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                list = null;
            }
            return Json(new { msg, list }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult LoadStudent(int examId, int subId,int FacultyId,int? fromRoll,int? toRoll)
        {
            string msg = "Ok";

            try
            {               
                //get class from examId

                int ClassId = db.ExamInfoes.Find(examId).ClassId; //added 11/06/18

                var StudentList = (from sis in db.ExamAttendances.Where(t => t.SubjectId == subId && t.FacultyId == FacultyId && t.Present =="Y" && t.ClassId == ClassId) //added classid 11/06/18
                                   join sinfo in db.StudentInfoes
                                    on sis.StudentId equals sinfo.Id
                                   select sinfo).OrderBy(t=>t.RollNo).ToList();

                // get Question Type
                List<View_ExamTypeInfo> type = (from eid in db.ExamInfoDetails.Where(t => t.SubjectId == subId && t.ExamId == examId)
                            join ti in db.ExamTypeInfos on eid.ExamTypeId equals ti.ExamTypeId
                            select new View_ExamTypeInfo{                         
                             ExamTypeId = ti.ExamTypeId,
                             ExamTypeName = ti.ExamTypeName,
                             TotalMark = eid.TotalMarks
                            }
                           ).ToList();

                if (fromRoll.HasValue)
                {
                    StudentList = StudentList.Where(t => t.RollNo >= fromRoll.Value).ToList();                
                }
                if (toRoll.HasValue)
                {
                    StudentList = StudentList.Where(t => t.RollNo <= toRoll.Value).ToList();
                }
                return Json(new { StudentList, type, msg }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                msg = "Server Respond Failed. Server Error:" + ex.Message;
            }
            return Json(new { msg }, JsonRequestBehavior.AllowGet);
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