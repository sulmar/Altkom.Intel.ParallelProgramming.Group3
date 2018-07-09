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
        // C# 7.1
        static async Task Main(string[] args)
        {
            Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId}");

            for (int i = 0; i < 100; i++)
            {
                Console.Write(".");

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

           //  CancelationTokenTest();

            // cts.CancelAfter(TimeSpan.FromSeconds(3));

          



            //  await CatchExceptionTest();

            // await AsyncAwaitTest();


            // CreateTaskTest();

            // Task task = Task.Factory.StartNew(() => Download("http://jsonplaceholder.typicode.com"));

            // Task task = Task.Run(() => Download("http://jsonplaceholder.typicode.com"));

            // MultiTaskTest();

            //  WaitTaskTest();

            // WhenAllTest();

            //int result = GetLength("http://jsonplaceholder.typicode.com");
            //Console.WriteLine($"Result={result}");

            // BlockingTaskResultTest();

            // TaskTupleTest();

            // ContinueTaskRun();

            // MultiContinueTaskRun();

            // Task.Run(()=>AsyncAwaitTest());



            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine("Marek !");

            //    Thread.Sleep(TimeSpan.FromSeconds(1));
            //}


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

        private static void CancelationTokenTest()
        {
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(7));

            CancellationToken token = cts.Token;

            // DoWorkAsync(token);

             DownloadTaskAsync("https://www.mocky.io/v2/5185415ba171ea3a00704eed?mocky-delay=10000ms", token);

            // manual
            //Console.WriteLine("Press Enter to cancel");
            //Console.ReadLine();
            //cts.Cancel();
        }

        private static Task DoWorkAsync(CancellationToken token)
        {
            return Task.Run(() => DoWork(token), token);
        }

        private static Task DoWorkAsync()
        {
            return Task.Run(() => DoWork());
        }

        private static void DoWork()
        {
            DoWork(CancellationToken.None);
        }

        private static void DoWork(CancellationToken token)
        {
            for (int i = 0; i < 100; i++)
            {
                if (token!=CancellationToken.None && token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                Console.Write(".");

                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
        }

        private static void SyncRun()
        {
            var result = GetLength("http://jsonplaceholder.typicode.com");

            Console.WriteLine($"Result={result}");

            Console.WriteLine("koniec");
        }


        private static async Task CatchExceptionTest()
        {
            try
            {
                await CatchExceptionAsyncTest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static async Task CatchExceptionAsyncTest()
        {
            try
            {
                await ThrowExceptionAsync();
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        private static Task ThrowExceptionAsync()
        {
            return Task.Run(() => ThrowException());
        }

        private static void ThrowException()
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));
            throw new InvalidOperationException();
        }

        private static void MultiContinueTaskRun()
        {
            GetLengthAsync("http://jsonplaceholder.typicode.com")
                    .ContinueWith(t => Task.Run(() => Console.WriteLine($"Result={t.Result}")));
        }

        private static async Task AsyncAwaitTest()
        {
            var result1 = GetLengthAsync("http://jsonplaceholder.typicode.com");
            var result2 = GetLengthAsync("http://jsonplaceholder.typicode.com");
            var result3 = GetLengthAsync("http://jsonplaceholder.typicode.com");

            Task.WaitAll(result1, result2, result3);


            Dictionary<int, int> results = new Dictionary<int, int>();

            for (int i = 0; i < 10; i++)
            {
                var result = await GetLengthAsync("http://jsonplaceholder.typicode.com");

                results.Add(i, result);
            }

            foreach (var item in results)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }

           
            

           // int result2 = await GetLengthAsync("http://jsonplaceholder.typicode.com");

            // var result = result1 + result2;


            // Console.WriteLine($"Result={result}");

        }

        private static Task DownloadAsync(string uri)
        {
            return Task.Run(() => Download(uri));
        }


        private static Task<int> GetLengthAsync(string uri)
        {
            return Task.Run(() => GetLength(uri));
        }


        

        private static void ContinueTaskRun()
        {
            Task<int> task =
                Task.Run(() => GetLength("http://jsonplaceholder.typicode.com"));

            task.ContinueWith(t => Console.WriteLine($"Result={t.Result}"));
        }

        private static void BlockingTaskResultTest()
        {
            Task<int> task = Task.Run(() => GetLength("http://jsonplaceholder.typicode.com"));
            Console.WriteLine($"Result={task.Result}");
        }

        private static void TaskTupleTest()
        {
            var task = Task.Run(() => GetValueTuple());
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


        static async Task DownloadTaskAsync(string uri)
        {
            using (var client = new WebClient())
            {
                Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} downloading {uri}...");
                string content = await client.DownloadStringTaskAsync(uri);

                Thread.Sleep(TimeSpan.FromSeconds(3));

                Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} downloaded {uri} size: {content.Length}.");
            }
        }

        static async Task DownloadTaskAsync(string uri, CancellationToken token)
        {
            using (var client = new WebClient())
            using (var registration = token.Register(() => client.CancelAsync()))
            {
                Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} downloading {uri}...");
                string content = await client.DownloadStringTaskAsync(uri);


                Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} downloaded {uri} size: {content.Length}.");
            }
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

                Thread.Sleep(TimeSpan.FromSeconds(1));

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
