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

            var mA = GetRandomMatrix(20, 30);
            var mB = GetRandomMatrix(30, 40);

            var result = MultiplySequential(mA, mB);

           // Parallel.For(0, 100, index => MyJob(index));

            IList<string> urls = new List<string>
            {
                "http://www.intel.com",
                "http://www.microsoft.com",
                "http://www.altkom.pl",
            };

            Parallel.ForEach(urls, url => Download(url));


            // ProgressTest();

            //  CancelationTokenTest();

            // cts.CancelAfter(TimeSpan.FromSeconds(3));

            //  await CatchExceptionTest();

            // await AsyncAwaitTest();


            //CreateTaskTest();


            //CreateTaskFactory();

            //CreateTaskFactory2();



            // MultiTaskTest();

            // WaitTaskTest();

            // WhenAllTest();

            // int result = GetLength("http://jsonplaceholder.typicode.com");
            // Console.WriteLine($"Result={result}");

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

            #region Threads

            // CreateThreadTest();

            // CreateParameterThreadTest();

            // CreateParameterLambdaThreadTest();

            // WaitThreadTest();

            // BackgroundTaskTest();

            // MultiThreadTest();

            #endregion

            #region ThreadPool

            // ThreadPoolTest();

            #endregion

            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();

        }


        private static void MyJob(int index)
        {
            // Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} -> {index}");

            Console.Write(index);
        }

        private static void ProgressTest()
        {
            IProgress<int> progress = new Progress<int>(p => Console.Write($".{p}"));

            DoWork(progress);
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

        private static void DoWork(IProgress<int> progress = null)
        {
            DoWork(CancellationToken.None, progress);
        }

        private static void DoWork(CancellationToken token, IProgress<int> progress = null)
        {
            const int size = 100;

            for (int i = 0; i < size; i++)
            {
                if (token!=CancellationToken.None && token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                // Console.Write(".");

                progress?.Report(i*100 / size);

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

        private static async Task AsyncAwaitTest2()
        {
            var result1 = await GetLengthAsync("http://jsonplaceholder.typicode.com").ConfigureAwait(false);
            var result2 = await GetLengthAsync("http://jsonplaceholder.typicode.com");
            var result3 = await GetLengthAsync("http://jsonplaceholder.typicode.com");

            // UI
            var result = result1 + result2 + result3;
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

        private static void CreateTaskFactory()
        {
            Task task = Task.Factory.StartNew(() => Download("http://jsonplaceholder.typicode.com"));
        }

        private static void CreateTaskFactory2()
        {
            Task task = Task.Run(() => Download("http://jsonplaceholder.typicode.com"));
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


        static double[,] MultiplySequential(double[,] matrixA, double[,] matrixB)
        {
            int matrixARows = matrixA.GetLength(0);
            int matrixBCols = matrixB.GetLength(1);

            if (matrixA.GetLength(1) != matrixB.GetLength(0))
            {
                throw new InvalidOperationException("Błędne macierze");
            }

            double[,] result = new double[matrixARows, matrixBCols];

            for (int i = 0; i < matrixARows; i++)
            {
                for (int j = 0; j < matrixBCols; j++)
                {
                    for (int k = 0; k < matrixA.GetLength(1); k++)
                    {
                        result[i,j] += matrixA[i, k] * matrixB[k, j];
                    }                    
                }
            }

            return result;
        }

        static double[,] GetRandomMatrix(int rows, int cols)
        {
            double[,] matrix = new double[rows, cols];

            Random random = new Random();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = random.Next(100);
                }
            }

            return matrix;
        }
    }
}
