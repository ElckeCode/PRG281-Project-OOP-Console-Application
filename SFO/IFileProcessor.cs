using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO_Class_Divided

{
    // Interface for file processors
    public interface IFileProcessor
    {
        void ProcessFile(string filePath);
    }
}
