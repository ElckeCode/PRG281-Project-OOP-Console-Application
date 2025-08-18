using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grok_runt
{
    // Interface for file processors
    public interface IFileProcessor
    {
        void ProcessFile(string filePath);
    }
}
