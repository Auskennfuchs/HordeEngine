using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Horde.Engine {
    public class ConstantBuffer : IDisposable {

        public Buffer Buffer {
            get;
            private set;
        }

        private byte[] cpuBuffer;
        private int bufferSize;
        private Dictionary<string,ConstantBufferParameter> members = new Dictionary<string, ConstantBufferParameter>();

        private DataStream dataStream;

        public ConstantBuffer(Buffer buf) {
            Buffer = buf;
            if (buf != null) {
                bufferSize = buf.Description.SizeInBytes;
                cpuBuffer = new byte[bufferSize];
                dataStream = new DataStream(bufferSize,true,true);
            }
        }

        public void Dispose() {
            if(Buffer!=null) {
                Buffer.Dispose();
            }
            if(dataStream!=null) {
                dataStream.Dispose();
            }
        }

        public void AddParameter(string name, int offset,IConstantBufferParameter param) {
            ConstantBufferParameter c = new ConstantBufferParameter(name,offset,param);
            members.Add(name,c);
        }

        public void SetParameterMatrix(string name, Matrix m) {
            SetParameterValue(name, m);
        }

        private void SetParameterValue(string name, object obj) {
            if (members.ContainsKey(name)) {
                members[name].Value=obj;
            }
        }

        public void UpdateBuffer(DeviceContext context) {
            UpdateCpuBuffer();
            DataStream ds;
            context.MapSubresource(Buffer, 0, MapMode.WriteDiscard, MapFlags.None, out ds);
            ds.Write(cpuBuffer, 0, bufferSize);
            context.UnmapSubresource(Buffer, 0);
        }

        private void UpdateCpuBuffer() {
            foreach(string key in members.Keys) {
                ConstantBufferParameter param = members[key];
                System.Buffer.BlockCopy(param.Param.GetBytes(), 0, cpuBuffer, param.Offset, param.Size);
            }
        }
    }
}
