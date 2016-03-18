using System;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using DXSwapChain = SlimDX.DXGI.SwapChain;
using Resource = SlimDX.Direct3D11.Resource;

namespace Horde.Engine
{
    public class SwapChain
    {
        private DXSwapChain swapChain;

        private RenderTarget renderTarget;
        public RenderTarget RenderTarget
        {
            get { return renderTarget; }
        }

        public Viewport Viewport
        {
            get { return renderTarget.Viewport; }
            set { renderTarget.Viewport = value; }
        }

        private int formWidth, formHeight;

        public SwapChain(Form form)
        {
            formWidth = form.Width;
            formHeight = form.Height;

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

            swapChain = new DXSwapChain(HordeEngine.Instance.Device.Factory, HordeEngine.Instance.Device, swapChainDescriptor);

            using (var resource = Resource.FromSwapChain<Texture2D>(swapChain, 0))
            {
                renderTarget = new RenderTarget(resource);
            }

            using (var factory = swapChain.GetParent<Factory>())
            {
                factory.SetWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAltEnter);
            }

            form.ResizeBegin += (o, e) =>
            {
                formHeight = ((Form)o).Height;
                formWidth = ((Form)o).Width;
            };
            form.ResizeEnd += HandleResize;
            form.KeyDown += HandleKeyDown;
        }

        public void Dispose()
        {
            if(renderTarget!=null)
            {
                renderTarget.Dispose();
            }
            if (swapChain != null)
            {
                swapChain.Dispose();
            }
        }

        public void Present()
        {
            swapChain.Present(0, PresentFlags.None);
        }

        public void Activate()
        {
            renderTarget.Activate();
        }

        private void HandleResize(object sender, System.EventArgs e)
        {
            Form f = (Form)sender;
            if (f.Width != formWidth || f.Height != formHeight)
            {
                formWidth = f.Width;
                formHeight = f.Height;
                renderTarget.Dispose();
                swapChain.ResizeBuffers(1, 0, 0, Format.R8G8B8A8_UNorm, SwapChainFlags.AllowModeSwitch);
                using (var resource = Resource.FromSwapChain<Texture2D>(swapChain, 0))
                {
                    renderTarget = new RenderTarget(resource);
                }

                HordeEngine.Instance.DeviceContext.OutputMerger.SetTargets(renderTarget.View);
            }
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.Enter)
            {
                swapChain.IsFullScreen = !swapChain.IsFullScreen;
            }
        }
    }
}
