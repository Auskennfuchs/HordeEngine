using System;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using PShader = SharpDX.Direct3D11.PixelShader;

namespace Horde.Engine.Shader {
    public class PixelShader : ShaderBase {
        private PShader pixelShader;
        public PShader PixelShaderPtr {
            get { return pixelShader; }
        }

        public PixelShader(string file, string entryfunction) {
            try {
                using (var bytecode = ShaderBytecode.CompileFromFile(file, entryfunction, "ps_5_0", ShaderFlags.None, EffectFlags.None)) {
                    inputSignature = ShaderSignature.GetInputSignature(bytecode);
                    pixelShader = new PShader(Renderer.Instance.Device, bytecode);
                }
            }
            catch (Exception exc) {
                throw HordeException.Create("Error loading VertexShader", exc);
            }

        }

        public new void Dispose() {
            base.Dispose();
            if(pixelShader!=null) {
                pixelShader.Dispose();
            }
        }

        public override void Apply(DeviceContext context) {
            context.PixelShader.Set(pixelShader);
        }
    }
}
