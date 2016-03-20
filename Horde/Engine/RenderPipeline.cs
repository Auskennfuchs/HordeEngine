using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX.Direct3D11;
using SlimDX;

namespace Horde.Engine {
    public class RenderPipeline : IDisposable {

        private DeviceContext devContext;

        public DeviceContext DeviceContext {
            get {
                return devContext;
            }
        }

        public RenderPipeline() {
            devContext = new DeviceContext(Renderer.Instance.Device);
        }

        public void Dispose() {
            if (devContext != null) {
                devContext.Dispose();
            }
        }

        public void Draw(int vertexCount, int startVertexLocation) {
            devContext.Draw(vertexCount, startVertexLocation);
        }

        public void DrawIndexed(int indexCount, int startIndexLocation, int baseVertexLocation) {
            devContext.DrawIndexed(indexCount, startIndexLocation, baseVertexLocation);
        }

        public CommandList GenerateCommandList() {
            return devContext.FinishCommandList(false);
        }

        public void ClearRenderTarget(RenderTarget renderTarget, Color4 col) {
            devContext.ClearRenderTargetView(renderTarget.View, col);
        }
    }
}
