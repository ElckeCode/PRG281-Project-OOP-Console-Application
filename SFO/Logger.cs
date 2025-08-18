using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO_Class_Divided

{
    // Handles logging
    class Logger
    {
        private readonly string logFilePath;

        public Logger(string fileName)
        {
            string directory = @"C:\Temp\Logs";

            // Make sure the directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Full file path
            logFilePath = Path.Combine(directory, fileName);

            // Create the file if it doesn't exist
            if (!File.Exists(logFilePath))
            {
                using (StreamWriter sw = File.CreateText(logFilePath))
                {
                    sw.WriteLine($"Log started at {DateTime.Now}");
                }
            }
        }

        public void Log(string message)
        {
            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
        }
    }
}