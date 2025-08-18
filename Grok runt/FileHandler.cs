using SFO_Class_Divided;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grok_runt
{
        public class FileHandler
        {
            protected string filePath;

            public FileHandler()
            {
                filePath = string.Empty;
            }

            public virtual string ReadFile()
            {
                try
                {
                    if (!File.Exists(filePath))
                        throw new FileNotFoundException("File not found.", filePath);
                    return File.ReadAllText(filePath);
                }
                catch (Exception ex)
                {
                    throw new SensitiveFileException($"Error reading file: {ex.Message}");
                }
            }

            public virtual void WriteFile(string data)
            {
                try
                {
                    File.WriteAllText(filePath, data);
                }
                catch (Exception ex)
                {
                    throw new SensitiveFileException($"Error writing file: {ex.Message}");
                }
            }

            public virtual void DeleteFile()
            {
                try
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    throw new SensitiveFileException($"Error deleting file: {ex.Message}");
                }
            }
        }
   
}
