using System;
using SlimDX.Direct3D11;

namespace Horde.Engine
{
    public class RenderTarget : IDisposable
    {
        private RenderTargetView rtv;

        public RenderTargetView View
        {
            get { return rtv; }
        }

        public Viewport Viewport
        {
            get; set;
        }

        public RenderTarget(Resource res)
        {
            ReCreate(res);
        }

        public void ReCreate(Resource res)
        {
            Dispose();
            rtv = new RenderTargetView(HordeEngine.Instance.Device, res);
            if (res.GetType() == typeof(Texture2D)) {
                Viewport = new Viewport(0, 0, ((Texture2D)res).Description.Width, ((Texture2D)res).Description.Height);
            }
        }

        public void Dispose()
        {
            if (rtv != null)
            {
                rtv.Dispose();
            }
            Viewport = new Viewport();
        }

        public void Activate()
        {
            HordeEngine.Instance.DeviceContext.OutputMerger.SetTargets(rtv);
            HordeEngine.Instance.DeviceContext.Rasterizer.SetViewports(Viewport);
        }
    }
}
