using System;
using System.Threading;

namespace TimerApplication
{
    public delegate void StopwatchEventHandler(string message);

    class Stopwatch
    {
        public event StopwatchEventHandler OnStarted;
        public event StopwatchEventHandler OnStopped;
        public event StopwatchEventHandler OnReset;

        private int timeElapsed;
        private bool isRunning;

        public Stopwatch()
        {
            timeElapsed = 0;
            isRunning = false;
        }

        public void Start()
        {
            if (isRunning) return;

            isRunning = true;
            OnStarted?.Invoke("Stopwatch Started!");

            new Thread(() =>
            {
                while (isRunning)
                {
                    Thread.Sleep(1000);
                    IncrementTime();
                }
            }).Start();
        }

        public void Stop()
        {
            if (!isRunning) return;

            isRunning = false;
            OnStopped?.Invoke("Stopwatch Stopped!");
        }

        public void Reset()
        {
            isRunning = false;
            timeElapsed = 0;
            OnReset?.Invoke("Stopwatch Reset!");
        }

        private void IncrementTime()
        {
            timeElapsed++;
        }

        public string GetElapsedTime()
        {
            var time = TimeSpan.FromSeconds(timeElapsed);
            return time.ToString("hh\\:mm\\:ss");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.OnStarted += DisplayMessage;
            stopwatch.OnStopped += DisplayMessage;
            stopwatch.OnReset += DisplayMessage;

            ShowHeader();

            bool exit = false;
            while (!exit)
            {
                ShowCommands();
                Console.Write("Enter Command: ");
                string command = Console.ReadLine()?.Trim().ToUpper();

                Action<string> handleCommand = cmd =>
                {
                    if (cmd == "S") stopwatch.Start();
                    else if (cmd == "T") stopwatch.Stop();
                    else if (cmd == "R") stopwatch.Reset();
                    else if (cmd == "E")
                    {
                        exit = true;
                        stopwatch.Stop();
                        ShowGoodbye();
                    }
                    else
                    {
                        ShowInvalidCommand();
                    }
                };

                handleCommand(command);

                if (!exit && command != "E")
                {
                    ShowElapsedTime(stopwatch.GetElapsedTime());
                }
            }
        }

        private static void ShowHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
            Console.WriteLine("       Welcome to the Stopwatch App!    ");
            Console.WriteLine("========================================");
            Console.ResetColor();
        }

        private static void ShowCommands()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nCommands:");
            Console.WriteLine(" S - Start the stopwatch");
            Console.WriteLine(" T - Stop the stopwatch");
            Console.WriteLine(" R - Reset the stopwatch");
            Console.WriteLine(" E - Exit the application");
            Console.ResetColor();
        }

        private static void DisplayMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n" + message);
            Console.ResetColor();
        }

        private static void ShowElapsedTime(string time)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\nTime Elapsed: {time}");
            Console.ResetColor();
        }

        private static void ShowInvalidCommand()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nInvalid Command. Please use S, T, R, or E.");
            Console.ResetColor();
        }

        private static void ShowGoodbye()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nExiting the application. Goodbye!");
            Console.ResetColor();
        }
    }
}
