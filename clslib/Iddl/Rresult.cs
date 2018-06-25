using clslib.mddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clslib.Iddl
{
    public class Rresult:IResult
    {
        public List<SingleResult> getSingleResult(List<GradeInfo> gradeInfo, List<SingleResult> singleResult) {

            List<SingleResult> result = new List<SingleResult>();

            string  _subjectName = "first";
            decimal _TotalMarks  = 0;
            string  _Status      = "";
            string  _Optional    = "";
            int     _subjectId   = 0;
            int     _studentId   = 0;
            string  _studentName = "";
            int      _RollNo     = 0;

            if (singleResult != null)
            {

                foreach (var item in singleResult)
                {

                    //2nd subject 
                    if (_subjectName != "first" && _subjectName != item.SubjectName)
                    {
                        if (_Status != "F")
                        { 
                        
                        }
                        result.Add(new SingleResult
                        {

                            SubjectId       = _subjectId,
                            MarksObtain     = _TotalMarks,
                            PassStatus      = _Status,                           
                            SubjectName     = _subjectName + (_Optional == "Y" ? " (Optional)" : ""),
                            OptionalSubject = _Optional,                         
                            StudentId       = _studentId,
                            RollNo          = _RollNo,
                            StudentName     = _studentName                
                        });

                        _TotalMarks = 0;
                        _Status = "";
                        _Optional = "";

                    }
                    if (item.PassStatus == "P")
                    {
                        _TotalMarks += item.MarksObtain;
                        _Status = "P";
                    }
                    else
                    {
                        _TotalMarks = 0; //Failed
                        _Status = "F";
                    }

                    _subjectName = item.SubjectName;
                    _Optional    = item.OptionalSubject;
                    _subjectId   = item.SubjectId;
                    _studentId   = item.StudentId;
                    _studentName = item.StudentName;
                    _RollNo      = item.RollNo;

                }

                //add the last subject
                result.Add(new SingleResult
                {
                    SubjectId       = _subjectId,
                    MarksObtain     = _TotalMarks,
                    PassStatus      = _Status,
                    SubjectName     = _subjectName + (_Optional == "Y" ? " (Optional)" : ""),
                    OptionalSubject = _Optional,
                    StudentId       = _studentId,
                    RollNo          = _RollNo,
                    StudentName     = _studentName
                });

            }

            return result;
        
        }
        
        public List<Subject> getTabulerResult(List<Subject> NewSubject, List<ExamType> type, int RollNo)
        {
            var editedSubject = NewSubject.DeepClone(); // create a new Reference object of NewSubject to editedSubject
            foreach (var sub in editedSubject)
            {
                var getType = type.Where(t => t.RollNo == RollNo && t.SubjectId == sub.SubjectId).ToList();

                if (getType != null && getType.Count() > 0)
                {   
                    sub.ExamTypeList = getType.DeepClone();
                }
            }

            return editedSubject;

        }
    }
}
