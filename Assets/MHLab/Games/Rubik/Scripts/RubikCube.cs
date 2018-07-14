using System;
using System.Collections;
using System.Collections.Generic;
using MHLab.SlidingTilePuzzle;
using MHLab.UI;
using UnityEngine;

namespace MHLab.Games.Rubik
{
    public class RubikCube : MonoBehaviour
    {
        /*
        
                    _______
                   |       |
                   |   T   |
            _______|_______|_______ _______
           |       |       |       |       |
           |   L   |   F   |   R   |  BA   |
           |_______|_______|_______|_______|
                   |       |
                   |  BOT  |
                   |_______|

        */

        #region Fields
        public float TimeRequiredForRotation = 0.5f;

        public EnableForLimitedTime ShufflingPopup;
        public EnableForLimitedTime LetsgoPopup;

        public RubikSubcube[] Front;
        public RubikSubcube[] Left;
        public RubikSubcube[] Top;
        public RubikSubcube[] Right;
        public RubikSubcube[] Back;
        public RubikSubcube[] Bottom;

        private readonly RubikSubcube[] _solutionFront = new RubikSubcube[9];
        private readonly RubikSubcube[] _solutionLeft = new RubikSubcube[9];
        private readonly RubikSubcube[] _solutionTop = new RubikSubcube[9];
        private readonly RubikSubcube[] _solutionRight = new RubikSubcube[9];
        private readonly RubikSubcube[] _solutionBack = new RubikSubcube[9];
        private readonly RubikSubcube[] _solutionBottom = new RubikSubcube[9];

        private bool _canMove = false;
        private bool _isCompleted = false;

        private readonly Array _moveTypes = Enum.GetValues(typeof(RubikFaceType));
        private int _numberOfShuffles;
        private int _shuffleCounter = 0;
        private bool _isShuffling = false;
        private bool _shuffleMode = true;
        #endregion

        protected void Awake()
        {
            // Storing the solved state
            Array.Copy(Front, _solutionFront, 9);
            Array.Copy(Left, _solutionLeft, 9);
            Array.Copy(Top, _solutionTop, 9);
            Array.Copy(Right, _solutionRight, 9);
            Array.Copy(Back, _solutionBack, 9);
            Array.Copy(Bottom, _solutionBottom, 9);

            _numberOfShuffles = UnityEngine.Random.Range(25, 50);

            _canMove = true;
            _isShuffling = true;

            ShufflingPopup.EnableFor(2);
        }

        private RubikSubcube[] RotateFace(RubikSubcube[] face, bool clockwise)
        {
            var newFace = new RubikSubcube[9];
            if (clockwise)
            {
                newFace[0] = face[2];
                newFace[1] = face[5];
                newFace[2] = face[8];
                newFace[3] = face[1];
                newFace[4] = face[4];
                newFace[5] = face[7];
                newFace[6] = face[0];
                newFace[7] = face[3];
                newFace[8] = face[6];
            }
            else
            {
                newFace[0] = face[6];
                newFace[1] = face[3];
                newFace[2] = face[0];
                newFace[3] = face[7];
                newFace[4] = face[4];
                newFace[5] = face[1];
                newFace[6] = face[8];
                newFace[7] = face[5];
                newFace[8] = face[2];
            }

            return newFace;
        }

        private IEnumerator RotateFront(bool clockwise, Action callback = null)
        {
            if (_canMove)
            {
                BeginMove();

                Front = RotateFace(Front, clockwise);
                
                Left[2] = Front[0];
                Left[5] = Front[3];
                Left[8] = Front[6];

                Top[0] = Front[6];
                Top[1] = Front[7];
                Top[2] = Front[8];

                Right[0] = Front[2];
                Right[3] = Front[5];
                Right[6] = Front[8];

                Bottom[6] = Front[0];
                Bottom[7] = Front[1];
                Bottom[8] = Front[2];

                var center = Front[4];

                foreach (var rubikSubcube in Front)
                {
                    rubikSubcube.transform.parent = center.transform;
                }
                
                var elapsedTime = 0f;
                var degree = 90.0f / TimeRequiredForRotation;
                var alreadyRotatedDegree = 0f;

                while (elapsedTime < TimeRequiredForRotation)
                {
                    elapsedTime += Time.deltaTime;
                    var degreeToRotate = degree * Time.deltaTime;

                    if (alreadyRotatedDegree < 90f)
                    {
                        center.transform.Rotate((clockwise) ? Vector3.back : Vector3.forward, degreeToRotate);
                        alreadyRotatedDegree += degreeToRotate;
                    }
                    
                    yield return new WaitForEndOfFrame();
                }

                if(alreadyRotatedDegree > 90f)
                {
                    center.transform.Rotate((clockwise) ? Vector3.back : Vector3.forward, 90f - alreadyRotatedDegree);
                }

                foreach (var rubikSubcube in Front)
                {
                    rubikSubcube.transform.parent = this.transform;
                }

                EndMove(callback);
            }
        }

