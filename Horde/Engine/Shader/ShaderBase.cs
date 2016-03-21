using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;

namespace Horde.Engine.Shader {
    public abstract class ShaderBase : IDisposable {
        protected ShaderSignature inputSignature;

        public ShaderSignature InputSignature {
            get { return inputSignature; }
        }

        public void Dispose() {
            if(inputSignature!=null) {
                inputSignature.Dispose();
            }
        }

        public abstract void Apply(DeviceContext context);
    }
}
