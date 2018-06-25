using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RSAEDU.Models;

namespace RSAEDU.ViewModel
{
    public class View_ExamTypeDetails
    {
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
      
        public decimal? TotalMark { get; set; }
        public decimal? PassMark { get; set; }

        public bool chk { get; set; }

    }


    public class View_ExamDetail {
        public ExamInfo Examinfo { get; set; }
        public List<View_ExamTypeDetails> TypeDetails { get; set; }
    }


    public class View_StudentPresent {
      
        public int RollNo { get; set; }
        public string StudentName { get; set; }

        public int StudentId { get; set; }
        public string IsCheck { get; set; }
    }

    public class View_Attendence {
        public int ClassId { get; set; }
        public int FacultyId { get; set; }
        public int SubjectId { get; set; }
        public List<View_StudentPresent> StuList { get; set; }

    }


    public class View_Result{
        public int ExamId { get; set; }
        public int DeparmentId { get; set; }
        public int Subject { get; set; }

        public List<View_ResultDetails> StuList { get; set; }
    }

    public class View_ResultDetails {

        public string IsCheck { get; set; }
        public string StudentName { get; set; }
        public int RollNo { get; set; }
        public int StudentId { get; set; }

        public decimal? CQ { get; set; }
        public decimal? MCQ { get; set; }
        public decimal? Practical { get; set; }
    
    }


    public class View_ExamTypeInfo {
       public int ExamTypeId {get; set;}
       public string  ExamTypeName {get; set;}
       public decimal? TotalMark { get; set; }
    
    }

    public class View_Subject {

        public int Id { get; set; }
        public string SubjectName { get; set; }

        public string ManualCode { get; set; }
        public string IsCheck { get; set; }
    }

}
