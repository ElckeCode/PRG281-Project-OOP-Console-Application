using SFO_Class_Divided;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO_Class_Divided

{
    public class FileHandler
        {
            protected string filePath;
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
