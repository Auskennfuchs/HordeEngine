using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using Horde.Engine.Task;
using Horde.Engine.Events;

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
        protected Vector3 rot;
        public Vector3 Rotation {
            get {
                return rot;
            }
            set {
                rot = value;
                needUpdate = true;
            }
        }

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
            qrot = Quaternion.RotationYawPitchRoll(rot.X, rot.Y, rot.Z);

            var mat = Matrix.Identity;
            mat = Matrix.Multiply(Matrix.RotationQuaternion(qrot),Matrix.Translation(pos));

            return mat;
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
    }
}
