using System.Drawing;
using Horde.Engine.Events;
using SlimDX;
using System.Diagnostics;
using Horde.Engine.Math;

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
                AddRotation(Vec3.UP, delta.X * elapsed);
                AddRotation(GetRightVector(), delta.Y * elapsed);
                //                Rotation = new Vector3(Rotation.X+delta.X*elapsed, Rotation.Y+delta.Y*elapsed, Rotation.Z);
                delta = Point.Empty;
                ClampRotation();
            }

            if (keyPressed[(int)ControlKeys.RIGHT]) {
                Position += GetRightVector() * (Speed * elapsed);
            }
            if (keyPressed[(int)ControlKeys.LEFT]) {
                Position += GetLeftVector() * (Speed * elapsed);
            }
            if (keyPressed[(int)ControlKeys.FORWARD]) {
                Position += GetForwardVector() * (Speed * elapsed);
            }
            if (keyPressed[(int)ControlKeys.BACKWARD]) {
                Position += GetBackwardVector() * (Speed * elapsed);
            }
        }

        protected override Matrix UpdateViewMatrix() {
            Debug.WriteLine("Right:" + GetRightVector());

//            qrot = Quaternion.RotationYawPitchRoll(rot.X, rot.Y, rot.Z);
//            qrot.Normalize();

            var mpos = Matrix.Translation(Position*-1.0f);
            var mrot = Matrix.RotationQuaternion(-qrot);

            return Matrix.Multiply(mpos, mrot);
        }

        private void ClampRotation() {
//            rot.Y = System.Math.Min(rot.Y, 1.0f);
//            rot.Y = System.Math.Max(rot.Y, -1.0f);
        }
    }
}
