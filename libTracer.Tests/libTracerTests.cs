using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace libTracer.Tests
{
    [TestClass]
    public class libTracerTests
    {
        static public Tracer tr;
        private TraceResult trRes;
        
        [TestInitialize]
        public void Setup()
        {
            tr = new Tracer();
            trRes = new TraceResult();
            
        }

        [TestMethod]
        public void StartTrace_MyMethod_returnMyMethod()
        {
            Foo f = new Foo(tr);
            
            f.MyMethod();
            trRes = tr.GetTraceResult();
            
            Assert.AreEqual("MyMethod",trRes.Threads[0].Method[0].Name);
        }
        
        [TestMethod]
        public void GetTraceResult_MyMethod_returnMyMethodInnerMethod()
        {
            Foo f = new Foo(tr);
            
            f.MyMethod();
            trRes = tr.GetTraceResult();
            
            Assert.AreEqual("MyMethod",trRes.Threads[0].Method[0].Name);
            Assert.AreEqual("InnerMethod", trRes.Threads[0].Method[0].ChildMethods[0].Name);
        }
        
        [TestMethod]
        public void StopTrace_ThreadCount_return2()
        {
            SomeClass scl = new SomeClass(tr);
            
            scl.NewMethod();
            trRes = tr.GetTraceResult();
            
            Assert.AreEqual(2, trRes.Threads.Count);
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

                public void TestMethod()
                {
                    _tracer.StartTrace();
                    Thread.Sleep(100);
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
                    Foo f = new Foo(tr);
                    Thread th2 = new Thread(new ThreadStart(f.MyMethod));
                    th2.Start();
                    th2.Join();
                    Thread.Sleep(50);
                    _tracer.StopTrace();
                }
            }
    }
}