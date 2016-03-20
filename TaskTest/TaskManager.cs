using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTest {
    public class TaskManager : IDisposable{
        Scheduler scheduler;
        List<HordeTask> queueTasks = new List<HordeTask>();
        List<Task> tasks = new List<Task>();        
        TaskFactory factory;
        CancellationTokenSource cts;

        public TaskManager() {
            scheduler = new Scheduler(Environment.ProcessorCount);
            factory = new TaskFactory(scheduler);
            cts = new CancellationTokenSource();
        }

        public void Dispose() {
            cts.Dispose();
        }

        public void QueueTask(HordeTask task) {
            queueTasks.Add(task);
        }

        public void ProcessTasks() {
            foreach (HordeTask ht in queueTasks) {
                Task t = factory.StartNew(_ => {
                    ht.Execute(0);
                }, cts);
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());
            queueTasks.Clear();
        }
    }
}
