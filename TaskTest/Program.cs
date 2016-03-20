using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTest {
    class Program {
        static void Main(string[] args) {
            Scheduler scheduler = new Scheduler(4);
            List<Task> tasks = new List<Task>();
            TaskFactory factory = new TaskFactory(scheduler);
            CancellationTokenSource cts = new CancellationTokenSource();

            TaskManager tm = new TaskManager();

            for (int tCtr = 0; tCtr <= 4; tCtr++) {
                tm.QueueTask(new HordeTask());
/*                for (int i = 0; i < 10; i++) {
                    tm.QueueTask(new HeartbeatTask());
                }*/
            }

            tm.ProcessTasks();

            Console.WriteLine("Success");
            Console.ReadKey();
        }
    }
}
