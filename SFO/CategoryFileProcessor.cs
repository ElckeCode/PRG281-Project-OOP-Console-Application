using System;
using System.IO;

namespace SFO_Class_Divided
{
    public class CategoryFileProcessor : FileHandler, IFileProcessor
    {
        // This matches IFileProcessor interface
        public void ProcessFile(string filePath)
        {
            try
            {
                string category = CategorizeFile(filePath);
                MoveToCategory(filePath, category);
                Console.WriteLine($"Processed file: {filePath}, Category: {category}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {filePath}: {ex.Message}");
            }
        }

        // Helper for categorization
        private string CategorizeFile(string filePath)
        {
            string extension = Path.GetExtension(filePath)?.ToLower();

            if (extension == ".txt")
                return "TextFiles";
            else if (extension == ".jpg" || extension == ".png")
                return "Images";
            else if (extension == ".docx" || extension == ".pdf")
                return "Documents";
            else
                return "Other";
        }

        // Helper for moving
        private void MoveToCategory(string filePath, string category)
        {
            string destinationDir = Path.Combine(Path.GetDirectoryName(filePath), category);
            Directory.CreateDirectory(destinationDir);

            string destinationPath = Path.Combine(destinationDir, Path.GetFileName(filePath));

            // Prevent overwriting files
            if (!File.Exists(destinationPath))
            {
                File.Move(filePath, destinationPath);
                Console.WriteLine($"Moved {filePath} → {destinationPath}");
            }
            else
            {
                Console.WriteLine($"Skipped {filePath}, already exists in {category}");
            }
        }
    }
}
