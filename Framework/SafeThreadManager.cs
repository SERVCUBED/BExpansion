using System;
using System.Diagnostics;
using System.Threading;

namespace BExpansion
{
    /// <summary>
    /// Provides a simple way to manage the amount of threads created.
    /// </summary>
    public class SafeThreadManager
    {
        private readonly int _maxThreadCount;
        private int _currentThreadCount;
        private readonly int _waitInterval;

        /// <summary>
        /// Initialises a new instance of the SafeThreadManager class.
        /// </summary>
        /// <param name="waitInterval">The number of milliseconds to wait between each check 
        /// when the maximum number of threads has been exceeded. A low value is recommended.</param>
        /// <param name="maxThreadCount">The maximum number of threads.</param>
        public SafeThreadManager(int waitInterval = 10, int maxThreadCount = 50)
        {
            _waitInterval = waitInterval;
            _maxThreadCount = maxThreadCount;
        }

        /// <summary>
        /// Performs an action in a theadsafe environment.
        /// </summary>
        /// <param name="func">The function to perform.</param>
        /// <param name="exceptionCallback">The callback for if the function generates an exception.</param>
        /// <param name="threadPriority">The priority of the thread. Default is normal priority.</param>
        public void RunThread(Action func, Action<Exception> exceptionCallback = null, ThreadPriority threadPriority = ThreadPriority.Normal)
        {
            var f = func;
            var ec = exceptionCallback;
            
            // Wait for clear space
            while (_currentThreadCount >= _maxThreadCount)
                Thread.Sleep(_waitInterval);

            new Thread(() =>
            {
                _currentThreadCount++;

                try
                {
                    f();
                }
                catch (Exception ex)
                {
                    ec?.Invoke(ex);
#if DEBUG
                    if (!Debugger.IsAttached)
                        Debugger.Launch();

                    _currentThreadCount--;
                    throw;
#endif
                }

                _currentThreadCount--;
            }) {Priority = threadPriority}.Start();
        }

        /// <summary>
        /// Pause the current thread while all other threads finish.
        /// </summary>
        public void WaitForFinish()
        {
            while (_currentThreadCount > 0)
            {
                Thread.Sleep(_waitInterval);
            }
        }

        /// <summary>
        /// The number of currently running threads.
        /// </summary>
        public int RunningThreads
        {
            get { return _currentThreadCount; }
        }
    }
}
