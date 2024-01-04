namespace multi_task_with_cancel
{
    internal class Program
    {
        private const int MaxLoops = 100;
        private const int TotalTasks = 5;

        private static CancellationTokenSource cts = new CancellationTokenSource();

        static void Main(string[] args)
        {
            // This event delegate is called when ctrl+c is pressed by the user.
            Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
            {
                cts.Cancel();
                e.Cancel = true;
            };

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < TotalTasks; i++)
            {
                tasks.Add(Task.Run(() => DoWork(cts.Token)));
            }

            Task.WaitAll(tasks.ToArray());
            WriteConsoleLine($"Process completed.", ConsoleColor.Yellow);

            Console.ResetColor();
         }

        static void DoWork(CancellationToken cancellationToken)
        {
            WriteConsoleLine($"Starting Task {Task.CurrentId}", ConsoleColor.Green);
            
            int counter = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                if (counter >= MaxLoops)
                {
                    break;
                }

                counter++;

                WriteConsoleLine($"Task {Task.CurrentId} loop count: {counter}.", ConsoleColor.Cyan);

                Thread.Sleep(500 + ((Task.CurrentId ?? 0) * 100));
            }

            WriteConsoleLine($"Task {Task.CurrentId} completed with {counter} calls made.", ConsoleColor.Magenta);
        }

        private static void WriteConsoleLine(string s)
        {
            WriteConsoleLine(s, Console.ForegroundColor);
        }

        private static void WriteConsoleLine(string s, ConsoleColor foreColor)
        {
            ConsoleColor current = Console.ForegroundColor;
            Console.ForegroundColor = foreColor;
            Console.WriteLine(s);
            Console.ForegroundColor = current;
        }

        private static void WriteConsole(string s)
        {
            WriteConsole(s, Console.ForegroundColor);
        }

        private static void WriteConsole(string s, ConsoleColor foreColor = ConsoleColor.White)
        {
            ConsoleColor current = Console.ForegroundColor;
            Console.ForegroundColor = foreColor;
            Console.Write(s);
            Console.ForegroundColor = current;
        }

        private static void WritePressAnyKeyAndWait()
        {
            WriteConsole("Press any key...", ConsoleColor.Yellow);
            Console.ReadKey();
        }
    }
}
