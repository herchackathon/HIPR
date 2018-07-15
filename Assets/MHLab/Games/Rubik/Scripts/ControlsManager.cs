using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MHLab.Games.Rubik
{
    public class ControlsManager : MonoBehaviour
    {
        public static bool CanMove = false;
        public RubikCube Cube;
        public float MovesActivationThreshold = 1f;

        private RubikSubcube _currentSubcube;
        private Vector3 _initialPoint;

        protected void LateUpdate()
        {
            if (!CanMove) return;

            DetectSelectedCube();

            if (_currentSubcube != null)
            {
                var position = Input.mousePosition;
                var distance = Vector3.Distance(_initialPoint, position);
                
                if (distance >= MovesActivationThreshold)
                {
                    var directionX = position.x - _initialPoint.x;
                    var directionY = position.y - _initialPoint.y;

                    var faces = Cube.GetSubcubeFaces(_currentSubcube);

                    if (faces.Count > 0)
                    {
                        RubikFaceType selectedFace = faces[0];
                        bool clockwise;

                        if (Mathf.Abs(directionX) > Mathf.Abs(directionY))
                        {
                            clockwise = (directionX >= 0);
                            /*switch (faces.Count)
                            {
                                case 2:

                                    break;
                                case 3:

                                    break;
                            }*/
                            if (faces.Contains(RubikFaceType.Top))
                                selectedFace = RubikFaceType.Top;
                            if(faces.Contains(RubikFaceType.Bottom))
                                selectedFace = RubikFaceType.Bottom;
                        }
                        else
                        {
                            clockwise = (directionY >= 0);
                            if (faces.Contains(RubikFaceType.Front))
                                selectedFace = RubikFaceType.Front;
                            if (faces.Contains(RubikFaceType.Right))
                                selectedFace = RubikFaceType.Right;
                            if (faces.Contains(RubikFaceType.Left))
                                selectedFace = RubikFaceType.Left;
                            if (faces.Contains(RubikFaceType.Back))
                                selectedFace = RubikFaceType.Back;
                        }

                        // Hardcoded moves orientation. This is something quick to fix the moves-set
                        if (selectedFace == RubikFaceType.Top && (faces.Contains(RubikFaceType.Right) || faces.Contains(RubikFaceType.Front) || faces.Contains(RubikFaceType.Left)))
                            clockwise = !clockwise;
                        if(selectedFace == RubikFaceType.Left && (faces.Contains(RubikFaceType.Front)))
                            clockwise = !clockwise;
                        if (selectedFace == RubikFaceType.Back && (faces.Contains(RubikFaceType.Left)))
                            clockwise = !clockwise;
                        if (selectedFace == RubikFaceType.Right && (faces.Contains(RubikFaceType.Bottom)))
                            clockwise = !clockwise;
                        if (selectedFace == RubikFaceType.Front && (faces.Contains(RubikFaceType.Right)))
                            clockwise = !clockwise;
                        if (selectedFace == RubikFaceType.Bottom && (faces.Contains(RubikFaceType.Right)))
                            clockwise = !clockwise;

                        Debug.Log("Selected face: " + selectedFace);

                        Cube.Rotate(selectedFace, clockwise);
                    }

                    _currentSubcube = null;
                }
            }

            ClearSelectedCube();
        }

        private void DetectSelectedCube()
        {
            if (_currentSubcube == null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform != null)
                        {
                            var subcube = hit.transform.gameObject.GetComponent<RubikSubcube>();
                            if (subcube != null)
                            {
                                _currentSubcube = subcube;
                                _initialPoint = Input.mousePosition;
                                CameraController.CanMove = false;
                            }
                        }
                    }
                }
            }
        }

        private void ClearSelectedCube()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _currentSubcube = null;
                CameraController.CanMove = true;
            }
        }
    }
}
