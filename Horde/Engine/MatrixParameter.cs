using System;
using System.Runtime.InteropServices;
using SlimDX;

namespace Horde.Engine {
/*    class MatrixParameter : ConstantBufferParameter<Matrix> {

        private byte[] bytes;

        public MatrixParameter(string name, int offset) {
            base();

       }

        protected override int GetSize() {
            return sizeof(float) * 4 * 4;
        }

        public byte[] GetBytes() {
            int len = Marshal.SizeOf(Value);
            byte[] arr = new byte[len];
            IntPtr ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(Value, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }*/
}
