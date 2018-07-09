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

            // Download();

            // CreateThreadTest();

            // CreateParameterThreadTest();

            // CreateParameterLambdaThreadTest();

            // WaitThreadTest();

            BackgroundTaskTest();

            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();

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

                Thread.Sleep(TimeSpan.FromSeconds(20));

                Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} downloaded {uri} size: {content.Length}.");
            }
        }

        static void Download()
        {
            var uri = "http://www.altkom.pl";

            Download(uri);


        }
    }
}
