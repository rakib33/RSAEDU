using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clslib.mddl;

namespace clslib.Iddl
{
   public interface IResult
    {
       List<SingleResult> getSingleResult(List<GradeInfo> gradeInfo, List<SingleResult> singleResult);
       List<Subject> getTabulerResult(List<Subject> subjectList, List<ExamType> type,int RollNo);
    }
}