        private IEnumerator RotateBack(bool clockwise, Action callback = null)
        {
            if (_canMove)
            {
                BeginMove();

                Back = RotateFace(Back, clockwise);

                Right[2] = Back[0];
                Right[5] = Back[3];
                Right[8] = Back[6];

                Top[8] = Back[6];
                Top[7] = Back[7];
                Top[6] = Back[8];

                Left[0] = Back[2];
                Left[3] = Back[5];
                Left[6] = Back[8];

                Bottom[2] = Back[0];
                Bottom[1] = Back[1];
                Bottom[0] = Back[2];

                var center = Back[4];

                foreach (var rubikSubcube in Back)
                {
                    rubikSubcube.transform.parent = center.transform;
                }

                var elapsedTime = 0f;
                var degree = 90.0f / TimeRequiredForRotation;
                var alreadyRotatedDegree = 0f;

                while (elapsedTime < TimeRequiredForRotation)
                {
                    elapsedTime += Time.deltaTime;
                    var degreeToRotate = degree * Time.deltaTime;

                    if (alreadyRotatedDegree < 90f)
                    {
                        center.transform.Rotate((clockwise) ? Vector3.forward : Vector3.back, degreeToRotate);
                        alreadyRotatedDegree += degreeToRotate;
                    }

                    yield return new WaitForEndOfFrame();
                }

                if (alreadyRotatedDegree > 90f)
                {
                    center.transform.Rotate((clockwise) ? Vector3.forward : Vector3.back, 90f - alreadyRotatedDegree);
                }

                foreach (var rubikSubcube in Back)
                {
                    rubikSubcube.transform.parent = this.transform;
                }

                EndMove(callback);
            }
        }

        private IEnumerator RotateLeft(bool clockwise, Action callback = null)
        {
            if (_canMove)
            {
                BeginMove();

                Left = RotateFace(Left, clockwise);

                Back[2] = Left[0];
                Back[5] = Left[3];
                Back[8] = Left[6];

                Top[6] = Left[6];
                Top[3] = Left[7];
                Top[0] = Left[8];

                Front[0] = Left[2];
                Front[3] = Left[5];
                Front[6] = Left[8];

                Bottom[0] = Left[0];
                Bottom[3] = Left[1];
                Bottom[6] = Left[2];

                var center = Left[4];

                foreach (var rubikSubcube in Left)
                {
                    rubikSubcube.transform.parent = center.transform;
                }

                var elapsedTime = 0f;
                var degree = 90.0f / TimeRequiredForRotation;
                var alreadyRotatedDegree = 0f;

                while (elapsedTime < TimeRequiredForRotation)
                {
                    elapsedTime += Time.deltaTime;
                    var degreeToRotate = degree * Time.deltaTime;

                    if (alreadyRotatedDegree < 90f)
                    {
                        center.transform.Rotate((clockwise) ? Vector3.left : Vector3.right, degreeToRotate);
                        alreadyRotatedDegree += degreeToRotate;
                    }

                    yield return new WaitForEndOfFrame();
                }

                if (alreadyRotatedDegree > 90f)
                {
                    center.transform.Rotate((clockwise) ? Vector3.left : Vector3.right, 90f - alreadyRotatedDegree);
                }

                foreach (var rubikSubcube in Left)
                {
                    rubikSubcube.transform.parent = this.transform;
                }

                EndMove(callback);
            }
        }

        private IEnumerator RotateRight(bool clockwise, Action callback = null)
        {
            if (_canMove)
            {
                BeginMove();

                Right = RotateFace(Right, clockwise);

                Front[2] = Right[0];
                Front[5] = Right[3];
                Front[8] = Right[6];

                Top[2] = Right[6];
                Top[5] = Right[7];
                Top[8] = Right[8];

                Back[0] = Right[2];
                Back[3] = Right[5];
                Back[6] = Right[8];

                Bottom[8] = Right[0];
                Bottom[5] = Right[1];
                Bottom[2] = Right[2];

                var center = Right[4];

                foreach (var rubikSubcube in Right)
                {
                    rubikSubcube.transform.parent = center.transform;
                }

                var elapsedTime = 0f;
                var degree = 90.0f / TimeRequiredForRotation;
                var alreadyRotatedDegree = 0f;

                while (elapsedTime < TimeRequiredForRotation)
                {
                    elapsedTime += Time.deltaTime;
                    var degreeToRotate = degree * Time.deltaTime;

                    if (alreadyRotatedDegree < 90f)
                    {
                        center.transform.Rotate((clockwise) ? Vector3.right : Vector3.left, degreeToRotate);
                        alreadyRotatedDegree += degreeToRotate;
                    }

                    yield return new WaitForEndOfFrame();
                }

                if (alreadyRotatedDegree > 90f)
                {
                    center.transform.Rotate((clockwise) ? Vector3.right : Vector3.left, 90f - alreadyRotatedDegree);
                }

                foreach (var rubikSubcube in Right)
                {
                    rubikSubcube.transform.parent = this.transform;
                }

                EndMove(callback);
            }
        }

