using System;
using System.Threading;
using System.Threading.Tasks;

namespace TimerOrTaskDelay
{
    class Program
    {
        private static Timer s_timer;

        static void Main(string[] args)
        {
            Console.WriteLine("Checking status every 2 seconds");

            // Create the Timer ensuring that it never fires. This ensures that
            // s_timer refers to it BEFORE Status is invoked by a thread pool thread
            s_timer = new Timer(Status, null, Timeout.Infinite, Timeout.Infinite);
            
            // Now that s_timer is assigned to, we can let the timer fire knowing
            // that calling Change in Status will not throw a NullReferenceException
            s_timer.Change(0, Timeout.Infinite);
            Console.ReadLine(); // Prevent the process from terminating

            Console.WriteLine("Checking status every 2 seconds");
            Status2();
            Console.ReadLine(); // Prevent the process from terminating
        }

        // This method's signature must match the TimerCallback delegate
        private static void Status(Object state)
        {
            // This method is executed by a thread pool thread
            Console.WriteLine("In Status at {0}", DateTime.Now);
            Thread.Sleep(1000); // Simulates other work (1 second)
                                // Just before returning, have the Timer fire again in 2 seconds
            s_timer.Change(2000, Timeout.Infinite);
            // When this method returns, the thread goes back
            // to the pool and waits for another work item
        }

        // This method can take whatever parameters you desire
        private static async void Status2()
        {
            while (true)
            {
                Console.WriteLine("Checking status at {0}", DateTime.Now);
                // Put code to check status here...
                // At end of loop, delay 2 seconds without blocking a thread
                await Task.Delay(2000); // await allows thread to return
                                        // After 2 seconds, some thread will continue after await to loop around
            }
        }
    }
}
