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

            Console.WriteLine("Welcome to the Stopwatch Application!");
            Console.WriteLine("Commands:");
            Console.WriteLine(" S - Start the stopwatch");
            Console.WriteLine(" T - Stop the stopwatch");
            Console.WriteLine(" R - Reset the stopwatch");
            Console.WriteLine(" E - Exit the application");

            bool exit = false;
            while (!exit)
            {
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
                        Console.WriteLine("Exiting the application. Goodbye!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Command. Please use S, T, R, or E.");
                    }
                };

                handleCommand(command);

                if (!exit && command != "E")
                {
                    Console.WriteLine($"Time Elapsed: {stopwatch.GetElapsedTime()}");
                }
            }
        }

        private static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
