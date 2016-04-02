using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using CBuffer = SharpDX.D3DCompiler.ConstantBuffer;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Horde.Engine.Shader {
    public abstract class ShaderBase : IDisposable {
        protected ShaderSignature inputSignature;
        protected List<ConstantBuffer> constantBuffers = new List<ConstantBuffer>();

        public ShaderSignature InputSignature {
            get { return inputSignature; }
        }

        public void Dispose() {
            if(inputSignature!=null) {
                inputSignature.Dispose();
            }
            if(constantBuffers!=null) {
                foreach(ConstantBuffer cb in constantBuffers) {
                    cb.Dispose();
                }
            }
        }

        public abstract void Apply(DeviceContext context);

        public void SetParameterMatrix(string name,Matrix m) {
            foreach(ConstantBuffer cb in constantBuffers) {
                cb.SetParameterMatrix(name, m);
            }
        }

        protected void ReflectBytecode(ShaderBytecode bytecode) {
            using (var reflection = new ShaderReflection(bytecode)) {
                for (int cBufferIndex = 0; cBufferIndex < reflection.Description.ConstantBuffers; cBufferIndex++) {
                    CBuffer cb = reflection.GetConstantBuffer(cBufferIndex);
                    Buffer buf = new Buffer(Renderer.Instance.Device, cb.Description.Size, ResourceUsage.Dynamic, BindFlags.ConstantBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, sizeof(float));
                    ConstantBuffer constantBuffer = new ConstantBuffer(buf);
                    for (int i = 0; i < cb.Description.VariableCount; i++) {
                        var refVar = cb.GetVariable(i);
                        var type = refVar.GetVariableType();
                        switch (type.Description.Type) {
                            case ShaderVariableType.Float:
                                if (type.Description.RowCount == 4 && type.Description.ColumnCount == 4) {
                                    var matParam = new MatrixParameter();
                                    if (matParam.GetSize() != refVar.Description.Size) {
                                        throw HordeException.Create("Error ConstantBufferParamtersize");
                                    }
                                    constantBuffer.AddParameter(refVar.Description.Name, refVar.Description.StartOffset, matParam);
                                }
                                break;
                        }
                    }
                    constantBuffers.Add(constantBuffer);
                }
            }
        }
    }
}
