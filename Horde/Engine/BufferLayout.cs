using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;

namespace Horde.Engine {
    class BufferLayout {
        public String Name {
            get;
        }

        Dictionary<string,ShaderVariableDescription> variables = new Dictionary<string, ShaderVariableDescription>();

        public BufferLayout(string name) {
            this.Name = name;
        }

        public void AddVariable(ShaderVariableDescription desc) {
            if(!variables.ContainsKey(desc.Name)) {
                variables.Add(desc.Name, desc);
            } else {
                throw HordeException.Create("Constant already exists: " + desc.Name);
            }
        }
    }
}
