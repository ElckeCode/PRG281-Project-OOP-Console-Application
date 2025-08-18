using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO_Class_Divided
{
    public interface IRule
    {
        void ProcessFile(string filePath);
    }
}
