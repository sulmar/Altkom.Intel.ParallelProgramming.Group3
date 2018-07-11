using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altkom.Intel.ParallelProg.Reactive
{
    class MyColdSource : IObservable<decimal>, IDisposable
    {
      
        public IDisposable Subscribe(IObserver<decimal> observer)
        {
            observer.OnNext(100m);
            observer.OnNext(90.50m);
            observer.OnNext(98.06m);
            observer.OnNext(101.26m);
            observer.OnNext(102.66m);

            observer.OnCompleted();

            return this;
        }

        public void Dispose()
        {
            Console.WriteLine("Disposed");
        }

    }
}
