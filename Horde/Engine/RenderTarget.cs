using SlimDX.Direct3D11;

namespace Horde.Engine
{
    public class RenderTarget
    {
        private RenderTargetView rtv;

        public RenderTargetView View
        {
            get { return rtv; }
        }          

        public RenderTarget(Resource res)
        {
            ReCreate(res);
        }

        public void ReCreate(Resource res)
        {
            Dispose();
            rtv = new RenderTargetView(HordeEngine.Instance.Device, res);
        }

        public void Dispose()
        {
            if (rtv != null)
            {
                rtv.Dispose();
            }
        }
    }
}
