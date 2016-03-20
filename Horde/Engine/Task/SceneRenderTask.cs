using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horde.Engine.Task {
    public abstract class SceneRenderTask : HordeTask {
        public abstract void Execute(RenderPipeline pipeline);
    }
}
