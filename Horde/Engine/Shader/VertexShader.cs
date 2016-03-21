using System;
using System.Collections.Generic;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;

using VShader = SlimDX.Direct3D11.VertexShader;

namespace Horde.Engine.Shader {
    public class VertexShader : ShaderBase {
        private VShader vertexShader;        
        public VShader VertexShaderPtr {
            get { return vertexShader; }
        }

        private List<BufferLayout> bufferLayouts = new List<BufferLayout>();

        public VertexShader(string file,string entryfunction) {
            try {
                using (var bytecode = ShaderBytecode.CompileFromFile(file, entryfunction, "vs_5_0", ShaderFlags.None, EffectFlags.None)) {
                    inputSignature = ShaderSignature.GetInputSignature(bytecode);
                    vertexShader = new VShader(Renderer.Instance.Device, bytecode);
                    ShaderReflection reflection = new ShaderReflection(bytecode);
                    for(int cBufferIndex=0; cBufferIndex<reflection.Description.ConstantBuffers;cBufferIndex++) {
                        ConstantBuffer cb = reflection.GetConstantBuffer(cBufferIndex);
//                        cb.Description.Variables
                    }
                }
            }
            catch (Exception exc) {
                throw HordeException.Create("Error loading VertexShader", exc);
            }
        }

        public new void Dispose() {
            base.Dispose();
            if(vertexShader!=null) {
                vertexShader.Dispose();
            }
        }

        public override void Apply(DeviceContext context) {
            context.VertexShader.Set(vertexShader);
        }
    }
}
