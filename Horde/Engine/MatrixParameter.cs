using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;

namespace Horde.Engine
{
    class MatrixParameter : IConstantBufferParameter {

        private Matrix val;

        public Matrix Value {
            get {
                return val;
            }
            set {
                val = value;
                UpdateBuffer();
            }
        }

        private static int SIZE = sizeof(float) * 4 * 4;

        private byte[] buffer = new byte[SIZE];

        public MatrixParameter() {
        }

        public byte[] GetBytes() {
            return buffer;
        }

        public int GetSize() {
            return SIZE;
        }

        public void SetValue(object obj) {
            Value = (Matrix)obj;
        }

        private void UpdateBuffer() {
            Buffer.BlockCopy(val.ToArray(), 0, buffer, 0, SIZE);
        }

        public object GetValue() {
            return Value;
        }
    }
}
