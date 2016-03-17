using SlimDX.Direct3D11;

namespace Horde.Engine
{
    class RenderTarget
    {
        private RenderTargetView rtv;

        public RenderTarget(Resource res)
        {
            ReCreate(res);
        }

        public void ReCreate(Resource res)
        {
            if(rtv!=null)
            {
                rtv.Dispose();
            }
            rtv = new RenderTargetView(HordeEngine.Instance.Device, res);
        }
    }
}
