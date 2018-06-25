using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace clslib.mddl
{

    //Deep Copy of Object is Extention Method
    /// <summary>
    ///Additional Info: (you can also read this article from MSDN)
    //Shallow copying is creating a new object and then copying the non-static fields of the current object to the new object. If a field is a value type, a bit-by-bit copy of the field is performed; for a reference type, the reference is copied but the referred object is not; therefore the original object and its clone refer to the same object.
    //Deep copy is creating a new object and then copying the nonstatic fields of the current object to the new object. If a field is a value type, a bit-by-bit copy of the field is performed. If a field is a reference type, a new copy of the referred object is performed.
    /// </summary>
    public static class ExtensionMethods
    {
        // Deep clone
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }

}
