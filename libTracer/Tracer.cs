using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace libTracer
{
    public interface ITracer
    {
        void StartTrace();
        void StopTrace();
        TraceResult GetTraceResult();
    }

    public class TraceResult
    {
        public List<ThreadInfo> Threads { get; private set; }
    }

    public class ThreadInfo
    {
        public int Id_thread { get; private set; }
        public double Time { get; private set; }
        public List<MethodInfo> Method { get; private set; }
    }

    public class MethodInfo
    {
        public string Name { get; private set; }
        public string Class { get; private set; }
        public double Time { get; private set; }
        public List<MethodInfo> ChildMethods { get; private set; }

    }
    public class Tracer : ITracer
    {
        
        public void StartTrace()
        {
            
        }
        public void StopTrace()
        {
          
        }
        public TraceResult GetTraceResult()
        {
            return new TraceResult();
        }
    }
    
}
