using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Horde.Engine;
using Horde.Engine.Task;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using SlimDX;
using SlimDX.DXGI;
using Buffer = SlimDX.Direct3D11.Buffer;
using System.Windows.Forms;
using System.IO;

namespace Horde {
    class SceneRenderTest : SceneRenderTask, IDisposable {

        ShaderSignature inputSignature;
        VertexShader vertexShader;
        PixelShader pixelShader;
        Buffer vertexBuffer;
        int vertexSize;
        InputLayout layout;
        RenderTarget backBuffer;

        public SceneRenderTest(RenderTarget backBuffer) {
            this.backBuffer = backBuffer;              
        }

        public void Init() {
            try {
                using (var bytecode = ShaderBytecode.CompileFromFile("simple.fx", "VShader", "vs_4_0", ShaderFlags.None, EffectFlags.None)) {
                    inputSignature = ShaderSignature.GetInputSignature(bytecode);
                    vertexShader = new VertexShader(Renderer.Instance.Device, bytecode);
                }
            }
            catch (Exception exc) {
                throw HordeException.Create("Error loading VertexShader", exc);
            }

            try {
                using (var bytecode = ShaderBytecode.CompileFromFile("simple.fx", "PShader", "ps_4_0", ShaderFlags.None, EffectFlags.None)) {
                    pixelShader = new PixelShader(Renderer.Instance.Device, bytecode);
                }
            }
            catch (Exception exc) {
                throw HordeException.Create("Error loading PixelShader", exc);
            }

            vertexSize = sizeof(float) * 3 * 3;
            var vertices = new DataStream(vertexSize, true, true);
            vertices.Write(new Vector3(0.0f, 0.5f, 0.5f));
            vertices.Write(new Vector3(0.5f, -0.5f, 0.5f));
            vertices.Write(new Vector3(-0.5f, -0.5f, 0.5f));
            vertices.Position = 0;

            var elements = new[] { new InputElement("POSITION", 0, Format.R32G32B32_Float, 0) };
            layout = new InputLayout(Renderer.Instance.Device, inputSignature, elements);
            vertexBuffer = new Buffer(Renderer.Instance.Device, vertices, vertexSize, ResourceUsage.Default, BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            vertices.Close();
        }

        public override void Execute(RenderPipeline pipeline) {
            pipeline.DeviceContext.InputAssembler.InputLayout = layout;
            pipeline.DeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            pipeline.DeviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, vertexSize / 3, 0));
            pipeline.DeviceContext.VertexShader.Set(vertexShader);
            pipeline.DeviceContext.PixelShader.Set(pixelShader);
            pipeline.ClearRenderTarget(backBuffer,new Color4(0,0,1.0f));
            backBuffer.Activate(pipeline);
            pipeline.Draw(3,0);
        }

        public void Dispose() {
            layout.Dispose();
            inputSignature.Dispose();
            vertexBuffer.Dispose();
            pixelShader.Dispose();
            vertexShader.Dispose();
        }
    }
}
