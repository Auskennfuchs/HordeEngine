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
    public class HordeEngine
    {
        private Device device;
        public Device Device
        {
            get { return device; }
        }
        private SwapChain swapChain;
        private DeviceContext devContext;
        public DeviceContext DeviceContext
        {
            get { return devContext; }
        }

        private static HordeEngine instance;

        public static HordeEngine Instance
        {
            get {
                return instance;
            }
        }

        public HordeEngine()
        {
            instance = this;
        }

        public void Dispose()
        {
            if (swapChain != null)
            {
                swapChain.Dispose();
            }
            if (device != null)
            {
                device.Dispose();
            }
        }

        public void Init(Form form)
        {
            device = new Device(DriverType.Hardware, DeviceCreationFlags.None);
            devContext = device.ImmediateContext;

            swapChain = new SwapChain(form);
        }

        public void ClearRenderTarget(Color4 col)
        {
            devContext.ClearRenderTargetView(swapChain.RenderTarget.View, col);
        }

        public void Present()
        {
            swapChain.Present();
        }

    }
}