        private IEnumerator RotateTop(bool clockwise, Action callback = null)
        {
            if (_canMove)
            {
                BeginMove();

                Top = RotateFace(Top, clockwise);

                Left[8] = Top[0];
                Left[7] = Top[3];
                Left[6] = Top[6];

                Back[8] = Top[6];
                Back[7] = Top[7];
                Back[6] = Top[8];

                Right[6] = Top[2];
                Right[7] = Top[5];
                Right[8] = Top[8];

                Front[6] = Top[0];
                Front[7] = Top[1];
                Front[8] = Top[2];

                var center = Top[4];

                foreach (var rubikSubcube in Top)
                {
                    rubikSubcube.transform.parent = center.transform;
                }

                var elapsedTime = 0f;
                var degree = 90.0f / TimeRequiredForRotation;
                var alreadyRotatedDegree = 0f;

                while (elapsedTime < TimeRequiredForRotation)
                {
                    elapsedTime += Time.deltaTime;
                    var degreeToRotate = degree * Time.deltaTime;

                    if (alreadyRotatedDegree < 90f)
                    {
                        center.transform.Rotate((clockwise) ? Vector3.up : Vector3.down, degreeToRotate);
                        alreadyRotatedDegree += degreeToRotate;
                    }

                    yield return new WaitForEndOfFrame();
                }

                if (alreadyRotatedDegree > 90f)
                {
                    center.transform.Rotate((clockwise) ? Vector3.up : Vector3.down, 90f - alreadyRotatedDegree);
                }

                foreach (var rubikSubcube in Top)
                {
                    rubikSubcube.transform.parent = this.transform;
                }

                EndMove(callback);
            }
        }

        private IEnumerator RotateBottom(bool clockwise, Action callback = null)
        {
            if (_canMove)
            {
                BeginMove();

                Bottom = RotateFace(Bottom, clockwise);

                Left[0] = Bottom[0];
                Left[1] = Bottom[3];
                Left[2] = Bottom[6];

                Front[0] = Bottom[6];
                Front[1] = Bottom[7];
                Front[2] = Bottom[8];

                Right[2] = Bottom[2];
                Right[1] = Bottom[5];
                Right[0] = Bottom[8];

                Back[2] = Bottom[0];
                Back[1] = Bottom[1];
                Back[0] = Bottom[2];

                var center = Bottom[4];

                foreach (var rubikSubcube in Bottom)
                {
                    rubikSubcube.transform.parent = center.transform;
                }

                var elapsedTime = 0f;
                var degree = 90.0f / TimeRequiredForRotation;
                var alreadyRotatedDegree = 0f;

                while (elapsedTime < TimeRequiredForRotation)
                {
                    elapsedTime += Time.deltaTime;
                    var degreeToRotate = degree * Time.deltaTime;

                    if (alreadyRotatedDegree < 90f)
                    {
                        center.transform.Rotate((clockwise) ? Vector3.down : Vector3.up, degreeToRotate);
                        alreadyRotatedDegree += degreeToRotate;
                    }

                    yield return new WaitForEndOfFrame();
                }

                if (alreadyRotatedDegree > 90f)
                {
                    center.transform.Rotate((clockwise) ? Vector3.down : Vector3.up, 90f - alreadyRotatedDegree);
                }

                foreach (var rubikSubcube in Bottom)
                {
                    rubikSubcube.transform.parent = this.transform;
                }

                EndMove(callback);
            }
        }

        private void BeginMove()
        {
            _canMove = false;
        }

        private void EndMove(Action callback)
        {
            _isCompleted = CheckForCorrectness();

            if (callback != null)
                callback.Invoke();

            if(!_shuffleMode)
                MovesCountUpdater.AddMoves();

            _canMove = true;
        }
        
