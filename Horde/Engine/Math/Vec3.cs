using SharpDX;

namespace Horde.Engine.Math {
    class Vec3  {
        public static Vector3 UP = new Vector3(0.0f, 1.0f, 0.0f);
        public static Vector3 DOWN = new Vector3(0.0f, -1.0f, 0.0f);
        public static Vector3 RIGHT = new Vector3(1.0f, 0.0f, 0.0f);
        public static Vector3 LEFT = new Vector3(-1.0f, 0.0f, 0.0f);
        public static Vector3 FORWARD = new Vector3(0.0f, 0.0f, 1.0f);
        public static Vector3 BACKWARD = new Vector3(0.0f, 0.0f, -1.0f);

    }
}
