using System;
using System.Windows.Forms;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;
using ResultCode = SharpDX.Direct3D11.ResultCode;

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
