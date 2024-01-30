using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    [System.Serializable]
    public class ObjectData
    {
        public float[] Position;
        public float Scale;
        public int ColorIndex;

        public ObjectData(ObjectController obj)
        {
            ColorIndex = obj.ColorIndex;
            Position = new float[] { obj.transform.position.x, obj.transform.position.y };
            Scale = obj.transform.localScale.x;
        }
    }
}
