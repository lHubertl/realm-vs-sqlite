using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDbs.Helpers
{
    static class TestingTools
    {
        public static long StartWatching(Action action)
        {
            var watch = Stopwatch.StartNew();

            action();

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public static string Diagnostic(Action action, string messageFormat)
        {
            var time = StartWatching(action);
            var message = string.Format(messageFormat, time);
            Debug.WriteLine(message);
            return message;
        }

        public static async Task<long> StartWatching(Task task)
        {

            var watch = Stopwatch.StartNew();

            if (task != null)
            {
                await task;
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public static async Task<string> Diagnostic(Task task, string messageFormat)
        {
            if(task == null)
            {
                return "Diagnostic error: task can`t be null";
            }

            var time = await StartWatching(task);
            var message = string.Format(messageFormat, time);
            Debug.WriteLine(message);
            return message;
        }

        public static async Task<string> CancelTaskAfterTime(Func<CancellationToken, Task> func, int time, string messageFormat = "{0}ms")
        {
            string message = "";

            try
            {
                var tokenSource = new CancellationTokenSource();
                var diagnosticTask = Task.Run(async () => message = await Diagnostic(func(tokenSource.Token), messageFormat));

                await Task.WhenAny(diagnosticTask, Task.Delay(time));

                tokenSource.Cancel();

                while (diagnosticTask.Status != TaskStatus.RanToCompletion) ;
            }
            catch (OperationCanceledException)
            {

            }

            return message;
        }
    }
}
