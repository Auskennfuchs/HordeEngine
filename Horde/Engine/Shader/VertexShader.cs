using System;
using System.Collections.Generic;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

using VShader = SharpDX.Direct3D11.VertexShader;

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
                    ReflectBytecode(bytecode);
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
            for(int i=0;i<constantBuffers.Count;i++) {
                constantBuffers[i].UpdateBuffer(context);
                context.VertexShader.SetConstantBuffer(i,constantBuffers[i].Buffer);
            }
        }
    }
}
