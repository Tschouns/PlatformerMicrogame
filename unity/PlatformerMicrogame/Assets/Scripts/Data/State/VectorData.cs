using UnityEngine;

namespace Assets.Scripts.Data.State
{
    public class VectorData
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public static VectorData FromUnityVector(Vector2 vector)
        {
            return new VectorData { X = vector.x, Y = vector.y, Z = 0 };
        }

        public static VectorData FromUnityVector(Vector3 vector)
        {
            return new VectorData { X = vector.x, Y = vector.y, Z = vector.z };
        }

        public Vector2 ToUnityVector2()
        {
            return new Vector2(X, Y);
        }

        public Vector3 ToUnityVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }
}
