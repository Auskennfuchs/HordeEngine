using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Horde.Engine {

    public interface IConstantBufferParameter {
        int GetSize();
        int GetOffset();
        byte[] GetBytes();
        string GetName();
        void SetValue(object obj);
    }

    public class ConstantBufferParameter<T> : IDisposable, IConstantBufferParameter{
        public int Size {
            get { return len; }
        }
        public int GetSize() {
            return Size;
        }

        public T Value {
            get; set;
        }
        public void SetValue(object obj) {
            Value = (T)obj;
        }

        public string Name {
            get;
        }
        public string GetName() {
            return Name;
        }

        public int Offset {
            get;
        }
        public int GetOffset() {
            return Offset;
        }

        private byte[] bytes;
        private int len;
        IntPtr ptr;

        public ConstantBufferParameter(string name, int offset) {
            Name = name;
            Offset = offset;
            len = Marshal.SizeOf(Value);
            bytes = new byte[len];
            ptr = Marshal.AllocHGlobal(len);
        }

        public byte[] GetBytes() {
            Marshal.StructureToPtr(Value, ptr, true);
            Marshal.Copy(ptr, bytes, 0, len);
            return bytes;
        }

        public void Dispose() {
            Marshal.FreeHGlobal(ptr);
        }
    }
}
