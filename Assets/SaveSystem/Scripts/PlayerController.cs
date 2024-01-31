using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] GameObject _ObjectPrefab;
        [SerializeField] int _MaxObjectCount = 100;
        [SerializeField] float _ScaleSpeed = 100;

        List<ObjectController> _objectsToSave => ObjectSaver.ObjectsToSave;
        ObjectController _heldObject = null;

        void Start()
        {
            ObjectController[] objects = FindObjectsOfType<ObjectController>(true);

            foreach (ObjectController oc in objects)
            {
                _objectsToSave.Add(oc);
            }
        }

        void Update()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            ObjectController hoverOverController = null;
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 100, 1 << 6);
            if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out ObjectController oc))
            {
                hoverOverController = oc;
            }

            if (Input.GetMouseButtonDown(0) && _heldObject == null)
            {
                _heldObject = hoverOverController;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _heldObject = null;
            }
            else if ((Input.GetMouseButtonDown(1) || (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftControl)))
                && _objectsToSave.Count < _MaxObjectCount)
            {
                if (_ObjectPrefab.TryGetComponent(out ObjectController _))
                {
                    _objectsToSave.Add(Instantiate(_ObjectPrefab, mousePos, Quaternion.identity)
                        .GetComponent<ObjectController>());
                }
            }

            if (_heldObject != null)
            {
                _heldObject.MoveTo(mousePos);
                SetScaleByMouseWheel(_heldObject);
                if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.E)) _heldObject.SetNextColor();
                else if (Input.GetKeyDown(KeyCode.Q)) _heldObject.SetPreviousColor();
            }
            else if (hoverOverController != null)
            {
                SetScaleByMouseWheel(hoverOverController);
                if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.E)) hoverOverController.SetNextColor();
                else if (Input.GetKeyDown(KeyCode.Q)) hoverOverController.SetPreviousColor();
            }
        }

        public void ResetAllObjects()
        {
            foreach(ObjectController oc in _objectsToSave)
            {
                oc.SetColorByIndex(0);
                oc.transform.localScale = Vector3.one;
            }
        }
        public void DeleteAllObjects()
        {
            foreach (ObjectController oc in _objectsToSave) Destroy(oc.gameObject);
            ObjectSaver.ResetObjectsToSaveList();
        }
        void SetScaleByMouseWheel(ObjectController obj)
        {
            if (obj == null) return;

            obj.AddToSize(Input.GetAxisRaw("Mouse ScrollWheel") * Time.deltaTime * _ScaleSpeed);
        }
    }
}