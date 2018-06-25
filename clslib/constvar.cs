using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace clslib
{

    /// <summary>
    /// Created by Rakibul.13/06/18
    /// Contains All static variable.
    /// </summary>
   public class constvar
   {
             
       public static string Query { get; set; }

       #region sp_SqlParameter
       public static SqlParameter ExamId { get; set; }
       public static SqlParameter SubjectId { get; set; }
       public static SqlParameter ClassId { get; set; }
       public static SqlParameter FacultyId { get; set; }
       public static SqlParameter StudentId { get; set; }
       public static SqlParameter RollNo { get; set; }

       #endregion
   }
}
