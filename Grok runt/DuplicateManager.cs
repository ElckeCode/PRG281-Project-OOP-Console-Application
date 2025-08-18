using SFO_Class_Divided;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO_Class_Divided
{
    // Handles duplicate detection and deletion
    public class DuplicateManager : FileHandler
    {
        public bool CheckDuplicates(string directory)
        {
            try
            {
                if (!Directory.Exists(directory))
                    throw new DirectoryNotFoundException("Directory not found.");
                // Placeholder: Implement duplicate detection logic (e.g., hash comparison)
                Console.WriteLine($"Checking duplicates in {directory}");
                return false; // Return true if duplicates found
            }
            catch (Exception ex)
            {
                throw new SensitiveFileException($"Error checking duplicates: {ex.Message}");
            }
        }

        public void DeleteDuplicate(string file)
        {
            try
            {
                filePath = file;
                DeleteFile();
                Console.WriteLine($"Duplicate file deleted: {file}");
            }
            catch (Exception ex)
            {
                throw new SensitiveFileException($"Error deleting duplicate: {ex.Message}");
            }
        }
    }
}
