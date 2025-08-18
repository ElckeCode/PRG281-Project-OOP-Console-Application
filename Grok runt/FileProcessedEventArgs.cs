using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO_Class_Divided

{
    // Custom event arguments for file events
    public class FileProcessedEventArgs : EventArgs
    {
        private string filePath;
        private DateTime timestamp;

        public FileProcessedEventArgs(string filePath, DateTime timestamp)
        {
            this.filePath = filePath;
            this.timestamp = timestamp;
        }

        public string FilePath => filePath;
        public DateTime Timestamp => timestamp;
    }
}
