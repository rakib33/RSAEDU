using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clslib.mddl
{
    
    public class Exam
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        List<Subject> SubjectList { get; set; }

    }

    [Serializable] 
    public class Subject
    {

        public int ExamId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string ShortName { get; set; }
        public string FacultyName { get; set; }
        public List<ExamType> ExamTypeList { get; set; }
    }

    [Serializable] 
    public class ExamType
    {
        public int ExamId { get; set; }
        public string FacultyName { get; set; }
        public int StudentId { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string OptionalSubject { get; set; }
        public int ExamTypeId { get; set; }
        public string ExamTypeName { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal PassMarks { get; set; }
        public decimal MarksObtain { get; set; }
        public decimal Total { get; set; }
        public decimal GPA { get; set; }
        public string PassStatus { get; set; }
    }

    public class ResultType :_ResultType
    {
      
     
    }

   public class _ResultType {

        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int ExamTypeId { get; set; }
        public string ExamTypeName { get; set; }
        public decimal MarksObtain { get; set; }
    
    }



}
