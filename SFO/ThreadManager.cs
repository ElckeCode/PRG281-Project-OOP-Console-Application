using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFO_Class_Divided

{
    public class ThreadManager
    {
        public void StartThread(Action task)
        {
            try
            {
                Thread thread = new Thread(() => task());
                thread.IsBackground = true;
                thread.Start();
                Console.WriteLine("Thread started.");
            }
            catch (Exception ex)
            {
                throw new SensitiveFileException($"Error starting thread: {ex.Message}");
            }
        }

        public void StopThread(Thread thread)
        {
            try
            {
                if (thread != null && thread.IsAlive)
                {
                    thread.Abort(); 
                    Console.WriteLine("Thread stopped.");
                }
            }
            catch (Exception ex)
            {
                throw new SensitiveFileException($"Error stopping thread: {ex.Message}");
            }
        }
    }
}
