using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using SlimDX.Windows;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;
using ResultCode = SlimDX.Direct3D11.ResultCode;
using Horde.Engine;

namespace Horde
{
    public class MainWindow : RenderForm
    {

        private HordeEngine engine;

        public MainWindow(string windowName) : 
            base(windowName)
        {
            this.Width = 1024;
            this.Height = 768;

            engine = new HordeEngine();
            engine.Init(this);
        }

        public new void Close()
        {
            engine.Dispose();
            base.Close();
        }

        public void MainLoop()
        {
            engine.ClearRenderTarget(new Color4(0, 0, 1.0f));
            engine.Present();
        }
    }
}
