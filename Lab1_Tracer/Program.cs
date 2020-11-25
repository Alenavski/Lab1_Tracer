using System.Threading;
using libTracer;

namespace Lab1_Tracer
{
    class Program
    {
        static void Main(string[] args)
        {
            Tracer tr = new Tracer();

            Foo f = new Foo(tr);
            f.MyMethod();

            SomeClass sc = new SomeClass(tr);
            Thread th2 = new Thread(new ThreadStart(sc.NewMethod));
            th2.Start();
            th2.Join();

            TraceResult trR = tr.GetTraceResult();

            ConsoleWritter consoleWritter = new ConsoleWritter();
            Json_Serializer json = new Json_Serializer();
            consoleWritter.Write(json.Serialize(trR));
            Xml_Serializer xml = new Xml_Serializer();
            consoleWritter.Write(xml.Serialize(trR));
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
