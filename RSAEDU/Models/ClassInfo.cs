using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

using RSAEDU.ViewModel;
using clslib.mddl;

namespace RSAEDU.Models
{

    public partial class UserSubject {

        public int Id { get; set; }
        public string UserId { get; set; }
        public int ClassId { get; set; }
        public int FacultyId { get; set; }
        public int SubjectId { get; set; }
        public string ManualCode {get;set;}
       // public bool IsActive { get; set; }
        public string EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string AuthorizedBy { get; set; }
        public Nullable<System.DateTime> AuthorizedDate { get; set; }


        [NotMapped]
        public string UserName { get; set; }

        [NotMapped]
        public string ClassName { get; set; }

        [NotMapped]
        public string FacultyName { get; set; }

        [NotMapped]
        public string SubjectName { get; set; }
       

    }
    public partial class ClassInfo
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public string EntryBy { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
    }

    public partial class ClassInfoSection
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string SectionName { get; set; }
        public string EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
    }

    public partial class ExamAttendance
    {
        public int Id { get; set; }
        public int ClassId { get; set; }        
        
        //Science.Commerce
        public int FacultyId { get; set; }
        public int SubjectId { get; set; }
        public int StudentId { get; set; }
        public string Present { get; set; }
        public string EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }

        [NotMapped]
        public string ClassName { get; set; }

        [NotMapped]
        public string FacultyName { get; set; }

        [NotMapped]
        public string SubjectName { get; set; }

        [NotMapped]
        public string StudentName { get; set; }

        [NotMapped]
        public int RollNo { get; set; }
    }

    public partial class ExamInfo
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string ExamName { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string ExamStatus { get; set; }

        public string Publish { get; set; } //P
        public string EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string AuthorizedBy { get; set; }
        public Nullable<System.DateTime> AuthorizedDate { get; set; }
    }
    public partial class ExamInfoDetail
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int ExamTypeId { get; set; }
        public int SubjectId { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal PassMarks { get; set; }
        public string EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }

        [NotMapped]
        public string ExamName { get; set; }

        [NotMapped]
        public string ExamType { get; set; }

        [NotMapped]
        public string SubjectName { get; set; }
    }
    public partial class ExamResult
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int ExamTypeId { get; set; }
        public decimal MarksObtain { get; set; }
        public string EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }

        [NotMapped]
        [Display(Name = "Exam Name")]
        public string ExamName { get; set; }

        [NotMapped]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [NotMapped]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [NotMapped]
        [Display(Name = "Question Type")]
        public string QuestionType { get; set; }

        [NotMapped]
        public int RollNo { get; set; }

        [NotMapped]
        public decimal TotalMarks { get; set; }


        [NotMapped]
        public decimal PassMarks { get; set; }

        [NotMapped]
        public List<ResultType> ResultType { get; set; }
        
    }
    public partial class ExamTypeInfo
    {
        [Key]
        public int ExamTypeId { get; set; }
        public string ExamTypeName { get; set; }
        public string EntryBy { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }

        [NotMapped]
        public decimal TotalMark { get; set; }

    }
    public partial class FacultyInfo
    {
        public int Id { get; set; }
        public string FacultyName { get; set; }
        public string EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
    }
    public partial class FacultyInfoSubject
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int FacultyId { get; set; }
        public int SubjectId { get; set; }
        public string Compulsory { get; set; }
        public string EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
    }
    public partial class GetExamType_Result
    {
        [Key]
        public int ExamTypeId { get; set; }
        public string ExamTypeName { get; set; }
        public string EntryBy { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
    }
    public partial class GradeInfo
    {
        public int Id { get; set; }
        public string GradeName { get; set; }
        public decimal GradePoint { get; set; }
        public decimal MarksFrom { get; set; }
        public decimal MarksTo { get; set; }
        public string EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
    }
    public partial class StudentInfo
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string SectionId { get; set; }
        public int FacultyId { get; set; }
        public string StudentTypeId { get; set; }
        public string AcademicSession { get; set; }
        public string Regno { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string PresentAdd { get; set; }
        public string PermanentAdd { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public string Remarks { get; set; }
        public string EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string AuthorizedBy { get; set; }
        public Nullable<System.DateTime> AuthorizedDate { get; set; }
    }

    public partial class StudentInfoSubject
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public string OptionalSubject { get; set; }
        public string EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string AuthorizedBy { get; set; }
        public Nullable<System.DateTime> AuthorizedDate { get; set; }
    }
    public partial class SubjectInfo
    {
        public int Id { get; set; }
        public string ManualCode { get; set; }
        public string SubjectName { get; set; }
        public string ShortName { get; set; }
        public string EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
    }

}