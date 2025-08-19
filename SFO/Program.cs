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
            StopWatching,
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
            Logger logger = new Logger("Log.txt");
            ThreadManager threadManager = new ThreadManager();
            WatcherService watcherService = new WatcherService();
            Thread watcherThread = null;
            DuplicateManager duplicateManager = new DuplicateManager();
            EncryptionManager encryptionManager = new EncryptionManager();
            EncryptionManager decryptionManager = new EncryptionManager();
            CategoryFileProcessor categoryFileProcessor = new CategoryFileProcessor();
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
                            watcherService.FileProcessedEvent += (sender, e) =>
                            {
                                logger.Log($"File processed: {e.FilePath} at {e.Timestamp}");
                            };
                            watcherThread = threadManager.StartThread(() =>
                            {
                                watcherService.StartWatching("C:\\Temp\\Watched");
                                Thread.Sleep(Timeout.Infinite);
                            });
                            if (watcherThread.IsAlive == true)
                            {
                                Console.WriteLine("Watcher thread is running.");
                            }
                            else
                            {
                                Console.WriteLine("Watcher thread failed to start.");
                            }
                            break;

                        case Menu.StopWatching:
                            Console.Clear();
                            Console.WriteLine("You selected Stop Watching");
                            threadManager.StopThread(watcherThread);
                            if (watcherThread.IsAlive == false)
                            {
                                Console.WriteLine("Watcher thread has been stopped.");
                            }
                            else
                            {
                                Console.WriteLine("Watcher thread is still running.");
                            }
                            break;

                        case Menu.DeleteDuplicates:
                            Console.Clear();
                            Console.WriteLine("You selected Delete Duplicates");
                            duplicateManager.DeleteDuplicates(duplicateManager.CheckDuplicates(mainDirectory));
                        break;

                        case Menu.EncryptFiles:
                            Console.Clear();
                            Console.WriteLine("You selected Encrypt Files");
                            Console.WriteLine("Please give me a password for encryption:");
                            string password = Console.ReadLine();
                            encryptionManager.EncryptSensitiveFiles(mainDirectory, password);
                        break;

                        case Menu.DecryptFiles:
                            Console.Clear();
                            Console.WriteLine("You selected Decrypt Files");
                            Console.WriteLine("Please give me a password for decryption:");
                            string decryptPassword = Console.ReadLine();
                            decryptionManager.DecryptSensitiveFiles(mainDirectory, decryptPassword);
                        break;

                        case Menu.CategorizeFiles:
                            Console.Clear();
                            Console.WriteLine("You selected Categorize Files");
                            foreach (var file in Directory.GetFiles(mainDirectory))
                            {
                                categoryFileProcessor.ProcessFile(file);
                            }

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
    }
}