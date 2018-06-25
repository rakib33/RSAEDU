using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RSAEDU.Models;


using System.Data;
using System.Globalization;
using System.ComponentModel;
using Microsoft.Reporting.WebForms;
using System.Data.Entity;
using clslib.mddl;
using clslib.Iddl;

namespace RSAEDU.Controllers
{
    public class ReportController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        public static IResult repo = new Rresult();

        public static List<Subject> subjectList = new List<Subject>();
        // GET: /Report/
        public ActionResult Index()
        {
            //List<SingleResult> Result = ReturnResult();

            ViewBag.ExamId = new SelectList(db.ExamInfoes.ToList().Where(t => t.ExamStatus == "Y"), "Id", "ExamName");
            ViewBag.Faculty = new SelectList(db.FacultyInfoes.ToList(), "Id", "FacultyName");

            return View();
        }


        public List<SingleResult> ReturnResult()
        {
            List<SingleResult> studentList = new List<SingleResult>();
            studentList.Add(new SingleResult { RollNo = 1, StudentName = "A", Subject = null });
            studentList.Add(new SingleResult { RollNo = 2, StudentName = "B", Subject = null });
            studentList.Add(new SingleResult { RollNo = 3, StudentName = "C", Subject = null });
            List<ExamType> type = new List<ExamType>();
            type.Add(new ExamType
            {
                ExamTypeId = 1,
                ExamTypeName = "CQ",
                MarksObtain = 0,
                PassMarks = 10,
                TotalMarks = 50,
                RollNo = 0,
                StudentName = ""
            });
            type.Add(new ExamType
            {
                ExamTypeId = 1,
                ExamTypeName = "MCQ",
                MarksObtain = 0,
                PassMarks = 10,
                TotalMarks = 50,
                RollNo = 0,
                StudentName = null
            });

            List<Subject> subjectList = new List<Subject>();
            subjectList.Add(new Subject
            {
                SubjectId = 1,
                SubjectName = "Bangla",
                ExamTypeList = type
            });
            subjectList.Add(new Subject
            {
                SubjectId = 1,
                SubjectName = "Sociology",
                ExamTypeList =
                    type
            });


            //loop student
            foreach (var item in studentList)
            {
                // List<Subject> NewSubject = new List<Subject>();
                var NewSubject = subjectList.DeepClone(); // 
                //loop subject
                foreach (var sub in NewSubject)
                {
                    var ExamType = sub.ExamTypeList.DeepClone(); // new List<ExamType>();
                    //loop ExamType… Problem 2
                    foreach (var _type in ExamType)
                    {
                        _type.MarksObtain = 10;
                        _type.RollNo = item.RollNo;
                        _type.StudentName = item.StudentName;
                        _type.PassStatus = "P";
                        _type.SubjectName = sub.SubjectName;

                        // ExamType.Add(_type.DeepClone());
                    }

                    sub.ExamTypeList = ExamType.DeepClone();
                }

                item.Subject = NewSubject;
            }
            return studentList;
        } 


        public List<Subject> CleanSubject(List<Subject> subject) {


            foreach (var s in subject)
            { 
               foreach(var t in s.ExamTypeList){
                   t.FacultyName = t.StudentName = t.OptionalSubject = t.PassStatus = null;
                   t.GPA = t.MarksObtain = t.RollNo = t.StudentId = 0;               
               }
            
            }
            return subject;
        
        }
        public ActionResult TabulerResult(int ExamId, int FacultyId) 
        {
           
            int flag = 0;


            List<SingleResult> allStudentResult = DDL.sp_GetExamResultStudent(ExamId, FacultyId);
            List<ExamType> type = DDL.GetStudentSubjectTypeResult(null, ExamId, FacultyId, null);
            subjectList = DDL.sp_ExamSubject(ExamId, FacultyId);

            foreach (var item in allStudentResult)
            {                
                item.Subject = repo.getTabulerResult(subjectList, type, item.RollNo);             
            }


            //if (list != null && list.Count() > 0)
            //{
            //    //remove from subject
            //    foreach (var item in list)
            //    {
            //        flag++;

            //        if (flag == 1)
            //        {
            //            sbObj.Add(new Subject
            //            {
            //                ExamTypeList = item.ExamTypeList,
            //                SubjectName = "Biology",
            //                FacultyName = item.FacultyName,
            //                ShortName = "Biology",
            //                ExamId = item.ExamId,
            //                SubjectId = item.SubjectId
            //            });
            //        }
            //        sbObj.Remove(item);
            //    }
            //}

            ViewBag.ExamName = db.ExamInfoes.Find(ExamId).ExamName;
            ViewBag.FacultyName = db.FacultyInfoes.Find(FacultyId).FacultyName;
           
            return View(allStudentResult);

        }


