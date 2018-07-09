using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Altkom.Intel.ParallelProg.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId}");

            // CreateTaskTest();

            // Task task = Task.Factory.StartNew(() => Download("http://jsonplaceholder.typicode.com"));

            // Task task = Task.Run(() => Download("http://jsonplaceholder.typicode.com"));


            // MultiTaskTest();

            //  WaitTaskTest();

            // WhenAllTest();

            //int result = GetLength("http://jsonplaceholder.typicode.com");
            //Console.WriteLine($"Result={result}");


            var t = Task.Run(() => GetValueTuple());
            t.Result.

            Task<int> task = Task.Run(() => GetLength("http://jsonplaceholder.typicode.com"));
            Console.WriteLine($"Result={task.Result}");
            

            // Download();

            // CreateThreadTest();

            // CreateParameterThreadTest();

            // CreateParameterLambdaThreadTest();

            // WaitThreadTest();

            // BackgroundTaskTest();

            // MultiThreadTest();

            // ThreadPoolTest();

            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();

        }

        private static void WhenAllTest()
        {
            var urls = new List<string>
            {
                "http://jsonplaceholder.typicode.com",
                "http://www.microsoft.com",
                "http://www.altkom.pl",
            };

            IList<Task> tasks = urls
                .Select(url => Task.Run(() => Download(url)))
                .ToList();

            Task.WhenAll(tasks).Wait();

            Console.WriteLine("Next job.");
        }

        private static void WaitTaskTest()
        {
            Task task1 = Task.Run(() => Download("http://jsonplaceholder.typicode.com"));
            Task task2 = Task.Run(() => Download("http://jsonplaceholder.typicode.com"));
            Task task3 = Task.Run(() => Download("http://jsonplaceholder.typicode.com"));

            Task.WaitAll(task1, task2);
        }

        private static void MultiTaskTest()
        {
            for (int i = 0; i < 100; i++)
            {
                Task.Run(() => Download("http://jsonplaceholder.typicode.com"));
            }
        }

        private static void CreateTaskTest()
        {
            Task task = new Task(() => Download("http://jsonplaceholder.typicode.com"));
            task.Start();
        }

        private static void ThreadPoolTest()
        {
            ThreadPool.SetMaxThreads(4, 4);
            // ThreadPool.SetMinThreads(40, 40);

            ThreadPool.GetAvailableThreads(out int worker, out int io);

            Console.WriteLine($"worker {worker} io {io}");

            for (int i = 0; i < 100; i++)
            {
                ThreadPool.QueueUserWorkItem(p => Download("http://jsonplaceholder.typicode.com"));
            }
        }

        private static void MultiThreadTest()
        {
            for (int i = 0; i < 100; i++)
            {
                Thread thread = new Thread(() => Download("http://www.google.com"));
                thread.Start();
            }
        }

        private static void BackgroundTaskTest()
        {
            Thread thread = new Thread(() => Download("http://www.altkom.pl"));
            thread.Start();
            thread.IsBackground = true;
        }

        private static void WaitThreadTest()
        {
            Thread thread = new Thread(() => Download("http://www.altkom.pl"));
            thread.Start();

            thread.Join();
        }

        private static void CreateParameterLambdaThreadTest()
        {
            Thread thread = new Thread(() => Download("http://www.altkom.pl"));
            Thread thread2 = new Thread(() => Download("http://www.intel.com"));

            thread.Start();
            thread2.Start();
        }

        private static void CreateParameterThreadTest()
        {
            Thread thread = new Thread(Download2);
            thread.Start("http://www.intel.com");

            Thread thread2 = new Thread(Download2);
            thread2.Start("http://www.altkom.pl");
        }

        private static void CreateThreadTest()
        {
            Thread thread = new Thread(Download);
            thread.Start();
        }

        static void Download2(object parameter)
        {
            string uri = (string)parameter;

            Download(uri);
        }

        static void Download(string uri)
        {
            using (var client = new WebClient())
            {
                Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} downloading {uri}...");
                string content = client.DownloadString(uri);

                Thread.Sleep(TimeSpan.FromSeconds(3));

                Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} downloaded {uri} size: {content.Length}.");
            }
        }

        static int GetLength(string uri)
        {
            using (var client = new WebClient())
            {
                Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} downloading {uri}...");
                string content = client.DownloadString(uri);

                Thread.Sleep(TimeSpan.FromSeconds(3));

                Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} downloaded {uri} size: {content.Length}.");

                return content.Length;
            }
        }

        static void Download()
        {
            var uri = "http://jsonplaceholder.typicode.com";

            Download(uri);
        }


        static Tuple<string, int> GetTuple()
        {
            return new Tuple<string, int>("Hello", 100);
        }

        static ValueTuple<string, int> GetValueTuple2()
        {
            return new ValueTuple<string, int>("Hello", 100);
        }

        static (string klucz, int wartosc) GetValueTuple()
        {
            return ("Hello", 100);
        }
    }
}
