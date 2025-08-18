using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SFO_Class_Divided
{
    public class DuplicateManager : FileHandler
    {
        // Returns a dictionary of duplicate files (hash -> list of files)
        public Dictionary<string, List<string>> CheckDuplicates(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException("Directory not found.");

            Console.WriteLine($"Checking for duplicates in {directory}...");

            var fileHashes = new Dictionary<string, List<string>>();

            foreach (string file in Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories))
            {
                string hash = ComputeFileHash(file);

                if (!fileHashes.ContainsKey(hash))
                    fileHashes[hash] = new List<string>();

                fileHashes[hash].Add(file);
            }

            var duplicates = fileHashes.Where(kv => kv.Value.Count > 1).ToDictionary(kv => kv.Key, kv => kv.Value);

            if (!duplicates.Any())
                Console.WriteLine("No duplicates found.");
            else
                Console.WriteLine("Duplicate files detected:");

            foreach (var group in duplicates)
            {
                Console.WriteLine($"Hash: {group.Key}");
                foreach (var file in group.Value)
                    Console.WriteLine($"   {file}");
            }

            return duplicates;
        }

        // Deletes duplicates, keeping the first file of each group
        public void DeleteDuplicates(Dictionary<string, List<string>> duplicates)
        {
            foreach (var group in duplicates)
            {
                string fileToKeep = group.Value[0];
                Console.WriteLine($"Keeping: {fileToKeep}");

                for (int i = 1; i < group.Value.Count; i++)
                {
                    string duplicateFile = group.Value[i];
                    try
                    {
                        filePath = duplicateFile; // from FileHandler
                        DeleteFile();
                        Console.WriteLine($"Deleted: {duplicateFile}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to delete {duplicateFile}: {ex.Message}");
                    }
                }
            }

            Console.WriteLine("Duplicate deletion complete.");
        }

        private string ComputeFileHash(string filePath)
        {
            using (var sha = SHA256.Create())
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hashBytes = sha.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
