using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Horde.Engine.Events;
using SlimDX;

namespace Horde.Engine {
    class FirstPersonCamera : Camera{

        enum ControlKeys {
            FORWARD,
            BACKWARD,
            LEFT,
            RIGHT,
            NUM_KEYS
        }

        private Timer timer;

        private bool[] keyPressed = new bool[(int)ControlKeys.NUM_KEYS];

        public float Speed {
            get; set;
        }


        public FirstPersonCamera() {
            RegisterEvent(EventType.KEYDOWN);
            RegisterEvent(EventType.KEYUP);
            RegisterEvent(EventType.FRAME_START);
            timer = new Timer();
            timer.Start();
            Speed = 1.0f;
        }

        public override bool HandleEvent(IEvent ev) {
            switch(ev.GetEventType()) {
                case EventType.KEYDOWN: {
                        var kev = (EventKeyDown)ev;
                        switch (kev.Data.keyCode) {
                            case System.Windows.Forms.Keys.D:
                                keyPressed[(int)ControlKeys.RIGHT] = true;
                                return true;
                            case System.Windows.Forms.Keys.A:
                                keyPressed[(int)ControlKeys.LEFT] = true;
                                return true;
                            case System.Windows.Forms.Keys.W:
                                keyPressed[(int)ControlKeys.FORWARD] = true;
                                return true;
                            case System.Windows.Forms.Keys.S:
                                keyPressed[(int)ControlKeys.BACKWARD] = true;
                                return true;
                        }
                    }
                    break;
                case EventType.KEYUP: {
                        var kev = (EventKeyUp)ev;
                        switch (kev.Data.keyCode) {
                            case System.Windows.Forms.Keys.D:
                                keyPressed[(int)ControlKeys.RIGHT] = false;
                                return true;
                            case System.Windows.Forms.Keys.A:
                                keyPressed[(int)ControlKeys.LEFT] = false;
                                return true;
                            case System.Windows.Forms.Keys.W:
                                keyPressed[(int)ControlKeys.FORWARD] = false;
                                return true;
                            case System.Windows.Forms.Keys.S:
                                keyPressed[(int)ControlKeys.BACKWARD] = false;
                                return true;
                        }
                    }
                    break;
                case EventType.FRAME_START: {
                        Update();
                    }
                    break;
            }
            return false;
        }

        private void Update() {
            float elapsed = timer.Restart();
            if(keyPressed[(int)ControlKeys.RIGHT]) {
                Position -= new Vector3(Speed, 0, 0)*elapsed;
            }
            if (keyPressed[(int)ControlKeys.LEFT]) {
                Position += new Vector3(Speed, 0, 0) * elapsed;
            }
            if (keyPressed[(int)ControlKeys.FORWARD]) {
                Position -= new Vector3(0, 0, Speed) * elapsed;
            }
            if (keyPressed[(int)ControlKeys.BACKWARD]) {
                Position += new Vector3(0, 0, Speed) * elapsed;
            }
        }

    }
}
