using System;
using System.Collections.Generic;
using SlimDX;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace Horde.Engine {
    public class ConstantBuffer : IDisposable {

        public Buffer Buffer {
            get;
        }

        private byte[] cpuBuffer;
        private int bufferSize;
        private Dictionary<string,IConstantBufferParameter> members = new Dictionary<string, IConstantBufferParameter>();

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

        public void AddParameter(IConstantBufferParameter param) {
            members.Add(param.GetName(),param);
        }

        public void SetParameterMatrix(string name, Matrix m) {
            SetParameterValue(name, m);
        }

        private void SetParameterValue(string name, object obj) {
            if (members.ContainsKey(name)) {
                members[name].SetValue(obj);
            }
        }

        public void UpdateBuffer(DeviceContext context) {
            UpdateCpuBuffer();
            dataStream.Seek(0,System.IO.SeekOrigin.Begin);
            dataStream.Write(cpuBuffer, 0, bufferSize);
            context.UpdateSubresource(new DataBox(0, 0,dataStream), Buffer, 0);
        }

        private void UpdateCpuBuffer() {
            foreach(string key in members.Keys) {
                IConstantBufferParameter param = members[key];
                Array.Copy(param.GetBytes(), 0, cpuBuffer, param.GetOffset(), param.GetSize());
            }
        }
    }
}
