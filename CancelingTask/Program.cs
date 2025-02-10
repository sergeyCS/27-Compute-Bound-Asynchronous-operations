using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancelingTask
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task<int> t = Task.Run(() => Sum(cts.Token, 1000000000), cts.Token);
            // Sometime later, cancel the CancellationTokenSource to cancel the Task
            // Thread.Sleep(1000); // Можно дать заданию время поработать
            cts.Cancel(); // This is an asynchronous request, the Task may have completed already
            try
            {
                // If the task got canceled, Result will throw an AggregateException
                Console.WriteLine("The sum is: " + t.Result); // An int value
            }
            catch (AggregateException x)
            {
                // Consider any OperationCanceledException objects as handled.
                // Any other exceptions cause a new AggregateException containing
                // only the unhandled exceptions to be thrown
                x.Handle(e => e is OperationCanceledException);
                // If all the exceptions were handled, the following executes
                Console.WriteLine("Sum was canceled");
            }
        }

        private static int Sum(CancellationToken ct, int n)
        {
            int sum = 0;
            for (; n > 0; n--)
            {
                ct.ThrowIfCancellationRequested();
                checked { sum += n; } // if n is large, this will throw System.OverflowException
            }
            return sum;
        }
    }
}
