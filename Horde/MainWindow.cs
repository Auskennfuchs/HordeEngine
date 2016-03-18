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
using Buffer = SlimDX.Direct3D11.Buffer;
using Horde.Engine;
using System.Windows.Forms;
using SlimDX.D3DCompiler;

namespace Horde
{
    public class MainWindow : Form
    {

        private HordeEngine engine;

        ShaderSignature inputSignature;
        VertexShader vertexShader;
        PixelShader pixelShader;
        Buffer vertexBuffer;


        public MainWindow(string windowName) : 
            base()
        {
            InitializeComponent();
            this.Text = windowName;
            this.Width = 1024;
            this.Height = 768;

            engine = new HordeEngine();
            engine.Init(this);

            try
            {
                using (var bytecode = ShaderBytecode.CompileFromFile("simple.fx", "VShader", "vs_4_0", ShaderFlags.None, EffectFlags.None))
                {
                    inputSignature = ShaderSignature.GetInputSignature(bytecode);
                    vertexShader = new VertexShader(engine.Device, bytecode);
                }
            }
            catch (Exception exc)
            {
                throw HordeException.Create("Error loading VertexShader",exc);
            }

            try
            {
                using (var bytecode = ShaderBytecode.CompileFromFile("simple.fx", "PShader", "ps_4_0", ShaderFlags.None, EffectFlags.None))
                {
                    pixelShader = new PixelShader(engine.Device, bytecode);
                }
            }
            catch (Exception exc)
            {
                throw HordeException.Create("Error loading PixelShader", exc);
            }

            var vertexSize = sizeof(float) * 3 * 3;
            var vertices = new DataStream(vertexSize, true, true);
            vertices.Write(new Vector3(0.0f, 0.5f, 0.5f));
            vertices.Write(new Vector3(0.5f, -0.5f, 0.5f));
            vertices.Write(new Vector3(-0.5f, -0.5f, 0.5f));
            vertices.Position = 0;

            var elements = new[] { new InputElement("POSITION", 0, Format.R32G32B32_Float, 0) };
            var layout = new InputLayout(engine.Device, inputSignature, elements);
            vertexBuffer = new Buffer(engine.Device, vertices, vertexSize, ResourceUsage.Default, BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            vertices.Close();

            engine.DeviceContext.InputAssembler.InputLayout = layout;
            engine.DeviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, vertexSize, 0));
            engine.DeviceContext.VertexShader.Set(vertexShader);
            engine.DeviceContext.PixelShader.Set(pixelShader);

        }

        public new void Close()
        {
            vertexBuffer.Dispose();
            pixelShader.Dispose();
            vertexShader.Dispose();
            engine.Dispose();
            base.Close();
        }

        public void MainLoop()
        {
            engine.ClearRenderTarget(new Color4(0, 0, 1.0f));

            engine.DeviceContext.Draw(3, 0);

            engine.Present();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.SuspendLayout();
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.ResumeLayout(false);

        }
    }
}
