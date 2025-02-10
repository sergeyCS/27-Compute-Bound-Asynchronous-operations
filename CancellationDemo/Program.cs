using System;
using System.Threading;

namespace CancellationDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            // Если передать CancellationTokenSource.None, то поток станет неотменяемым
            ThreadPool.QueueUserWorkItem(o => Count(cts.Token, 1000));
            Console.WriteLine("Press <Enter> to cancel the operation.");
            Console.ReadLine();
            cts.Cancel(); // If Count returned already, Cancel has no effect on it
            // Ещё есть метод CancelAfter(int millisecondsDelay)

            // Cancel returns immediately, and the method continues running here…
            Console.ReadLine();
        }

        private static void Count(CancellationToken token, int countTo)
        {
            for (int count = 0; count < countTo; count++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Count is cancelled");
                    break; // Exit the loop to stop the operation
                }
                Console.WriteLine(count);
                Thread.Sleep(200); // For demo, waste some time
            }

            Console.WriteLine("Count is done");
        }
    }
}
