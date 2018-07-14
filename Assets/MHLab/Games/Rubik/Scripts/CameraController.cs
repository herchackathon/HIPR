using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace MHLab.Games.Rubik
{
    public class CameraController : MonoBehaviour
    {
        public static bool CanMove = true;

        public RubikCube Target;
        
        private bool _mouseDown;
        private Vector3 _lastMousePos;
        private Vector2 _currentRot, _targetRot;
        public float _zoom = 1f;

        // Use this for initialization
        protected void Start()
        {
            _targetRot.Set(0, Target.transform.eulerAngles.y - 180);
        }
        
        // Update is called once per frame
        protected void LateUpdate()
        {
            if (!CanMove) return;
            if (!Target) return;
            ProcessMouseInputs();
            RotateCamera();
        }

        private void ProcessMouseInputs()
        {

            if (Input.GetMouseButtonDown(0))
            {
                _lastMousePos = Input.mousePosition;
                _mouseDown = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _mouseDown = false;
            }

            if (_mouseDown)
            {
                Vector2 delta = new Vector2(_lastMousePos.x - Input.mousePosition.x, _lastMousePos.y - Input.mousePosition.y) * .3f;
                delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 10f;
                float yRot = (_targetRot.y - delta.x);
                if (yRot < -180f)
                    yRot += 359f;

                if (yRot > 180f)
                    yRot -= 359f;
                _targetRot.Set(Mathf.Clamp(_targetRot.x - delta.y, -80f, 80f), yRot);
                _lastMousePos = Input.mousePosition;
            }

            _zoom = Mathf.Clamp(_zoom - Input.GetAxis("Mouse ScrollWheel"), 1f, 30f);
        }

        Vector3 _currentOffset;

        private void RotateCamera()
        {
            Vector3 offset = Quaternion.Euler(_targetRot.x, _targetRot.y, 0) * Vector3.forward;

            _currentOffset = Vector3.Slerp(_currentOffset, offset, .1f);
            Vector3 targetOffset = Target.transform.position;
            //float zoom = _zoom;
            Vector3 targetPosition = targetOffset + _currentOffset * _zoom;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 80f * Time.deltaTime);
            
            Vector3 direction = transform.position - targetOffset;
            direction.Normalize();

            this.transform.rotation = Quaternion.LookRotation(-direction);
        }
    }
}
