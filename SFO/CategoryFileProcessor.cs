using SFO_Class_Divided;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO_Class_Divided

{
    public class CategoryFileProcessor : FileHandler, IFileProcessor
    {
        public void ProcessFile(string filePath)
        {
            try
            {
                this.filePath = filePath;
                string category = CategorizeFile(filePath);
                MoveToCategory(filePath);
                Console.WriteLine($"Processed file: {filePath}, Category: {category}");
            }
            catch (Exception ex)
            {
                throw new SensitiveFileException($"Error processing file: {ex.Message}");
            }
        }

        public string CategorizeFile(string filePath)
        {
            string extension = Path.GetExtension(filePath)?.ToLower();
            if (extension == ".txt")
                return "Text";
            else if (extension == ".jpg")
                return "Image";
            else
                return "Other";
        }

        public void MoveToCategory(string filePath)
        {
            try
            {
                string category = CategorizeFile(filePath);
                string destinationDir = Path.Combine(Path.GetDirectoryName(filePath), category);
                Directory.CreateDirectory(destinationDir);
                string destinationPath = Path.Combine(destinationDir, Path.GetFileName(filePath));
                File.Move(filePath, destinationPath);
                Console.WriteLine($"Moved {filePath} to {destinationPath}");
            }
            catch (Exception ex)
            {
                throw new SensitiveFileException($"Error moving file: {ex.Message}");
            }
        }
    }
}
