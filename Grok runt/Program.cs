using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace SFO_Class_Divided
{    

    // Handles AES encryption/decryption
    public class EncryptionManager
    {
        public void EncryptFile(string filePath, string key)
        {
            try
            {
                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                using (Aes aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.GenerateIV();
                    using (FileStream fsOutput = new FileStream(filePath + ".enc", FileMode.Create))
                    {
                        fsOutput.Write(aes.IV, 0, aes.IV.Length);
                        using (FileStream fsInput = new FileStream(filePath, FileMode.Open))
                        using (CryptoStream cs = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            fsInput.CopyTo(cs);
                        }
                    }
                }
                Console.WriteLine($"Encrypted {filePath} to {filePath}.enc");
            }
            catch (Exception ex)
            {
                throw new SensitiveFileException($"Error encrypting file: {ex.Message}");
            }
        }

        public void DecryptFile(string filePath, string key)
        {
            try
            {
                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                using (Aes aes = Aes.Create())
                {
                    byte[] iv = new byte[16];
                    using (FileStream fsInput = new FileStream(filePath, FileMode.Open))
                    {
                        fsInput.Read(iv, 0, iv.Length);
                        aes.Key = keyBytes;
                        aes.IV = iv;
                        using (FileStream fsOutput = new FileStream(filePath + ".dec", FileMode.Create))
                        using (CryptoStream cs = new CryptoStream(fsOutput, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            fsInput.CopyTo(cs);
                        }
                    }
                }
                Console.WriteLine($"Decrypted {filePath} to {filePath}.dec");
            }
            catch (Exception ex)
            {
                throw new SensitiveFileException($"Error decrypting file: {ex.Message}");
            }
        }
    }

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