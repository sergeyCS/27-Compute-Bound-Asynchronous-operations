using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SyncContextTaskScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TaskScheduler m_syncContextTaskScheduler;
        private CancellationTokenSource m_cts;

        public MainWindow()
        {
            InitializeComponent();
            m_syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Title = "Synchronization Context Task Scheduler Demo";
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (m_cts != null)
            { // An operation is in flight, cancel it
                m_cts.Cancel();
                m_cts = null;
            }
            else
            { // An operation is not in flight, start it
                Title = "Operation running";
                m_cts = new CancellationTokenSource();
                // This task uses the default task scheduler and executes on a thread pool thread
                Task<int> t = Task.Run(() => Sum(m_cts.Token, 200000000), m_cts.Token);
                // These tasks use the sync context task scheduler and execute on the GUI thread
                t.ContinueWith(task => Title = "Result: " + task.Result, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, m_syncContextTaskScheduler);
                t.ContinueWith(task => Title = "Operation canceled", CancellationToken.None, TaskContinuationOptions.OnlyOnCanceled, m_syncContextTaskScheduler);
                t.ContinueWith(task => Title = "Operation faulted", CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, m_syncContextTaskScheduler);
            }

            base.OnMouseDown(e);
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
