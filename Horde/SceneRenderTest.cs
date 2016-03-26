using System;
using Horde.Engine;
using Horde.Engine.Task;
using SlimDX.Direct3D11;
using SlimDX;
using SlimDX.DXGI;
using Buffer = SlimDX.Direct3D11.Buffer;
using VertexShader = Horde.Engine.Shader.VertexShader;
using PixelShader = Horde.Engine.Shader.PixelShader;

namespace Horde {
    class SceneRenderTest : SceneRenderTask, IDisposable {

        VertexShader vertexShader;
        PixelShader pixelShader;
        Buffer vertexBuffer;
        int vertexSize;
        InputLayout layout;
        RenderTarget backBuffer;
        public Camera cam;

        Matrix world;

        public SceneRenderTest(RenderTarget backBuffer) {
            this.backBuffer = backBuffer;              
        }

        public void Init() {
            vertexShader = new VertexShader("simple.fx", "VShader");
            pixelShader = new PixelShader("simple.fx", "PShader");

            vertexSize = sizeof(float) * 3 * 4;
            var vertices = new DataStream(vertexSize, true, true);
            vertices.Write(new Vector3(-1.0f, -1.0f, 0.0f));
            vertices.Write(new Vector3(-1.0f, 1.0f, 0.0f));
            vertices.Write(new Vector3(1.0f, -1.0f, 0.0f));
            vertices.Write(new Vector3(1.0f, 1.0f, 0.0f));
            vertices.Position = 0;

            var elements = new[] { new InputElement("POSITION", 0, Format.R32G32B32_Float, 0) };
            layout = new InputLayout(Renderer.Instance.Device, vertexShader.InputSignature, elements);
            vertexBuffer = new Buffer(Renderer.Instance.Device, vertices, vertexSize, ResourceUsage.Default, BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            vertices.Close();

            world = Matrix.Translation(0.0f, 0.0f, 0.0f);
            world = Matrix.Identity;
        }

        public override void Execute(RenderPipeline pipeline) {
            vertexShader.SetParameterMatrix("projMatrix", Matrix.Transpose(cam.ProjectionMatrix));
            vertexShader.SetParameterMatrix("viewMatrix", Matrix.Transpose(cam.ViewMatrix));
            world = world *Matrix.RotationZ(0.001f);
            vertexShader.SetParameterMatrix("worldMatrix", Matrix.Transpose(world));

            pipeline.DeviceContext.InputAssembler.InputLayout = layout;
            pipeline.DeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleStrip;
            pipeline.DeviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, vertexSize / 4, 0));

            RasterizerStateDescription desc = new RasterizerStateDescription() {
                CullMode = CullMode.None,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = false,
                IsFrontCounterclockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };

            var rs = RasterizerState.FromDescription(Renderer.Instance.Device, desc);

            pipeline.DeviceContext.Rasterizer.State = rs;
            vertexShader.Apply(pipeline.DeviceContext);
            pixelShader.Apply(pipeline.DeviceContext);
            pipeline.ClearRenderTarget(backBuffer,new Color4(0,0,1.0f));
            pipeline.OutputMergerStage.SetRenderTarget(0, backBuffer);
            pipeline.ApplyRenderTargets();
            backBuffer.Activate(pipeline);
            pipeline.Draw(4,0);
        }

        public void Dispose() {
            layout.Dispose();
            vertexBuffer.Dispose();
            pixelShader.Dispose();
            vertexShader.Dispose();
        }
    }
}
