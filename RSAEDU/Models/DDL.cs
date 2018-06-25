using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RSAEDU.Models;
using System.Data.SqlClient;
using clslib;
using clslib.mddl;
using clslib.Iddl;

namespace RSAEDU.Models
{
    public class DDL
    {
        //ApplicationDbContext db = new ApplicationDbContext();


        public static IResult repo = new Rresult();
        public static List<ResultUnMarked> sp_ExamResultUnmarked(int examId, int subjectId, int classId,int facultyId)
        {

            List<ResultUnMarked> result = new List<ResultUnMarked>();

            try
            {
                constvar.Query = "sp_ExamResultUnMarked @ExamId,@SubjectId,@ClassId,@FacultyId";
                constvar.ExamId = new SqlParameter("@ExamId", examId);
                constvar.SubjectId = new SqlParameter("@SubjectId", subjectId);
                constvar.ClassId = new SqlParameter("@ClassId", classId);
                constvar.FacultyId = new SqlParameter("@FacultyId", facultyId);

                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    result = NewContext.Database.SqlQuery<ResultUnMarked>(constvar.Query, constvar.ExamId, constvar.SubjectId, constvar.ClassId, constvar.FacultyId).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }

        }
        public static List<ResultType> sp_ExamResult(int examId, int subjectId,int studentId)
        {

            List<ResultType> subject = new List<ResultType>();

            try
            {
                constvar.Query = "sp_ExamResult @ExamId,@SubjectId,@StudentId";
                constvar.ExamId = new SqlParameter("@ExamId", examId);
                constvar.SubjectId = new SqlParameter("@SubjectId", subjectId);
                constvar.StudentId = new SqlParameter("@StudentId", studentId);

                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    subject = NewContext.Database.SqlQuery<ResultType>(constvar.Query, constvar.ExamId, constvar.SubjectId, constvar.StudentId).ToList();
                }

               

                return subject;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }

        }
        public static List<Subject> sp_ExamSubject(int examId, int facultyId) 
        {
            int flag = 0;
            List<Subject> subject = new List<Subject>();
            List<ExamType> TypeList = new List<ExamType>();       

            try
            {
                constvar.Query = "sp_ExamSubject @ExamId,@FacultyId";
                constvar.ExamId = new SqlParameter("@ExamId", examId);
                constvar.FacultyId = new SqlParameter("@FacultyId", facultyId);
                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    subject = NewContext.Database.SqlQuery<Subject>(constvar.Query, constvar.ExamId, constvar.FacultyId).ToList();
                }

                //now add all Subject ExamTypeInfo
                foreach (var item in subject)
                {                    
                  item.ExamTypeList = sp_ExamSubjectType(examId, facultyId, item.SubjectId);                   
                }              

                return subject;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        
        }
        private static List<ExamType> sp_ExamSubjectType(int examId, int facultyId, int subjectId) {

            List<ExamType> obj = new List<ExamType>();

            try
            {
                constvar.Query = "sp_ExamSubjectType @ExamId,@FacultyId,@SubjectId";

                constvar.ExamId = new SqlParameter("@ExamId", examId);
                constvar.FacultyId = new SqlParameter("@FacultyId", facultyId);
                constvar.SubjectId = new SqlParameter("@SubjectId", subjectId);

                using (ApplicationDbContext NewContext = new ApplicationDbContext())
                {
                    obj = NewContext.Database.SqlQuery<ExamType>(constvar.Query, constvar.ExamId, constvar.FacultyId, constvar.SubjectId).ToList();
                }             

                return obj;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
        private static List<SingleResult> sp_GetsingleResult(int? Roll, int? examId, int? FacultyId, int? SubjectId)
        {
            List<SingleResult> sresult = new List<SingleResult>();

           try
                {

                    constvar.Query = "sp_GetsingleResult @RollNo,@ExamId,@FacultyId,@SubjectId";

                     if(Roll.HasValue)
                        constvar.RollNo = new SqlParameter("@RollNo", Roll);
                     else
                         constvar.RollNo = new SqlParameter("@RollNo", DBNull.Value);

                    if (SubjectId.HasValue)
                        constvar.SubjectId = new SqlParameter("@SubjectId",SubjectId);
                    else
                        constvar.SubjectId = new SqlParameter("@SubjectId", DBNull.Value);

                    constvar.ExamId = new SqlParameter("@ExamId", examId);
                    constvar.FacultyId = new SqlParameter("@FacultyId", FacultyId);

                    using (ApplicationDbContext NewContext = new ApplicationDbContext())
                    {
                        sresult = NewContext.Database.SqlQuery<SingleResult>(constvar.Query, constvar.RollNo, constvar.ExamId,constvar.FacultyId,constvar.SubjectId).ToList();
                    }

                       

                    return sresult;
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    return null;
                }
            }

        public static List<SingleResult> GetSingleResult(int? Roll, int? ExamId, int? FacultyId, int? SubjectId) 
        {

            List<SingleResult> result = new List<SingleResult>();

            List<SingleResult> CallProceduer = sp_GetsingleResult(Roll,ExamId,FacultyId,SubjectId);

           // IResult repo = new Rresult();
           
            result = repo.getSingleResult(null,CallProceduer);

            return result;
        
        }


        public static List<ExamType> GetStudentSubjectTypeResult(int? Roll, int ExamId, int FacultyId, int? SubjectId)
        {
            try
            {

                List<ExamType> type = new List<ExamType>();
                constvar.Query = "sp_GetsingleResult @RollNo,@ExamId,@FacultyId,@SubjectId";

              
               constvar.RollNo = new SqlParameter("@RollNo", DBNull.Value);    
               constvar.SubjectId = new SqlParameter("@SubjectId", DBNull.Value);
               constvar.ExamId = new SqlParameter("@ExamId", ExamId);
               constvar.FacultyId = new SqlParameter("@FacultyId", FacultyId);

               using (ApplicationDbContext NewContext = new ApplicationDbContext())
               {
                   type = NewContext.Database.SqlQuery<ExamType>(constvar.Query, constvar.RollNo, constvar.ExamId, constvar.FacultyId, constvar.SubjectId).ToList();
               }

               return type;
            }
            catch (Exception)
            {
                return null;
            }
        
        }

        public static List<SingleResult> GetTabulerResult(int ExamId, int FacultyId ,List<Subject> subjectList) 
        {
           
            List<SingleResult> allStudentResult = sp_GetExamResultStudent(ExamId, FacultyId);
            List<ExamType> type = GetStudentSubjectTypeResult(null, ExamId, FacultyId, null);

            List<SingleResult> NewStudent = new List<SingleResult>();
      
           

            foreach (var item in allStudentResult)
            {

               
                 //now add all Subject of this Faculty
                List<Subject> NewSubject = new List<Subject>();
                NewSubject = subjectList;

                item.Subject = repo.getTabulerResult(NewSubject,type,item.RollNo);

               
                //NewStudent.Add(
                //     new SingleResult { 
                //      ExamId = item.ExamId,
                //      FacultyName = item.FacultyName,
                //      RollNo = item.RollNo,
                //      PassStatus = item.PassStatus,
                //      StudentId = item.StudentId,
                //      StudentName = item.StudentName,
                //      SubjectName = item.SubjectName,
                //      SubjectId = item.SubjectId,
                //      Subject = item.Subject
                //     }

                //    );
                  
               
               
            }

            //IResult repo = new Rresult();
            //var result = repo.getSingleResult(null, allStudentResult);                       
            return allStudentResult;
        
        }




        public static List<SingleResult> sp_GetExamResultStudent(int ExamId, int FacultyId)
        {
          
            List<SingleResult> sresult = new List<SingleResult>();
            
            try{

                    constvar.Query = "sp_GetExamResultStudent @ExamId,@FacultyId";               
                    constvar.ExamId = new SqlParameter("@ExamId", ExamId);
                    constvar.FacultyId = new SqlParameter("@FacultyId",FacultyId);
                                  
                    using (ApplicationDbContext NewContext = new ApplicationDbContext())
                    {
                        sresult = NewContext.Database.SqlQuery<SingleResult>(constvar.Query,constvar.ExamId,constvar.FacultyId).ToList();
                    }

               


                    return sresult;
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    return null;
                }
        
        }
    }

    
}