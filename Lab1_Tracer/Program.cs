using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using libTracer;

namespace Lab1_Tracer
{
    class Program
    {
        static void Main(string[] args)
        {
            Tracer tr = new Tracer();
            tr.StartTrace();

            Foo f = new Foo(tr);
            f.MyMethod();

            SomeClass sc = new SomeClass(tr);
            Thread th2 = new Thread(new ThreadStart(sc.NewMethod));
            th2.Start();
            th2.Join();

            tr.StopTrace();
            TraceResult trR = tr.GetTraceResult();
            
            Console.ReadLine();
        }
    }

    public class Foo
    {
        private Bar _bar;
        private ITracer _tracer;

        internal Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }
        public void MyMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(50);
            _bar.InnerMethod();
            _tracer.StopTrace();
        }
    }

    public class Bar
    {
        private ITracer _tracer;

        internal Bar(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void InnerMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(50);
            _tracer.StopTrace();
        }
    }

    public class SomeClass
    {
        private ITracer _tracer;

        internal SomeClass(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void NewMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(100);
            _tracer.StopTrace();
        }
    }
}
