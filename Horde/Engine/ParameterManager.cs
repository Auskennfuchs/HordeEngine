using System;
using System.Collections.Generic;
using SharpDX;

namespace Horde.Engine {
    class ParameterManager {
        Dictionary<string, IConstantBufferParameter> parameters = new Dictionary<string, IConstantBufferParameter>();

        public void SetParameter(string name, Matrix mat) {
            if(!SetParam(name, mat, ConstantBufferParameterType.MATRIX)) {
                parameters.Add(name, new MatrixParameter(mat));
            }
        }

        public Matrix GetMatrixParameter(string name) {
            return (Matrix)GetParam(name, ConstantBufferParameterType.MATRIX);
        }

        public void SetParameter(string name, Vector3 vec) {
            if(!SetParam(name, vec, ConstantBufferParameterType.VECTOR3)) {
                throw new NotImplementedException();
            }
        }
        public Vector3 GetVector3Parameter(string name) {
            return (Vector3)GetParam(name, ConstantBufferParameterType.VECTOR3);
        }

        public void SetParameter(string name, Vector4 vec) {
            if(!SetParam(name, vec, ConstantBufferParameterType.VECTOR4)) {
                throw new NotImplementedException();
            }
        }
        public Vector4 GetVector4Parameter(string name) {
            return (Vector4)GetParam(name, ConstantBufferParameterType.VECTOR4);
        }

        private bool SetParam(string name,object obj, ConstantBufferParameterType type) {
            if (parameters.ContainsKey(name)) {
                var param = parameters[name];
                if (param.GetType() != type) {
                    throw HordeException.Create("Wrong Parametertype expected " + type + " but was " + param.GetType());
                }
                param.SetValue(obj);
                return true;
            }
            return false;
        }

        private object GetParam(string name,ConstantBufferParameterType type) {
            if (parameters.ContainsKey(name)) {
                var param = parameters[name];
                if (param.GetType() != type) {
                    throw HordeException.Create("Wrong Parametertype expected " + type + " but was " + param.GetType());
                }
                return param.GetValue();
            }
            return null;
        }
    }
}
