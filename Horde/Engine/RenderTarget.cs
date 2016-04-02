using System;
using SharpDX.Direct3D11;
using SharpDX;

namespace Horde.Engine { 
    public class RenderTarget : IDisposable {
        private RenderTargetView rtv;

        public RenderTargetView View {
            get { return rtv; }
        }

        public Viewport Viewport {
            get; set;
        }

        public RenderTarget(Resource res) {
            ReCreate(res);
        }

        public void ReCreate(Resource res) {
            Dispose();
            rtv = new RenderTargetView(Renderer.Instance.Device, res);
            if (res.GetType() == typeof(Texture2D)) {
                SetViewport(0, 0, ((Texture2D)res).Description.Width, ((Texture2D)res).Description.Height);
            }
        }

        public void Dispose() {
            if (rtv != null) {
                rtv.Dispose();
            }
            Viewport = new Viewport();
        }

        public void Activate(RenderPipeline pipeline) {
//            pipeline.DeviceContext.OutputMerger.SetTargets(rtv);
            pipeline.DeviceContext.Rasterizer.SetViewport(Viewport);
        }

        public void Resize(int width, int height, Resource src) {
            ReCreate(src);
        }

        public void SetViewport(int x, int y, int width, int height) {
            Viewport = new Viewport(x, y, width, height);
        }
    }
}
