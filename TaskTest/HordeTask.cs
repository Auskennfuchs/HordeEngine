using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TaskTest {
    public class HordeTask {
        Object lockObj = new Object();

        int outputItem = 0;

        public virtual void Execute(int iteration) {
            for (int i = 0; i < 10; i++) {
                lock (lockObj) {
                    int sleep = new Random().Next(200)* Thread.CurrentThread.ManagedThreadId;
                    Console.Write("{0} in task t-{1} on thread {2} - {3}\n",
                                  i, iteration, Thread.CurrentThread.ManagedThreadId,sleep);
                    outputItem++;
                    if (outputItem % 3 == 0) {
                        Console.WriteLine();
                    }
                    Thread.Sleep(sleep);
                }
            }
        }
    }
}
