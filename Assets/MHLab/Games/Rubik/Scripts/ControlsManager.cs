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
        public static bool EnableSwiping = false;
        public RubikCube Cube;
        public float MovesActivationThreshold = 1f;

        private RubikFace _currentRubikFace;
        private Vector3 _initialPoint;

        public GameObject FrontFace;
        public GameObject BackFace;
        public GameObject LeftFace;
        public GameObject RightFace;
        public GameObject TopFace;
        public GameObject BottomFace;

        public GameObject FrontFaceSelection;
        public GameObject BackFaceSelection;
        public GameObject LeftFaceSelection;
        public GameObject RightFaceSelection;
        public GameObject TopFaceSelection;
        public GameObject BottomFaceSelection;

        public AudioClip OnSelectionSound;

        private bool _isBackFaceVisible = false;
        private List<float> _faceDistances = new List<float>(6);
        private RubikFaceType _nearestFace;

        private AudioSource _audioSource;
        private RubikFaceType _previousSelectedFace = RubikFaceType.None;

        protected void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _faceDistances.Add(Vector3.Distance(Camera.main.transform.position, FrontFace.transform.position));
            _faceDistances.Add(Vector3.Distance(Camera.main.transform.position, BackFace.transform.position));
            _faceDistances.Add(Vector3.Distance(Camera.main.transform.position, LeftFace.transform.position));
            _faceDistances.Add(Vector3.Distance(Camera.main.transform.position, RightFace.transform.position));
            _faceDistances.Add(Vector3.Distance(Camera.main.transform.position, TopFace.transform.position));
            _faceDistances.Add(Vector3.Distance(Camera.main.transform.position, BottomFace.transform.position));
        }

        protected void LateUpdate()
        {
            if (!CanMove) return;
            if(EnableSwiping) DeactivateAllFaceSelections();

            DetectNearestFace();
            CheckBackFaceVisibility();

            if (EnableSwiping)
            {
                DetectSelectedCube();

                if (_currentRubikFace != null)
                {
                    var position = Input.mousePosition;
                    var distance = Vector3.Distance(_initialPoint, position);

                    if (distance >= MovesActivationThreshold)
                    {
                        var directionX = position.x - _initialPoint.x;
                        var directionY = position.y - _initialPoint.y;

                        var cubeSelectedMove = Cube.GetStartingFaceData(_currentRubikFace);

                        bool isHorizontal = Mathf.Abs(directionX) > Mathf.Abs(directionY);
                        if (_nearestFace == RubikFaceType.Right || _nearestFace == RubikFaceType.Left)
                        {
                            if (_currentRubikFace.Type == RubikFaceType.Top ||
                                _currentRubikFace.Type == RubikFaceType.Bottom)
                                isHorizontal = !isHorizontal;
                        }

                        RubikFaceType selectedFace = RubikFaceType.None;
                        bool clockwise;

                        if (isHorizontal)
                        {
                            clockwise = (directionX >= 0);
                            bool isPositive = directionY >= 0;
                            if (isPositive)
                                selectedFace = cubeSelectedMove.HorizontalMove;
                            else
                                selectedFace = cubeSelectedMove.HorizontalMove;

                            if (cubeSelectedMove.InvertHorizontalMove)
                                clockwise = !clockwise;
                        }
                        else
                        {
                            clockwise = (directionY >= 0);
                            bool isPositive = directionX >= 0;
                            if (isPositive)
                                selectedFace = cubeSelectedMove.VerticalMove;
                            else
                                selectedFace = cubeSelectedMove.VerticalMove;

                            if (cubeSelectedMove.InvertVerticalMove)
                                clockwise = !clockwise;
                        }

                        /*if (selectedFace == RubikFaceType.Top || selectedFace == RubikFaceType.Bottom)
                        {
                            if (_isBackFaceVisible)
                                clockwise = !clockwise;
                        }*/

                        Debug.Log("Face: " + selectedFace + " - " + clockwise);
                        Cube.Rotate(selectedFace, clockwise);

                        _currentRubikFace = null;
                    }
                }

                ClearSelectedCube();
            }
            else
            {
                DetectSelectedCubeOnMouseUp();
                ActivateFaceSelection();
            }
        }

        private void DetectSelectedCube()
        {
            if (_currentRubikFace == null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform != null)
                        {
                            var subcube = hit.transform.gameObject.GetComponent<RubikFace>();
                            if (subcube != null)
                            {
                                _currentRubikFace = subcube;
                                _initialPoint = Input.mousePosition;
                                CameraController.CanMove = false;
                            }
                        }
                    }
                }
            }
        }

        private void DetectSelectedCubeOnMouseUp()
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform != null)
                    {
                        var subcube = hit.transform.gameObject.GetComponent<RubikFace>();
                        if (subcube != null)
                        {
                            _currentRubikFace = subcube;
                        }
                    }
                }
            }
        }

        private void ClearSelectedCube()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _currentRubikFace = null;
                CameraController.CanMove = true;
            }
        }

        private void CheckBackFaceVisibility()
        {
            RaycastHit hit;
            var ray = new Ray(Camera.main.transform.position, BackFace.transform.position - Camera.main.transform.position);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != null)
                {
                    var subcube = hit.transform.gameObject.name;
                    if (subcube == BackFace.name)
                    {
                        _isBackFaceVisible = true;
                        return;
                    }
                }
            }

            _isBackFaceVisible = false;
        }

        private void DetectNearestFace()
        {
            _faceDistances[0] = (Vector3.Distance(Camera.main.transform.position, FrontFace.transform.position));
            _faceDistances[1] = (Vector3.Distance(Camera.main.transform.position, BackFace.transform.position));
            _faceDistances[2] = (Vector3.Distance(Camera.main.transform.position, LeftFace.transform.position));
            _faceDistances[3] = (Vector3.Distance(Camera.main.transform.position, RightFace.transform.position));
            _faceDistances[4] = (Vector3.Distance(Camera.main.transform.position, TopFace.transform.position));
            _faceDistances[5] = (Vector3.Distance(Camera.main.transform.position, BottomFace.transform.position));

            int index = 0;
            float tempNearest = _faceDistances[0];
            for (int i = 1; i < 6; i++)
            {
                if (_faceDistances[i] < tempNearest)
                {
                    tempNearest = _faceDistances[i];
                    index = i;
                }
            }

            switch (index)
            {
                case 0:
                    _nearestFace = RubikFaceType.Front;
                    break;
                case 1:
                    _nearestFace = RubikFaceType.Back;
                    break;
                case 2:
                    _nearestFace = RubikFaceType.Left;
                    break;
                case 3:
                    _nearestFace = RubikFaceType.Right;
                    break;
                case 4:
                    _nearestFace = RubikFaceType.Top;
                    break;
                case 5:
                    _nearestFace = RubikFaceType.Bottom;
                    break;
            }
        }

        private void ActivateFaceSelection()
        {
            DeactivateAllFaceSelections();

            if (_currentRubikFace == null) return;

            switch (_currentRubikFace.Type)
            {
                case RubikFaceType.Top:
                    TopFaceSelection.SetActive(true);
                    break;
                case RubikFaceType.Left:
                    LeftFaceSelection.SetActive(true);
                    break;
                case RubikFaceType.Front:
                    FrontFaceSelection.SetActive(true);
                    break;
                case RubikFaceType.Right:
                    RightFaceSelection.SetActive(true);
                    break;
                case RubikFaceType.Bottom:
                    BottomFaceSelection.SetActive(true);
                    break;
                case RubikFaceType.Back:
                    BackFaceSelection.SetActive(true);
                    break;
                case RubikFaceType.None:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_currentRubikFace.Type != _previousSelectedFace)
            {
                _audioSource.PlayOneShot(OnSelectionSound);
                _previousSelectedFace = _currentRubikFace.Type;
            }
        }

        private void DeactivateAllFaceSelections()
        {
            FrontFaceSelection.SetActive(false);
            BackFaceSelection.SetActive(false);
            LeftFaceSelection.SetActive(false);
            RightFaceSelection.SetActive(false);
            TopFaceSelection.SetActive(false);
            BottomFaceSelection.SetActive(false);
        }

        public void RotateClockwise()
        {
            if (_currentRubikFace == null) return;
            Cube.Rotate(_currentRubikFace.Type, true);
        }

        public void RotateAnticlockwise()
        {
            if (_currentRubikFace == null) return;
            Cube.Rotate(_currentRubikFace.Type, false);
        }
    }
}
