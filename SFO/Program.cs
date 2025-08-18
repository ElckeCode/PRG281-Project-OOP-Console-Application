using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Serialization;

namespace SFO_Class_Divided
{
    class Program
    {
        public enum Menu
        {
            StartWatching = 1,
            DeleteDuplicates,
            EncryptFiles,
            DecryptFiles,
            CategorizeFiles,
            Exit
        }
        public static void ShowMenu()
        {
            foreach (Menu option in Enum.GetValues(typeof(Menu)))
            {
                Console.WriteLine($"{(int)option}. {option}");
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Starting SFO_Class_Divided Application...");
            bool running = true;
            string mainDirectory = "C:\\Temp\\Watched";
            while (running)
            {
                ShowMenu();
                Console.WriteLine("Please select an option:");
                if (int.TryParse(Console.ReadLine(), out int choice)
                    && Enum.IsDefined(typeof(Menu), choice))
                {
                    Menu selected = (Menu)choice;
                    switch (selected)
                    {
                        case Menu.StartWatching:
                            Console.Clear();
                            Console.WriteLine("You selected Start Watching");
                            RunApp();
                        break;

                        case Menu.DeleteDuplicates:
                            Console.Clear();
                            Console.WriteLine("You selected Delete Duplicates");
                            DuplicateManager duplicateManager = new DuplicateManager();
                            duplicateManager.DeleteDuplicates(duplicateManager.CheckDuplicates(mainDirectory));
                        break;

                        case Menu.EncryptFiles:
                            Console.Clear();
                            EncryptionManager encryptionManager = new EncryptionManager();
                            Console.WriteLine("You selected Encrypt Files");
                            Console.WriteLine("Please give me a password for encryption:");
                            string password = Console.ReadLine();
                            encryptionManager.EncryptSensitiveFiles(mainDirectory, password);
                        break;
                        case Menu.DecryptFiles:
                            Console.Clear();
                            EncryptionManager decryptionManager = new EncryptionManager();
                            Console.WriteLine("You selected Decrypt Files");
                            Console.WriteLine("Please give me a password for decryption:");
                            string decryptPassword = Console.ReadLine();
                            decryptionManager.DecryptSensitiveFiles(mainDirectory, decryptPassword);
                        break;
                        case Menu.CategorizeFiles:
                            Console.Clear();
                            Console.WriteLine("You selected Categorize Files");
                            CategorizeFiles(mainDirectory);
                            Console.WriteLine("Files have been categorized.");
                        break;

                        case Menu.Exit:
                            Console.WriteLine("Goodbye");
                            running = false;
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            }
        }

        static void RunApp()
        {
            try
            {
                Logger logger = new Logger("Log.txt");
                ThreadManager threadManager = new ThreadManager();
                CategoryFileProcessor fileProcessor = new CategoryFileProcessor();
                WatcherService watcherService = new WatcherService();

                watcherService.FileProcessedEvent += (sender, e) =>
                {
                    logger.Log($"File processed: {e.FilePath} at {e.Timestamp}");
                };

                threadManager.StartThread(() => watcherService.StartWatching("C:\\Temp\\Watched"));

                Console.WriteLine("Logs written to log.txt");
                Console.WriteLine("Application running. Press any key to exit.");
                Console.ReadKey();
            }
            catch (SensitiveFileException ex)
            {
                Logger logger = new Logger("Log.txt");
                logger.Log($"Sensitive file error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger logger = new Logger("Log.txt");
                logger.Log($"Unexpected error: {ex.Message}");
            }
        }

        public static void CategorizeFiles(string sourceDirectory)
        {
            var categories = new Dictionary<string, string>
            {
                { ".txt", "TextFiles" },
                { ".jpg", "Images" },
                { ".png", "Images" },
                { ".docx", "Documents" },
                { ".pdf", "Documents" }
            };

            var files = Directory.GetFiles(sourceDirectory);
            Console.WriteLine($"Found {files.Length} file(s) in {sourceDirectory}:");
            foreach (var file in files)
            {
                Console.WriteLine($"- {file} (Extension: {Path.GetExtension(file).ToLower()})");
            }

            int movedCount = 0;
            foreach (var file in files)
            {
                string extension = Path.GetExtension(file).ToLower();
                if (categories.TryGetValue(extension, out string folderName))
                {
                    string targetFolder = Path.Combine(sourceDirectory, folderName);
                    Directory.CreateDirectory(targetFolder);

                    string targetPath = Path.Combine(targetFolder, Path.GetFileName(file));
                    try
                    {
                        File.Move(file, targetPath);
                        movedCount++;
                        Console.WriteLine($"Moved: {file} -> {targetPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error moving {file}: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Skipped: {file} (No category for extension '{extension}')");
                }
            }
            if (movedCount == 0)
            {
                Console.WriteLine("No files were categorized. Check file extensions and directory contents.");
            }
            else
            {
                Console.WriteLine($"{movedCount} file(s) categorized.");
            }
        }
    }
}