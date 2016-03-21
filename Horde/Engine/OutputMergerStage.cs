using SlimDX.Direct3D11;

namespace Horde.Engine {
    public class OutputMergerStage {
        RenderTarget[] renderTargets = new RenderTarget[Constants.SIMULTANEOUS_RENDER_TARGETS];
        RenderTargetView[] rtvs = new RenderTargetView[Constants.SIMULTANEOUS_RENDER_TARGETS];

        public OutputMergerStage() {
            ClearStates();
        }

        public void ClearStates() {
            for(int i = 0; i < Constants.SIMULTANEOUS_RENDER_TARGETS; i++) {
                renderTargets[i] = null;
            }
        }

        public void SetRenderTarget(uint slot, RenderTarget t) {
            if(t!=null) {
                rtvs[slot] = t.View;
            } else {
                rtvs[slot] = null;
            }
        }

        public void ApplyRenderTargets(DeviceContext context) {
            context.OutputMerger.SetTargets(rtvs);
        }
    }
}
