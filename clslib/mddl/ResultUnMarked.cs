using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clslib.mddl
{
    public class ResultUnMarked
    {
        public int FacultyId { get; set; }
        public int ClassId { get; set; }
        public string Present { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public string ExamName { get; set; }
    }

    public class SingleResult
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int RollNo { get; set; }
        public string FacultyName { get; set; }
        public string StudentName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string OptionalSubject { get; set; }
        public int ExamTypeId { get; set; }
        public string ExamTypeName { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal PassMarks { get; set; }
        public decimal MarksObtain { get; set; }
        public string PassStatus { get; set; }
        public List<Subject> Subject { get; set; }
    }

    public class result
    {
        public string SubjectName { get; set; }
        public string Grade { get; set; }
        public string GradePoint { get; set; }
        public int sl { get; set; }
    }
}
