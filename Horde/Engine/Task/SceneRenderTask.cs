using SharpDX;

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
