using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altkom.Intel.ParallelProg.Reactive
{
    class MyObserver : IObserver<decimal>
    {
        public string Name { get; set; }

        public MyObserver(string name)
        {
            Name = name;
        }

        public void OnCompleted()
        {
            Console.WriteLine($"[{Name}] End of transmission");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"[{Name}] Exception {error.Message}");
        }

        public void OnNext(decimal value)
        {
            Console.WriteLine($"[{Name}] {value}");
        }
    }
}
