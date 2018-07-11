using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altkom.Intel.ParallelProg.Reactive
{



    class Program
    {
        

        static void Main(string[] args)
        {
            //SendSMS("Hello World");
            //SendPost("Hello World");

            DelegatesTest();

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
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