        public grade getGrade(decimal marks)
        {

          grade grade = new grade();

          var Gradeinfo = db.GradeInfoes.Where(t => t.MarksFrom <= marks && t.MarksTo >= marks).SingleOrDefault();

           grade.Grade = Gradeinfo.GradeName;
           grade.GradePoint = Convert.ToDouble(Gradeinfo.GradePoint);

            //new grade();
            //if (marks >= 80)
            //{
            //    grade.Grade = "A+";
            //    grade.GradePoint = 5;
            //}
            //else if(marks >=70){
            //    grade.Grade = "A";
            //    grade.GradePoint = 4;
            //}
            //else if (marks >= 60)
            //{
            //    grade.Grade = "A-";
            //    grade.GradePoint = 3.5;
            //}
            //else if (marks >= 50)
            //{
            //    grade.Grade = "B";
            //    grade.GradePoint = 3;
            //}
            //else if (marks >= 40)
            //{
            //    grade.Grade = "C";
            //    grade.GradePoint = 2;
            //}
            //else if (marks >= 33 && marks<=39)
            //{
            //    grade.Grade = "D";
            //    grade.GradePoint = 1;
            //}
            //else
            //{
            //    grade.Grade = "F";
            //    grade.GradePoint = 0;
            //}

            return grade;
        }

        public struct grade {

            public string Grade { get; set; }
            public double GradePoint { get; set; }
        }

