using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Horde.Engine {

    public interface IConstantBufferParameter {
        int GetSize();
        byte[] GetBytes();
        void SetValue(object obj);
        object GetValue();
    }

    public class ConstantBufferParameter {
        public int Size {
            get {
                return param.GetSize();
            }
        }

        public object Value {
            get {
                return param.GetValue();
            }
            set {
                param.SetValue(value);
            }
        }

        private IConstantBufferParameter param;
        public IConstantBufferParameter Param {
            get { return param; }
        }

        public string Name {
            get;
        }

        public int Offset {
            get;
        }

        public ConstantBufferParameter(string name, int offset, IConstantBufferParameter param) {
            Name = name;
            Offset = offset;
            this.param = param;
        }
    }
}
