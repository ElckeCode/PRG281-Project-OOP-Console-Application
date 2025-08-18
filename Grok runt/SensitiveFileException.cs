using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grok_runt
{
    // Custom exception for sensitive file operations
    public class SensitiveFileException : Exception
    {
        public SensitiveFileException(string message) : base(message)
        {
        }
    }
}
