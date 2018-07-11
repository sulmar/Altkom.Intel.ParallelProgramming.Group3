using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Altkom.Intel.ParallelProg.Reactive
{

    class Phone
    {
        public delegate void CallingDelegate(string number);

        // public CallingDelegate Calling;

        public event CallingDelegate Calling; 

      //   public event EventHandler<string> Calling;
   

        public void Call(string number)
        {
            Calling?.Invoke(number);

            // rozmowa

           //  Ending?.Invoke(number);
        }
    }


    class Program
    {
        

        static void Main(string[] args)
        {
            FileSystemWatcherTest();

            // CpuMonitorTest();


            // RandomSourceTest();


            // gorące źródło
            // HotSourceTest();

            // zimne źródło
            //  ReplaySubjectTest();


            // ColdSourceTest();

            //SendSMS("Hello World");
            //SendPost("Hello World");

            //DelegatesTest();

            //EventsTest();

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private static void FileSystemWatcherTest()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var watcher = new FileSystemWatcher(path);
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;  // (!!!)

            // watcher.Created += Watcher_Created;
            //watcher.Deleted += Watcher_Deleted;

            var files = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>
            (
                h => { watcher.Created += h; watcher.Deleted += h; },
                h => { watcher.Created -= h; watcher.Deleted -= h; }
            )
            .Select(p => p.EventArgs)
            ;

            files
                .Where(file => Path.GetExtension(file.Name) == ".txt")
                .Subscribe(file => Console.WriteLine($"{file.ChangeType} {file.Name}"));


            //var created = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>
            //(
            //    h => watcher.Created += h,
            //    h => watcher.Created -= h
            //)
            //.Select(p => p.EventArgs);

            //created.Subscribe(file => Console.WriteLine($"Created {file.Name}"));

            //var deleted = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>
            //(
            //    h => watcher.Deleted += h,
            //    h => watcher.Deleted -= h
            //)
            //.Select(p => p.EventArgs);

            //deleted.Subscribe(file =>
            //{
            //    Console.BackgroundColor = ConsoleColor.Red;
            //    Console.WriteLine($"Deleted {file.Name}");
            //    Console.ResetColor();
            //}
            //);
        }

        private static void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"Deleted {e.Name}");
            Console.ResetColor();
        }

        private static void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Created {e.Name}");
        }

        private static void CpuMonitorTest()
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            // gorące źródło
            var source = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(_ => cpuCounter.NextValue());

            source
                .Subscribe(cpu => Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} [CPU] {cpu}"));

            source
                .Where(cpu => cpu > 70)
                .Subscribe(cpu => Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} [ALERT] {cpu}"));
        }

        private static void RandomSourceTest()
        {
            Random random = new Random();

            // gorące źródło
            var source = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(_ => random.Next(0, 100));

            source
                .Subscribe(amount => Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} [Marcin] {amount}"));

            source
                .Where(amount => amount > 80)
                .Subscribe(amount => Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} [Bartek] {amount}"));
        }

        private static void HotSourceTest()
        {
            var source = new Subject<decimal>();

            MyObserver observer = new MyObserver("Marcin");
            source.Subscribe(observer);

            source.OnNext(100.05m);
            source.OnNext(98.45m);
            source.OnNext(89.05m);
            source.OnNext(106.75m);

            MyObserver observer2 = new MyObserver("Bartek");
            source.Subscribe(observer2);

            source.Subscribe(amount => Console.WriteLine($"[Anna] {amount}"));

            source.Subscribe(
                amount => Console.WriteLine($"[Piotr] {amount}"), // OnNext
                e => Console.WriteLine("[Piotr] ERROR"),                  // OnError
                () => Console.WriteLine("[Piotr] EOT")                  // OnCompleted
                );

            source.OnNext(104.88m);
            source.OnNext(103.93m);

            source.OnCompleted();

            source.OnNext(99.99m);
        }

        private static void ReplaySubjectTest()
        {
            var source = new ReplaySubject<decimal>();

            MyObserver observer = new MyObserver("Marcin");
            source.Subscribe(observer);

            source.OnNext(100.05m);
            source.OnNext(98.45m);
            source.OnNext(89.05m);
            source.OnNext(106.75m);

            MyObserver observer2 = new MyObserver("Bartek");
            source.Subscribe(observer2);

            source.OnNext(104.88m);
            source.OnNext(103.93m);

            source.OnCompleted();
        }

        private static void ColdSourceTest()
        {
            MyColdSource source = new MyColdSource();
            MyObserver observer = new MyObserver("Marcin");
            using (var subsciption = source.Subscribe(observer))
            {

                // ...
            }
        }

        private static void EventsTest()
        {
            Phone phone = new Phone();

            phone.Calling += Console.WriteLine;

            phone.Call("+58 555-543-959");
            phone.Call("+22 555-543-959");
            phone.Call("+58 555-543-959");
            phone.Call("+63 555-543-959");

            // phone.Calling?.Invoke("555-999-000");

            // phone.Ending?.Invoke("54553453");
        }

        private static void DelegatesTest()
        {
            Send send = null;

            send += SendSMS;
            send += SendSMS;
            send += SendPost;
            // send += SendEmail;
            send += Console.WriteLine;

            // metoda anonimowa (metoda inline)
            send += delegate (string tweet)
            {
                Console.WriteLine($"Send tweet {tweet}");
            };

            send += tweet => Console.WriteLine($"Send tweet {tweet}");

            send?.Invoke("Hello World");

            send -= SendSMS;
            // send("Hello Intel");

            var methods = send.GetInvocationList().ToList();

            send = null;


            send?.Invoke("Hello Altkom");
        }

        public delegate void Send(string message);

        public static void SendEmail(string subject, string message )
        {

        }

        public static void SendSMS(string message) => Console.WriteLine($"send sms {message}");

        public static void SendPost(string post) => Console.WriteLine($"send to facebook {post}");
    }
}
