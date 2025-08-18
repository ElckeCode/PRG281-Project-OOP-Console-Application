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
                Logger logger = new Logger("Log.txt");
                ThreadManager threadManager = new ThreadManager();
                EncryptionManager encryptionManager = new EncryptionManager();
                CategoryFileProcessor fileProcessor = new CategoryFileProcessor();
                DuplicateManager duplicateManager = new DuplicateManager();
                WatcherService watcherService = new WatcherService();

                // Subscribe to watcher events
                watcherService.FileProcessedEvent += (sender, e) =>
                {
                    logger.Log($"File processed: {e.FilePath} at {e.Timestamp}");
                };

                // Start file system watcher in a separate thread
                threadManager.StartThread(() => watcherService.StartWatching("C:\\Temp"));

                // Example: Process a file
                //string sampleFile = "C:\\Temp\\sample.txt";
                //fileProcessor.ProcessFile(sampleFile);
                duplicateManager.CheckDuplicates("C:\\Temp");
                logger.Log("Application started");
                logger.Log("User selected option 1");
                logger.Log("Application closed");

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
    }
}