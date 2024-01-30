using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace SaveSystem
{
    public class ObjectSaver : MonoBehaviour
    {
        public static List<ObjectController> ObjectsToSave = new List<ObjectController>();

        [SerializeField] GameObject _Prefab;

        const string OBJECT_HASH = "/object";
        const string OBJECT_COUNT_HASH = "/object-count";

        public void SaveObjects()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + OBJECT_HASH;
            string countPath = Application.persistentDataPath + OBJECT_COUNT_HASH;

            FileStream countStream = new FileStream(countPath, FileMode.Create);

            formatter.Serialize(countStream, ObjectsToSave.Count);
            countStream.Close();

            for (int i = 0; i < ObjectsToSave.Count; i++)
            {
                FileStream stream = new FileStream(path + i, FileMode.Create);
                ObjectData data = new ObjectData(ObjectsToSave[i]);

                formatter.Serialize(stream, data);
                stream.Close();
            }
        }
        public void LoadObjects()
        {
            foreach (ObjectController obj in ObjectsToSave)
            {
                Destroy(obj.gameObject);
            }

            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + OBJECT_HASH;
            string countPath = Application.persistentDataPath + OBJECT_COUNT_HASH;
            int count = 0;

            if (File.Exists(countPath))
            {
                FileStream countStream = new FileStream(countPath, FileMode.Open);
                count = (int)formatter.Deserialize(countStream);
                countStream.Close();
            }
            else
            {
                Debug.LogError("No file found at " + countPath);
                return;
            }

            for (int i = 0; i < count; i++)
            {
                if (File.Exists(path + i))
                {
                    FileStream stream = new FileStream(path + i, FileMode.Open);
                    ObjectData data = (ObjectData)formatter.Deserialize(stream);

                    Vector2 pos = new Vector2(data.Position[0], data.Position[1]);
                    ObjectController oc = Instantiate(_Prefab, pos, Quaternion.identity).GetComponent<ObjectController>();
                    oc.transform.localScale = Vector3.one * data.Scale;
                    oc.SetColorByIndex(data.ColorIndex);

                    stream.Close();
                }
                else
                {
                    Debug.LogError("No file found at " + path + i);
                }
            }
        }
    }
}