        public ActionResult IndividualGradeSheet(int? examId, int? Roll, int? DeparmentId)
        {
            try
            {
                double totalGPA = 0;
                int flag = 1;
                
                var getResult = DDL.GetSingleResult(Roll.Value,examId,DeparmentId,null);
                StudentInfo student = db.StudentInfoes.Where(t => t.RollNo == Roll).SingleOrDefault();
                FacultyInfo facinfo = db.FacultyInfoes.Where(t => t.Id == DeparmentId).SingleOrDefault();
                List<result> result = new List<result>();

                // get total Subject from StudentInfoSubject Except optional = Y
                int StudentSubject = db.StudentInfoSubjects.Where(t =>t.OptionalSubject != "Y" && t.StudentId == student.Id).Count();


                if (getResult != null)
                {
                   
                    foreach (var item in getResult)
                    {
                        //get is this subjectId is Optional 
                       grade _grade =new grade();
                       if (item.PassStatus == "F")
                       {
                           _grade.Grade = "F";
                           _grade.GradePoint = 0;
                       }
                       else
                       {
                           _grade = getGrade(item.MarksObtain);
                       }

                        if (item.OptionalSubject == "Y")
                        {
                            totalGPA += (_grade.GradePoint - 2) > 0 ? (_grade.GradePoint - 2) : 0;
                        }
                        else
                        {
                            if (flag == 1 && _grade.GradePoint < 1)
                                flag = 0;
                            totalGPA += _grade.GradePoint;
                        }

                        result.Add(new result
                        {
                            SubjectName = item.SubjectName,
                            Grade = _grade.Grade,
                            GradePoint = Convert.ToString(_grade.GradePoint)
                        });

                    }

                    if (flag == 0)
                        totalGPA = 0;
                    else
                        totalGPA = totalGPA / StudentSubject;
                 }

                string GPA = "";

                if (totalGPA < 1)
                    GPA = "0 " + "(F)";
                else if(totalGPA <2)
                    GPA = totalGPA.ToString("0.##") + " (D)";
                else if(totalGPA <3)
                    GPA = totalGPA.ToString("0.##") + " (C)";
                else if (totalGPA < 3.5)
                    GPA = totalGPA.ToString("0.##") + " (B)";
                else if (totalGPA < 4)
                    GPA = totalGPA.ToString("0.##") + " (A-)";
                else if (totalGPA < 5)
                    GPA = totalGPA.ToString("0.##") + " (A)";
                else if (totalGPA < 6)
                    GPA = totalGPA.ToString("0.##") + " (A+)";

                LocalReport lr = new LocalReport();
                ReportDataSource rd = new ReportDataSource();

                lr.ReportPath = Server.MapPath("~/ReporFile/IndividualGradeSheet.rdlc");

                DataTable dtFDRStatement = ConvertToDataTable(result.ToList());
                rd.Name = "SingleResult";
                rd.Value = dtFDRStatement;

                ReportParameter[] parameters = new ReportParameter[] 
                           {
                             new ReportParameter("Name", student.StudentName),
                             new ReportParameter("ClassRoll",Convert.ToString(student.RollNo)),
                             new ReportParameter("Faculty",Convert.ToString(facinfo.FacultyName)),
                             new ReportParameter("GPA",GPA)  // returns "0"  when decimalVar == 0 
                           };

                lr.SetParameters(parameters);
                lr.DataSources.Add(rd);

                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                "<DeviceInfo>" +
                    "  <OutputFormat>PDF</OutputFormat>" +
                    "  <PageWidth>10.5in</PageWidth>" +
                    "  <PageHeight>11in</PageHeight>" +
                    "  <MarginTop>0.5in</MarginTop>" +
                    "  <MarginLeft>1in</MarginLeft>" +
                    "  <MarginRight>1in</MarginRight>" +
                    "  <MarginBottom>0.5in</MarginBottom>" +
                    "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;
                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                renderedBytes = lr.Render(reportType);
                string reportName = "IndGradeSheet-Roll" + Convert.ToString(student.RollNo) + ".pdf";
                return File(renderedBytes, mimeType, reportName);
            }
            catch (Exception ex) {

                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        //public ActionResult IndividualGradeSheetStudent(int? examId)
        //{
        //    try
        //    {
        //        double totalGPA = 0;
        //        int flag = 1;

        //        int UserName = Convert.ToInt32(User.Identity.Name); // Student User Name is his/her roll No


        //        StudentInfo student = db.StudentInfoes.Where(t => t.RollNo == UserName).SingleOrDefault();

        //        FacultyInfo facinfo = db.FacultyInfoes.Where(t => t.Id == student.FacultyId).SingleOrDefault();

        //        List<result> result = new List<result>();

        //       // var list = DDL.GetSingleResult(UserName, examId);

        //        //var list = (from uresult in db.ExamResults.Where(t => t.ExamId == examId && t.StudentId == student.Id).ToList()
        //        //            group uresult by uresult.SubjectId into gresult
        //        //            select new
        //        //            {
        //        //                SubjectId = gresult.Key,
        //        //                StudentId = gresult.First().StudentId,
        //        //                ExamId = gresult.First().ExamId,
        //        //                Marks = gresult.Sum(f => f.MarksObtain),
        //        //            }).ToList();



        //       // var StudentSubject = db.StudentInfoSubjects.Where(t => t.StudentId == student.Id).ToList();

        //        // get total Subject from StudentInfoSubject Except optional = Y
        //        int TotalSubject = db.StudentInfoSubjects.Where(t => t.OptionalSubject != "Y").Count();

        //        string IsOptional = "";

        //        if (list != null)
        //        {

        //            foreach (var item in list)
        //            {
        //                //get is this subjectId is Optional 


        //               // var isOptional = StudentSubject.Where(t => t.SubjectId == item.SubjectId && t.OptionalSubject == "Y").SingleOrDefault();

        //                grade _grade = new grade();
        //                if (item.PassStatus == "F")
        //                {
        //                    _grade.Grade = "F";
        //                    _grade.GradePoint = 0;
        //                }
        //                else
        //                {
        //                    _grade = getGrade(item.MarksObtain);
        //                }

        //                if (item.OptionalSubject == "Y")
        //                {
        //                    totalGPA += (_grade.GradePoint - 2) > 0 ? (_grade.GradePoint - 2) : 0;
        //                }
        //                else
        //                {
        //                    if (flag == 1 && _grade.GradePoint < 1)
        //                        flag = 0;
        //                    totalGPA += _grade.GradePoint;
        //                }

        //                result.Add(new result
        //                {
        //                    SubjectName = item.SubjectName,
        //                    Grade = _grade.Grade,
        //                    GradePoint = Convert.ToString(_grade.GradePoint),
        //                    sl = IsOptional ==""? 1:2
        //                });

        //            }

        //            if (flag == 0)
        //                totalGPA = 0;
        //            else
        //                totalGPA = totalGPA / TotalSubject;


        //        }

        //        string GPA = "";

        //        if (totalGPA < 1)
        //            GPA = "0 " + "(F)";
        //        else if (totalGPA < 2)
        //            GPA = totalGPA.ToString("0.##") + " (D)";
        //        else if (totalGPA < 3)
        //            GPA = totalGPA.ToString("0.##") + " (C)";
        //        else if (totalGPA < 3.5)
        //            GPA = totalGPA.ToString("0.##") + " (B)";
        //        else if (totalGPA < 4)
        //            GPA = totalGPA.ToString("0.##") + " (A-)";
        //        else if (totalGPA < 5)
        //            GPA = totalGPA.ToString("0.##") + " (A)";
        //        else if (totalGPA < 6)
        //            GPA = totalGPA.ToString("0.##") + " (A+)";

        //        LocalReport lr = new LocalReport();
        //        ReportDataSource rd = new ReportDataSource();

        //        lr.ReportPath = Server.MapPath("~/ReporFile/IndividualGradeSheet.rdlc");

        //        DataTable dtFDRStatement = ConvertToDataTable(result.OrderBy(t=>t.sl).ToList());
        //        rd.Name = "SingleResult";
        //        rd.Value = dtFDRStatement;

        //        ReportParameter[] parameters = new ReportParameter[] 
        //                   {
        //                     new ReportParameter("Name", student.StudentName),
        //                     new ReportParameter("ClassRoll",Convert.ToString(student.RollNo)),
        //                     new ReportParameter("Faculty",Convert.ToString(facinfo.FacultyName)),
        //                     new ReportParameter("GPA",GPA)  // returns "0"  when decimalVar == 0 
        //                   };

        //        lr.SetParameters(parameters);
        //        lr.DataSources.Add(rd);

        //        string reportType = "PDF";
        //        string mimeType;
        //        string encoding;
        //        string fileNameExtension;

        //        string deviceInfo =
        //        "<DeviceInfo>" +
        //            "  <OutputFormat>PDF</OutputFormat>" +
        //            "  <PageWidth>10.5in</PageWidth>" +
        //            "  <PageHeight>11in</PageHeight>" +
        //            "  <MarginTop>0.5in</MarginTop>" +
        //            "  <MarginLeft>1in</MarginLeft>" +
        //            "  <MarginRight>1in</MarginRight>" +
        //            "  <MarginBottom>0.5in</MarginBottom>" +
        //            "</DeviceInfo>";

        //        Warning[] warnings;
        //        string[] streams;
        //        byte[] renderedBytes;
        //        renderedBytes = lr.Render(
        //            reportType,
        //            deviceInfo,
        //            out mimeType,
        //            out encoding,
        //            out fileNameExtension,
        //            out streams,
        //            out warnings);

        //        renderedBytes = lr.Render(reportType);
        //        string reportName = "IndGradeSheet-Roll" + Convert.ToString(student.RollNo) + ".pdf";
        //        return File(renderedBytes, mimeType, reportName);
        //    }
        //    catch (Exception ex)
        //    {

        //        ViewBag.Error = ex.Message;
        //        return View("Error");
        //    }
        //}

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    try
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                    catch { }
                }
                table.Rows.Add(row);
            }
            return table;
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