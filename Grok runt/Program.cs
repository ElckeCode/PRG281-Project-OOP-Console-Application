using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace SFO_Class_Divided
{    
    // Main program
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting SFO_Class_Divided Application...");
            RunApp();
        }

        static void RunApp()
        {
            try
            {
                Logger logger = new Logger();
                ThreadManager threadManager = new ThreadManager();
                EncryptionManager encryptionManager = new EncryptionManager();
                CategoryFileProcessor fileProcessor = new CategoryFileProcessor();
                DuplicateManager duplicateManager = new DuplicateManager();
                WatcherService watcherService = new WatcherService();

                // Subscribe to watcher events
                watcherService.FileProcessedEvent += (sender, e) =>
                {
                    logger.LogInfo($"File processed: {e.FilePath} at {e.Timestamp}");
                };

                // Start file system watcher in a separate thread
                threadManager.StartThread(() => watcherService.StartWatching("C:\\Temp"));

                // Example: Process a file
                string sampleFile = "C:\\Temp\\sample.txt";
                fileProcessor.ProcessFile(sampleFile);
                duplicateManager.CheckDuplicates("C:\\Temp");

                Console.WriteLine("Application running. Press any key to exit.");
                Console.ReadKey();
            }
            catch (SensitiveFileException ex)
            {
                Logger logger = new Logger();
                logger.LogError($"Sensitive file error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger logger = new Logger();
                logger.LogError($"Unexpected error: {ex.Message}");
            }
        }
    }
}