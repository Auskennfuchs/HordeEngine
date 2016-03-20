using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TaskTest {
    class HeartbeatTask : HordeTask{

        public override void Execute(int iteration) {
            Console.Write("HeartBeat\n");
            Thread.Sleep(100);
        }
    }
}
