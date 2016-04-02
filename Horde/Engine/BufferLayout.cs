using System;
using System.Collections.Generic;
using SharpDX.D3DCompiler;

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
