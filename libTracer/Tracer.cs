using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace libTracer
{
    [Serializable]
    public class TraceResult
    {
        public List<ThreadInfo> Threads { get; internal set; }        
    }

    public class ThreadInfo
    {
        public int IdThread { get; internal set; }
        public double Time { get; internal set; }
        public List<MethodInfo> Method { get; internal set; }
    }

    public class MethodInfo
    {
        public string Name { get; internal set; }
        public string Class { get; internal set; }
        public double Time { get; internal set; }
        public List<MethodInfo> ChildMethods { get; internal set; }

    }
    public class Tracer : ITracer
    {
        readonly TraceResult _trace = new TraceResult();        
        readonly ConcurrentDictionary<int, Stack<(Stopwatch,MethodInfo)>> _threadInfos = new ConcurrentDictionary<int, Stack<(Stopwatch,MethodInfo)>>();

        public void StartTrace()
        {
            Stopwatch sw = new Stopwatch();
            MethodInfo methodInfo = new MethodInfo();

            sw.Start();
            int id = Thread.CurrentThread.ManagedThreadId;

            if (_trace.Threads == null)
            {
                _trace.Threads = new List<ThreadInfo>();
            }

            if (_threadInfos.TryAdd(id, new Stack<(Stopwatch, MethodInfo)>()))
            {
                _trace.Threads.Add(new ThreadInfo() {IdThread = id});
            }

            StackFrame sf = new StackFrame(1);
            MethodBase mb = sf.GetMethod();
            methodInfo.Name = mb.Name;
            methodInfo.Class = !(mb.DeclaringType is null) ? mb.DeclaringType.Name : "Unknown";
            _threadInfos[id].Push((sw,methodInfo));

        }

        public void StopTrace()
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            Stopwatch sw;
            MethodInfo methodInfo;
            (sw, methodInfo) = _threadInfos[id].Pop();
            methodInfo.Time = sw.ElapsedMilliseconds;
            sw.Stop();
            if (_threadInfos[id].Count > 0)
            {
                MethodInfo mi;
                (_, mi) = _threadInfos[id].Peek();
                if (mi.ChildMethods == null)
                {
                    mi.ChildMethods = new List<MethodInfo>();
                }

                mi.ChildMethods.Add(methodInfo);
            }
            else
            {
                int index = _trace.Threads.FindIndex(thread => thread.IdThread == id);
                if (_trace.Threads[index].Method == null)
                {
                    _trace.Threads[index].Method = new List<MethodInfo>();
                }

                _trace.Threads[index].Method.Add(methodInfo);
                _trace.Threads[index].Time += methodInfo.Time;
            }
        }

        public TraceResult GetTraceResult()
        {
            return _trace;
        }
    }
    
}
