using SFO_Class_Divided;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grok_runt
{
    // Monitors file system changes
    public class WatcherService
    {
        private FileSystemWatcher watcher;

        public delegate void FileProcessedHandler(object sender, FileProcessedEventArgs e);
        public event FileProcessedHandler FileProcessedEvent;

        public WatcherService()
        {
            watcher = new FileSystemWatcher();
        }

        public void StartWatching(string directory)
        {
            try
            {
                if (!Directory.Exists(directory))
                    throw new DirectoryNotFoundException("Directory not found.");

                watcher.Path = directory;
                watcher.EnableRaisingEvents = true;
                watcher.Created += OnFileCreated;
                watcher.Changed += OnFileChanged;
                Console.WriteLine($"Watching directory: {directory}");
            }
            catch (Exception ex)
            {
                throw new SensitiveFileException($"Error starting watcher: {ex.Message}");
            }
        }

        public void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            FileProcessedEvent?.Invoke(this, new FileProcessedEventArgs(e.FullPath, DateTime.Now));
        }

        public void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            FileProcessedEvent?.Invoke(this, new FileProcessedEventArgs(e.FullPath, DateTime.Now));
        }
    }
}
