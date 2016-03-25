﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Horde.Engine {
    class Timer {
        private Stopwatch watch;

        private long frequency;

        public Timer() {
            frequency = Stopwatch.Frequency;
            watch = new Stopwatch();
        }
        
        public void Start() {
            watch.Reset();
            watch.Start();
        } 
        public float Stop() {
            watch.Stop();
            return ((float)watch.ElapsedTicks) / frequency;
        }

        public float Restart() {
            watch.Stop();
            float elapsed = ((float)watch.ElapsedTicks) / frequency; 
            watch.Stop();
            return elapsed;
        }
    }
}
