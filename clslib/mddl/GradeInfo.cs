using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clslib.mddl
{
   public  class GradeInfo
    {
      
           public int Id { get; set; }
           public string GradeName { get; set; }
           public decimal GradePoint { get; set; }
           public decimal MarksFrom { get; set; }
           public decimal MarksTo { get; set; }
           public string EntryBy { get; set; }
           public System.DateTime EntryDate { get; set; }
    
    }
}