        public void Rotate(RubikFaceType type, bool clockwise, Action callback = null)
        {
            switch (type)
            {
                case RubikFaceType.Front:
                    StartCoroutine(RotateFront(clockwise, callback));
                    break;
                case RubikFaceType.Top:
                    StartCoroutine(RotateTop(clockwise, callback));
                    break;
                case RubikFaceType.Left:
                    StartCoroutine(RotateLeft(clockwise, callback));
                    break;
                case RubikFaceType.Right:
                    StartCoroutine(RotateRight(clockwise, callback));
                    break;
                case RubikFaceType.Bottom:
                    StartCoroutine(RotateBottom(clockwise, callback));
                    break;
                case RubikFaceType.Back:
                    StartCoroutine(RotateBack(clockwise, callback));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }

        public bool CheckForCorrectness()
        {
            for (var i = 0; i < 9; i++)
            {
                if (_solutionFront[i].GetInstanceID() != Front[i].GetInstanceID())
                    return false;
            }
            for (var i = 0; i < 9; i++)
            {
                if (_solutionLeft[i].GetInstanceID() != Left[i].GetInstanceID())
                    return false;
            }
            for (var i = 0; i < 9; i++)
            {
                if (_solutionRight[i].GetInstanceID() != Right[i].GetInstanceID())
                    return false;
            }
            for (var i = 0; i < 9; i++)
            {
                if (_solutionTop[i].GetInstanceID() != Top[i].GetInstanceID())
                    return false;
            }
            for (var i = 0; i < 9; i++)
            {
                if (_solutionBack[i].GetInstanceID() != Back[i].GetInstanceID())
                    return false;
            }
            for (var i = 0; i < 9; i++)
            {
                if (_solutionBottom[i].GetInstanceID() != Bottom[i].GetInstanceID())
                    return false;
            }

            // Ok, if we are here the puzzle is completed.
            OnCompleted();

            return true;
        }

        public List<RubikFaceType> GetSubcubeFaces(RubikSubcube subcube)
        {
            var faces = new List<RubikFaceType>();

            for (var i = 0; i < 9; i++)
            {
                if (subcube.GetInstanceID() == Front[i].GetInstanceID())
                {
                    faces.Add(RubikFaceType.Front);
                    break;
                }
            }
            for (var i = 0; i < 9; i++)
            {
                if (subcube.GetInstanceID() == Left[i].GetInstanceID())
                {
                    faces.Add(RubikFaceType.Left);
                    break;
                }
            }
            for (var i = 0; i < 9; i++)
            {
                if (subcube.GetInstanceID() == Right[i].GetInstanceID())
                {
                    faces.Add(RubikFaceType.Right);
                    break;
                }
            }
            for (var i = 0; i < 9; i++)
            {
                if (subcube.GetInstanceID() == Top[i].GetInstanceID())
                {
                    faces.Add(RubikFaceType.Top);
                    break;
                }
            }
            for (var i = 0; i < 9; i++)
            {
                if (subcube.GetInstanceID() == Back[i].GetInstanceID())
                {
                    faces.Add(RubikFaceType.Back);
                    break;
                }
            }
            for (var i = 0; i < 9; i++)
            {
                if (subcube.GetInstanceID() == Bottom[i].GetInstanceID())
                {
                    faces.Add(RubikFaceType.Bottom);
                    break;
                }
            }

            return faces;
        }

        private void OnCompleted()
        {
            GameTimerUpdater.StopTimer();
        }

        private void Shuffle()
        {
            /*var type = (RubikFaceType)_moveTypes.GetValue(UnityEngine.Random.Range(0, _moveTypes.Length));
            var clockwise = (UnityEngine.Random.value <= 0.5f);
            _isShuffling = true;

            Rotate(type, clockwise, () =>
            {
                _shuffleCounter++;

                if (_shuffleCounter < _numberOfShuffles)
                {
                    Shuffle();
                }
                else
                {
                    _isShuffling = false;
                    GameTimerUpdater.StartTimer();
                    Debug.Log("asdsasd");
                }
            });*/
        }
        
        protected void Update()
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                Rotate(RubikFaceType.Front, true);
            }
            if (Input.GetKeyUp(KeyCode.L))
            {
                Rotate(RubikFaceType.Left, false);
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                Rotate(RubikFaceType.Right, false);
            }
            if (Input.GetKeyUp(KeyCode.T))
            {
                Rotate(RubikFaceType.Top, false);
            }
            if (Input.GetKeyUp(KeyCode.B))
            {
                Rotate(RubikFaceType.Bottom, false);
            }
            if (Input.GetKeyUp(KeyCode.U))
            {
                Rotate(RubikFaceType.Back, false);
            }

            if (_shuffleMode)
            {
                if (_shuffleCounter < _numberOfShuffles && _canMove)
                {
                    var type = (RubikFaceType) _moveTypes.GetValue(UnityEngine.Random.Range(0, _moveTypes.Length));
                    var clockwise = (UnityEngine.Random.value <= 0.5f);
                    _isShuffling = true;

                    Rotate(type, clockwise, () =>
                    {
                        _shuffleCounter++;
                        _isShuffling = false;
                    });
                }
            }

            if (_shuffleMode && _shuffleCounter >= _numberOfShuffles)
            {
                GameTimerUpdater.StartTimer();
                _shuffleMode = false;
                LetsgoPopup.EnableFor(1);
                ControlsManager.CanMove = true;
            }
        }
    }
}