using System.Drawing;
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

        private bool mousedown = false;
        private Point mousepos, delta;

        public float Speed {
            get; set;
        }


        public FirstPersonCamera() {
            RegisterEvent(EventType.KEYDOWN);
            RegisterEvent(EventType.KEYUP);
            RegisterEvent(EventType.FRAME_START);
            RegisterEvent(EventType.MOUSE_DOWN);
            RegisterEvent(EventType.MOUSE_UP);
            RegisterEvent(EventType.MOUSE_MOVE);
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
                case EventType.MOUSE_DOWN: {
                        var mev = (EventMouse)ev;
                        if(mev.Data.button==System.Windows.Forms.MouseButtons.Left) {
                            mousedown = true;
                            mousepos = mev.Data.position;
                        }
                        break;
                    }
                case EventType.MOUSE_UP: {
                        var mev = (EventMouse)ev;
                        if (mev.Data.button == System.Windows.Forms.MouseButtons.Left) {
                            mousedown = false;
                        }
                        break;
                    }
                case EventType.MOUSE_MOVE: {
                        var mev = (EventMouse)ev;
                        if (mousedown) {
                            delta = new Point(delta.X + mousepos.X - mev.Data.position.X, 
                                              delta.Y + mousepos.Y - mev.Data.position.Y);
                            mousepos = mev.Data.position;
                        }
                        break;
                    }
                case EventType.FRAME_START: {
                        Update();
                    }
                    break;
            }
            return false;
        }

        private void Update() {
            float elapsed = timer.Restart();
            if (mousedown) {
                Rotation += new Vector3(delta.X, delta.Y, 0.0f) * elapsed;
                delta = Point.Empty;
            }

            Vector3 addPos = Vector3.Zero;

            if (keyPressed[(int)ControlKeys.RIGHT]) {
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

        protected override Matrix UpdateViewMatrix() {
            qrot = Quaternion.RotationYawPitchRoll(rot.X, rot.Y, rot.Z);

            var mat = Matrix.Identity;
            mat = Matrix.Multiply(Matrix.Translation(pos), Matrix.RotationQuaternion(qrot));

            return mat;
        }

    }
}
