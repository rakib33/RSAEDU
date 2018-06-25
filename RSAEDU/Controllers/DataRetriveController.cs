using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RSAEDU.Models;
using RSAEDU.ViewModel;

namespace RSAEDU.Controllers
{
    public class DataRetriveController : Controller
    {


        private ApplicationDbContext db = new ApplicationDbContext();


        [AllowAnonymous]
        [HttpPost]
        public ActionResult LoadAllSubject(int ClassId, int FacultyId)
        {
            string msg = "";
            List<SubjectInfo> list = new List<SubjectInfo>();
            try
            {
                list = (from fs in db.FacultyInfoSubjects.Where(t => t.FacultyId == FacultyId && t.ClassId == ClassId)
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


        //User Only Can see those Subjects That is assigned to him
        //a bangla teacher can see only Bangla an english teacher can see only English
        //LoadUserSubject
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LoadSubject(int ClassId, int FacultyId)
        {
            string UId = db.Users.Where(t => t.UserName == User.Identity.Name).SingleOrDefault().Id;
            string msg = "";
            List<SubjectInfo> list = new List<SubjectInfo>();
            try
            {
              

                if (User.Identity.Name != "Admin")
                {

                    list = (from fs in db.UserSubjects.Where(t => t.FacultyId == FacultyId && t.ClassId == ClassId && t.UserId == UId)
                            join si in db.SubjectInfoes on fs.SubjectId equals si.Id
                            select si
                       ).ToList();
                }
                else {

                    list = (from fs in db.UserSubjects.Where(t => t.FacultyId == FacultyId && t.ClassId == ClassId)
                            join si in db.SubjectInfoes on fs.SubjectId equals si.Id
                            select si
                         ).ToList();
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
        public ActionResult LoadStudent(int classId,int facultyId, int subId)
        {
            string msg = "Ok";

          
            try
            {
               
                // here we load student there subject wise type for result entry
                //var StudentList = (from sis in db.FacultyInfoSubjects.Where(t => t.SubjectId == subId && t.ClassId == classId && t.FacultyId == facultyId)
                //                   join sinfo in db.StudentInfoSubjects on sis.SubjectId equals sinfo.SubjectId
                //                   join stu in db.StudentInfoes on sinfo.StudentId equals stu.Id
                //                   select stu).ToList();


                var StudentList = (from sis in db.StudentInfoSubjects.Where(t=>t.SubjectId == subId)
                                  join sinfo in db.StudentInfoes.Where(t => t.ClassId == classId && t.FacultyId == facultyId)
                                   on sis.StudentId equals sinfo.Id 
                                   select sinfo ).OrderBy(t=>t.RollNo).ToList();

                return Json(new { StudentList, msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                msg = "Server Respond Failed. Server Error:"+ex.Message;
            }
            return Json(new { msg }, JsonRequestBehavior.AllowGet);

            //  return null;
        }


        [AllowAnonymous]
        public ActionResult GetTotalSubject(int facultyId,int flag)
        { 
          string msg = "Ok";          
            try
            {
               
               var  TotalSubject =  db.FacultyInfoSubjects.Where(t => t.FacultyId == facultyId).Count();
               return Json(new { TotalSubject, msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                msg = "Server Respond Failed. Server Error:"+ex.Message;
            }
            return Json(new { msg }, JsonRequestBehavior.AllowGet);
        }

	}
}