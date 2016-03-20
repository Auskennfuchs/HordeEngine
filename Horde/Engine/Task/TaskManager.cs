using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using STask = System.Threading.Tasks.Task;

namespace Horde.Engine.Task {

    public class TaskPayload {
        public HordeTask task;
    }

    public abstract class TaskManager {
        private Scheduler scheduler;
        private List<HordeTask> queuedTasks = new List<HordeTask>();
        public List<HordeTask> QueuedTasks {
            get {
                return queuedTasks;
            }
        }

        private List<STask> tasks = new List<STask>();        
        private TaskFactory factory;
        private CancellationToken ct;

        public TaskManager() {
            scheduler = new Scheduler(Environment.ProcessorCount);
            factory = new TaskFactory(scheduler);
            ct = new CancellationToken();
        }

        public void QueueTask(HordeTask task) {
            queuedTasks.Add(task);
        }

        public void ProcessTasks() {
            lock(queuedTasks) {
                OnProcessTasks();
                queuedTasks.Clear();
            }
        }

        public void StartTask(Action<TaskPayload> action,TaskPayload tp) {

            STask t = factory.StartNew((ttp)=> {
                action((TaskPayload)ttp);
            }, tp);
            tasks.Add(t);
        }

        public void WaitAllTasks() {
            STask.WaitAll(tasks.ToArray());
            tasks.Clear();
        }

        protected abstract void OnProcessTasks();
    }
}
