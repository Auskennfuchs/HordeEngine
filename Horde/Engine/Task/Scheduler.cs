using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using STask = System.Threading.Tasks.Task;

namespace Horde.Engine.Task {
    public class Scheduler : TaskScheduler {
        [ThreadStatic]
        private static bool currentThreadIsProcessing = false;

        private readonly LinkedList<STask> tasks = new LinkedList<STask>();

        private readonly int maxDegreeOfParallelism;

        public sealed override int MaximumConcurrencyLevel {
            get {
                return maxDegreeOfParallelism;
            }
        }

        private int delegatesQueuedOrRunning = 0;

        public Scheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1)
            {
                throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
            }
            this.maxDegreeOfParallelism = maxDegreeOfParallelism;
        }


        protected sealed override void QueueTask(STask task)
        {
            lock (tasks)
            {
                tasks.AddLast(task);
                if (delegatesQueuedOrRunning < maxDegreeOfParallelism)
                {
                    delegatesQueuedOrRunning++;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }

        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem(_ => {
                currentThreadIsProcessing = true;
                try {
                    while (true) {
                        STask item;
                        lock (tasks) {
                            // Taskliste ist leer, nichts mehr zu tun
                            if (tasks.Count == 0) {
                                delegatesQueuedOrRunning--;
                                break;
                            }

                            item = tasks.First.Value;
                            tasks.RemoveFirst();
                        }
                        base.TryExecuteTask(item);
                    }
                }
                finally {
                    currentThreadIsProcessing = false;
                }
            }, null);
        }

        // Attempts to execute the specified task on the current thread. 
        protected sealed override bool TryExecuteTaskInline(STask task, bool taskWasPreviouslyQueued) {
            // If this thread isn't already processing a task, we don't support inlining
            if (!currentThreadIsProcessing) {
                return false;
            }
            if (taskWasPreviouslyQueued) {
                if (TryDequeue(task)) {
                    return base.TryExecuteTask(task);
                } else {
                    return false;
                }
            } else {
                return base.TryExecuteTask(task);
            }
        }

        protected sealed override bool TryDequeue(STask task) {
            lock (tasks) {
                return tasks.Remove(task);
            }
        }

        protected sealed override IEnumerable<STask> GetScheduledTasks() {
            bool lockTaken = false;
            try {
                Monitor.TryEnter(tasks, ref lockTaken);
                if (lockTaken) {
                    return tasks;
                } else {
                    throw new NotSupportedException();                    
                }
            }
            finally {
                if (lockTaken) {
                    Monitor.Exit(tasks);
                }
            }
        }
    }
}
