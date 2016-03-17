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
        private RenderTarget rtv;
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

        private int formWidth, formHeight;

        public HordeEngine()
        {
            instance = this;
        }

        public void Dispose()
        {
            if (rtv != null)
            {
                rtv.Dispose();
            }
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
            formWidth = form.Width;
            formHeight = form.Height;
            form.ResizeBegin += (o, e) =>
            {
                formHeight = ((Form)o).Height;
                formWidth = ((Form)o).Width;
            };
            form.ResizeEnd += HandleResize;

            form.KeyDown += HandleKeyDown;

            var swapChainDescriptor = new SwapChainDescription()
            {
                BufferCount = 1,
                Usage = Usage.RenderTargetOutput,
                Flags = SwapChainFlags.AllowModeSwitch,
                IsWindowed = true,
                ModeDescription = new ModeDescription(form.Width, form.Height, new Rational(0, 1), Format.R8G8B8A8_UNorm),
                OutputHandle = form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard
            };
            var hResult = Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, swapChainDescriptor, out device, out swapChain);
            if (hResult != ResultCode.Success)
            {
                throw HordeException.Create("Error initialize Device and SwapChain");
            }
            devContext = device.ImmediateContext;

            using (var resource = Resource.FromSwapChain<Texture2D>(swapChain, 0))
            {
                rtv = new RenderTarget(resource);
            }
        }

        public void ClearRenderTarget(Color4 col)
        {
            devContext.ClearRenderTargetView(rtv.View, col);
        }

        public void Present()
        {
            swapChain.Present(1, 0);
        }

        private void HandleResize(object sender, System.EventArgs e)
        {
            Form f = (Form)sender;
            if (f.Width != formWidth || f.Height != formHeight)
            {
                formWidth = f.Width;
                formHeight = f.Height;
                rtv.Dispose();
                swapChain.ResizeBuffers(1, 0, 0, Format.R8G8B8A8_UNorm, SwapChainFlags.AllowModeSwitch);
                using (var resource = Resource.FromSwapChain<Texture2D>(swapChain, 0))
                {
                    rtv = new RenderTarget(resource);
                }

                devContext.OutputMerger.SetTargets(rtv.View);
            }
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Alt && e.KeyCode==Keys.Enter)
            {
                swapChain.IsFullScreen = !swapChain.IsFullScreen;
            }
        }

    }
}
