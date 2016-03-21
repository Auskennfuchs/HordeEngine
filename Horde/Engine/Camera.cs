using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using Horde.Engine.Task;

namespace Horde.Engine {
    public class Camera {
        protected Matrix projMatrix;

        public Matrix ProjectionMatrix {
            get { return projMatrix; }
        }

        public SceneRenderTask SceneRenderTask {
            get; set;
        }

        public void SetProjection(float zNear, float zFar, float aspect, float fov) {
            projMatrix = Matrix.PerspectiveFovLH(fov, aspect, zNear, zFar);
        }

        public void ApplyProjectionParams() {
/*            if(SceneRenderTask) {
                SceneRenderTask-
            }*/
        }
    }
}
