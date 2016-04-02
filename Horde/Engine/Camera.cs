using System;
using SharpDX;
using Horde.Engine.Task;
using Horde.Engine.Events;
using Horde.Engine.Math;

namespace Horde.Engine {
    public class Camera : EventListener {
        protected Matrix projMatrix;

        public Matrix ProjectionMatrix {
            get { return projMatrix; }
        }

        protected Vector3 pos;
        public Vector3 Position {
            get {
                return pos;
            }
            set {
                pos = value;
                needUpdate = true;
            }
        }

        protected Quaternion qrot;
        public Quaternion QRot {
            get { return qrot; }
        }

/*        protected Vector3 rot;
        public Vector3 Rotation {
            get {
                return rot;
            }
            set {
                rot = value;
                needUpdate = true;
            }
        }*/

        protected Matrix viewMatrix;
        private bool needUpdate = false;

        public Matrix ViewMatrix {
            get {
                if(needUpdate) {
                    UpdateCamMatrices();
                }
                return viewMatrix;
            }
        }

        public SceneRenderTask SceneRenderTask {
            get; set;
        }

        public Camera() {
            viewMatrix = Matrix.Identity;
            projMatrix = Matrix.Identity;
            SceneRenderTask = null;
            qrot = Quaternion.Identity;
        }

        public void SetProjection(float zNear, float zFar, float aspect, float fov) {
            projMatrix = Matrix.PerspectiveFovLH(fov, aspect, zNear, zFar);
        }

        private void UpdateCamMatrices() {
            if(needUpdate) {
                needUpdate = false;
                viewMatrix = UpdateViewMatrix();
            }
        }

        protected virtual Matrix UpdateViewMatrix() {
//            qrot = Quaternion.RotationYawPitchRoll(rot.X, rot.Y, rot.Z);
//            qrot.Normalize();

            return Matrix.Multiply(Matrix.RotationQuaternion(qrot),Matrix.Translation(pos));
        }

        protected void AddRotation(Vector3 axis, float angle) {
            qrot *= Quaternion.RotationAxis(axis, angle);
            qrot.Normalize();
            needUpdate = true;
        }

        public void RenderFrame(Renderer renderer) {
            if(SceneRenderTask!=null) {
                renderer.QueueTask(SceneRenderTask);
            }
            renderer.ProcessTasks();
        }

        public override bool HandleEvent(IEvent ev) {
            throw new NotImplementedException();
        }

        protected Vector3 GetRightVector() {
            return GetRotationVector(Vec3.RIGHT);
        }
        protected Vector3 GetLeftVector() {
            return GetRotationVector(Vec3.LEFT);
        }
        protected Vector3 GetForwardVector() {
            return GetRotationVector(Vec3.FORWARD);
        }
        protected Vector3 GetBackwardVector() {
            return GetRotationVector(Vec3.BACKWARD);
        }
        protected Vector3 GetUpVector() {
            return GetRotationVector(Vec3.UP);
        }
        protected Vector3 GetDownVector() {
            return GetRotationVector(Vec3.DOWN);
        }

        private Vector3 GetRotationVector(Vector3 vecin) {
            return Vector3.Transform(vecin, Quaternion.Conjugate(qrot));
        }
    }
}
