using System;
using System.Windows.Forms;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;
using ResultCode = SlimDX.Direct3D11.ResultCode;

namespace Horde.Engine
{
    public class HordeEngine : IDisposable
    {
        #region Members

        private static HordeEngine instance;

        public static HordeEngine Instance
        {
            get {
                return instance;
            }
        }

        private Renderer renderer;

        #endregion Members

        public HordeEngine()
        {
            instance = this;
        }

        public void Dispose()
        {
            renderer.Dispose();
        }

        public void Init(Form form)
        {
            renderer = new Renderer();
        }
    }
}
