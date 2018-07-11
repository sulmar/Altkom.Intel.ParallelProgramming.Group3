using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
          

            //SendSMS("Hello World");
            //SendPost("Hello World");

            //DelegatesTest();

            //EventsTest();

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
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
