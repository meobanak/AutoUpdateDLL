﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServerShared
{
    public class Utility
    {
        public static void EmptyTryCatch(Action action)
        {
            try
            {
                action(); 
            }
            catch
            {
            }
        }


        private class AsyncTask
        {
            public ManualResetEvent doneEvent;
            public Action action;
        }


        public static ManualResetEvent RunAsync(Action action)
        {
            var task = new AsyncTask();

            task.doneEvent = new ManualResetEvent(false);
            task.action = action;

            var thread = new Thread(RunAsyncProc);
            thread.Start(task); 

            return task.doneEvent;
        }


        private static void RunAsyncProc(Object threadContext)
        {
            var task = threadContext as AsyncTask;

            task.action();
            task.doneEvent.Set();  
        }
    }
}
