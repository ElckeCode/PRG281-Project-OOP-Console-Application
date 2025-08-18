using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO_Class_Divided

{
    class Logger
    {
        private readonly string logFilePath;

        public Logger(string fileName)
        {
            string directory = @"C:\Temp\Logs";

           
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

           
            logFilePath = Path.Combine(directory, fileName);

            
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