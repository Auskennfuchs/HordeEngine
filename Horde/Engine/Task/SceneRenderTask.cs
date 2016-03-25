using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;

namespace Horde.Engine.Task {
    public abstract class SceneRenderTask : HordeTask {
        public abstract void Execute(RenderPipeline pipeline);

        public Matrix ViewMatrix {
            get; set;
        }

        public Matrix ProjectionMatrix {
            get; set;
        }
    }
}
