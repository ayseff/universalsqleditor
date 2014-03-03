using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;

namespace Utilities.Threading
{
    public class AbortableBackgroundWorker : BackgroundWorker
    {
        private Thread _thread;
        private bool _threadAborted;

        public AbortableBackgroundWorker()
        {
            _threadAborted = false;
            WorkerSupportsCancellation = true;
            WorkerReportsProgress = true;
        }

        public Thread CurrentThread
        {
            get { return _thread; }
        }

        protected override void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (_threadAborted)
                return;

            base.OnProgressChanged(e);
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            _threadAborted = false;
            _thread = Thread.CurrentThread;
            try
            {
                base.OnDoWork(e);
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }
        
        public void StopImmediately()
        {
            if (!IsBusy || _thread == null)
            {
                return;
            }
            try
            {
                CancelAsync();
                Thread.SpinWait(10);
                _thread.Abort();
            }
            catch (ThreadAbortException)
            {
                //swallow thread abort in this part, even though it will propogate onwards
            }

            _threadAborted = true;
            AsyncOperation op = GetPrivateFieldValue<AsyncOperation>("asyncOperation");
            SendOrPostCallback completionCallback = GetPrivateFieldValue<SendOrPostCallback>("operationCompleted");
            RunWorkerCompletedEventArgs completedArgs = new RunWorkerCompletedEventArgs(null, null, true);
            op.PostOperationCompleted(completionCallback, completedArgs);
        }

        //type safe reflection
        private TFieldType GetPrivateFieldValue<TFieldType>(string fieldName)
        {
            Type type = typeof(BackgroundWorker);
            FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null)
            {
                object fieldVal = field.GetValue(this);
                return SafeCastTo<TFieldType>(fieldVal);
            }
            else
            {
                throw new MissingFieldException(string.Format("Field {0} could not be found", "fieldName"));
            }
        }

        /// <summary>
        /// Works like a strongly typed "as" operator
        /// If the object is not of the requested type, 
        /// the default value for that type is returns instead of throwing an exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static T SafeCastTo<T>(object obj)
        {
            if (obj == null)
            {
                return default(T);
            }
            if (!(obj is T))
            {
                return default(T);
            }
            return (T)obj;
        }
    }
}
