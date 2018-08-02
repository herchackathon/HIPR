using System;
using System.Collections;
using System.Collections.Generic;
using HIPR.Encoding;
using MHLab.SlidingTilePuzzle;
using MHLab.UI;
using MHLab.Utilities;
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
        public Texture2D PuzzleImage;
        public Shader PuzzleShader;

        public GameObject[] TilesForSteganography;

        public EnableForLimitedTime ShufflingPopup;
        public EnableForLimitedTime LetsgoPopup;

        public RubikSubcube[] Front;
        public RubikSubcube[] Left;
        public RubikSubcube[] Top;
        public RubikSubcube[] Right;
        public RubikSubcube[] Back;
        public RubikSubcube[] Bottom;

        public AudioClip VictorySound;
        public AudioClip OnMoveSound;

        private readonly RubikSubcube[] _solutionFront = new RubikSubcube[9];
        private readonly RubikSubcube[] _solutionLeft = new RubikSubcube[9];
        private readonly RubikSubcube[] _solutionTop = new RubikSubcube[9];
        private readonly RubikSubcube[] _solutionRight = new RubikSubcube[9];
        private readonly RubikSubcube[] _solutionBack = new RubikSubcube[9];
        private readonly RubikSubcube[] _solutionBottom = new RubikSubcube[9];

        private readonly Dictionary<RubikFaceType, List<RubikFace>> _faces = new Dictionary<RubikFaceType, List<RubikFace>>(6);
        private readonly Dictionary<RubikFaceType, List<RubikMove>> _moves = new Dictionary<RubikFaceType, List<RubikMove>>(6);

        private bool _canMove = false;
        private bool _isCompleted = false;

        private readonly Array _moveTypes = Enum.GetValues(typeof(RubikFaceType));
        private int _numberOfShuffles;
        private int _shuffleCounter = 0;
        private bool _isShuffling = false;
        private bool _shuffleMode = true;

        private AudioSource _audioSource;

        //private RubikFaceType _selectedFace = RubikFaceType.None;
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

            var faces = GetComponentsInChildren<RubikFace>();
            foreach (var rubikFace in faces)
            {
                if (!_faces.ContainsKey(rubikFace.Type))
                {
                    _faces.Add(rubikFace.Type, new List<RubikFace>(9));
                    _moves.Add(rubikFace.Type, new List<RubikMove>(9));
                    for (int i = 0; i < 9; i++)
                    {
                        _faces[rubikFace.Type].Add(null);
                        _moves[rubikFace.Type].Add(new RubikMove());
                    }
                }
                _faces[rubikFace.Type][rubikFace.Index] = rubikFace;
                _moves[rubikFace.Type][rubikFace.Index] = new RubikMove()
                {
                    Index =  rubikFace.Index,
                    Type = rubikFace.Type,
                    VerticalMove = rubikFace.VerticalMove,
                    InvertVerticalMove = rubikFace.InvertVerticalMove,
                    HorizontalMove = rubikFace.HorizontalMove,
                    InvertHorizontalMove = rubikFace.InvertHorizontalMove
                };
            }

            _numberOfShuffles = UnityEngine.Random.Range(25, 50);

            InitializeTextures();

            _audioSource = GetComponent<AudioSource>();

            _canMove = true;
            _isShuffling = true;

            ShufflingPopup.EnableFor(2);
        }

        private void InitializeTextures()
        {
            PuzzleImage = Steganography.Encode(PuzzleImage, "1234");

            int index = 0;

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    var material = new Material(PuzzleShader)
                    {
                        mainTexture = PuzzleImage,
                        mainTextureOffset = new Vector2(1.0f / 3 * j, 1.0f / 3 * i),
                        mainTextureScale = new Vector2(1.0f / 3, 1.0f / 3)
                    };
                    
                    // assign the new material to this tile for display.
                    TilesForSteganography[index].GetComponent<Renderer>().material = material;
                    index++;
                }
            }
        }

        private RubikSubcube[] RotateFace(RubikSubcube[] face, RubikFaceType type, bool clockwise)
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

                var faces = _faces[type];
                var newFaces = new List<RubikFace>(9)
                {
                    faces[2],
                    faces[5],
                    faces[8],
                    faces[1],
                    faces[4],
                    faces[7],
                    faces[0],
                    faces[3],
                    faces[6]
                };

                for (int i = 0; i < 9; i++)
                {
                    newFaces[i].Index = i;
                    newFaces[i].Type = type;
                }

                _faces[type] = newFaces;
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
                
                var faces = _faces[type];
                var newFaces = new List<RubikFace>(9)
                {
                    faces[6],
                    faces[3],
                    faces[0],
                    faces[7],
                    faces[4],
                    faces[1],
                    faces[8],
                    faces[5],
                    faces[2]
                };

                for (int i = 0; i < 9; i++)
                    newFaces[i].Index = i;

                _faces[type] = newFaces;
            }

            return newFace;
        }

        private void SwitchFaces(RubikFaceType from, int indexFrom, RubikFaceType to, int indexTo)
        {
            var faceFrom = _faces[from][indexFrom];
            faceFrom.Index = indexTo;
            faceFrom.Type = to;

            _faces[to][indexTo] = faceFrom;
        }

        private void SwitchFaces(RubikFace from, RubikFaceType to, int indexTo)
        {
            from.Index = indexTo;
            from.Type = to;

            _faces[to][indexTo] = from;
        }

        private IEnumerator RotateFront(bool clockwise, Action callback = null)
        {
            if (_canMove)
            {
                BeginMove();

                Front = RotateFace(Front, RubikFaceType.Front, clockwise);
                
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

                if (clockwise)
                {
                    var temp0 = _faces[RubikFaceType.Right][0];
                    var temp3 = _faces[RubikFaceType.Right][3];
                    var temp6 = _faces[RubikFaceType.Right][6];
                    SwitchFaces(RubikFaceType.Top, 2, RubikFaceType.Right, 0);
                    SwitchFaces(RubikFaceType.Top, 1, RubikFaceType.Right, 3);
                    SwitchFaces(RubikFaceType.Top, 0, RubikFaceType.Right, 6);
                    SwitchFaces(RubikFaceType.Left, 2, RubikFaceType.Top, 0);
                    SwitchFaces(RubikFaceType.Left, 5, RubikFaceType.Top, 1);
                    SwitchFaces(RubikFaceType.Left, 8, RubikFaceType.Top, 2);
                    SwitchFaces(RubikFaceType.Bottom, 8, RubikFaceType.Left, 2);
                    SwitchFaces(RubikFaceType.Bottom, 7, RubikFaceType.Left, 5);
                    SwitchFaces(RubikFaceType.Bottom, 6, RubikFaceType.Left, 8);
                    SwitchFaces(temp0, RubikFaceType.Bottom, 6);
                    SwitchFaces(temp3, RubikFaceType.Bottom, 7);
                    SwitchFaces(temp6, RubikFaceType.Bottom, 8);

                }
                else
                {
                    var temp0 = _faces[RubikFaceType.Right][0];
                    var temp3 = _faces[RubikFaceType.Right][3];
                    var temp6 = _faces[RubikFaceType.Right][6];
                    SwitchFaces(RubikFaceType.Bottom, 6, RubikFaceType.Right, 0);
                    SwitchFaces(RubikFaceType.Bottom, 7, RubikFaceType.Right, 3);
                    SwitchFaces(RubikFaceType.Bottom, 8, RubikFaceType.Right, 6);
                    SwitchFaces(RubikFaceType.Left, 8, RubikFaceType.Bottom, 6);
                    SwitchFaces(RubikFaceType.Left, 5, RubikFaceType.Bottom, 7);
                    SwitchFaces(RubikFaceType.Left, 2, RubikFaceType.Bottom, 8);
                    SwitchFaces(RubikFaceType.Top, 0, RubikFaceType.Left, 2);
                    SwitchFaces(RubikFaceType.Top, 1, RubikFaceType.Left, 5);
                    SwitchFaces(RubikFaceType.Top, 2, RubikFaceType.Left, 8);
                    SwitchFaces(temp6, RubikFaceType.Top, 0);
                    SwitchFaces(temp3, RubikFaceType.Top, 1);
                    SwitchFaces(temp0, RubikFaceType.Top, 2);
                }

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

                Back = RotateFace(Back, RubikFaceType.Back, clockwise);

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

                if (clockwise)
                {
                    var temp0 = _faces[RubikFaceType.Left][0];
                    var temp3 = _faces[RubikFaceType.Left][3];
                    var temp6 = _faces[RubikFaceType.Left][6];
                    SwitchFaces(RubikFaceType.Top, 6, RubikFaceType.Left, 0);
                    SwitchFaces(RubikFaceType.Top, 7, RubikFaceType.Left, 3);
                    SwitchFaces(RubikFaceType.Top, 8, RubikFaceType.Left, 6);
                    SwitchFaces(RubikFaceType.Right, 8, RubikFaceType.Top, 6);
                    SwitchFaces(RubikFaceType.Right, 5, RubikFaceType.Top, 7);
                    SwitchFaces(RubikFaceType.Right, 2, RubikFaceType.Top, 8);
                    SwitchFaces(RubikFaceType.Bottom, 0, RubikFaceType.Right, 2);
                    SwitchFaces(RubikFaceType.Bottom, 1, RubikFaceType.Right, 5);
                    SwitchFaces(RubikFaceType.Bottom, 2, RubikFaceType.Right, 8);
                    SwitchFaces(temp6, RubikFaceType.Bottom, 0);
                    SwitchFaces(temp3, RubikFaceType.Bottom, 1);
                    SwitchFaces(temp0, RubikFaceType.Bottom, 2);
                }
                else
                {
                    var temp0 = _faces[RubikFaceType.Left][0];
                    var temp3 = _faces[RubikFaceType.Left][3];
                    var temp6 = _faces[RubikFaceType.Left][6];
                    SwitchFaces(RubikFaceType.Bottom, 2, RubikFaceType.Left, 0);
                    SwitchFaces(RubikFaceType.Bottom, 1, RubikFaceType.Left, 3);
                    SwitchFaces(RubikFaceType.Bottom, 0, RubikFaceType.Left, 6);
                    SwitchFaces(RubikFaceType.Right, 2, RubikFaceType.Bottom, 0);
                    SwitchFaces(RubikFaceType.Right, 5, RubikFaceType.Bottom, 1);
                    SwitchFaces(RubikFaceType.Right, 8, RubikFaceType.Bottom, 2);
                    SwitchFaces(RubikFaceType.Top, 8, RubikFaceType.Right, 2);
                    SwitchFaces(RubikFaceType.Top, 7, RubikFaceType.Right, 5);
                    SwitchFaces(RubikFaceType.Top, 6, RubikFaceType.Right, 8);
                    SwitchFaces(temp0, RubikFaceType.Top, 6);
                    SwitchFaces(temp3, RubikFaceType.Top, 7);
                    SwitchFaces(temp6, RubikFaceType.Top, 8);
                }

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

                Left = RotateFace(Left, RubikFaceType.Left, clockwise);

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


                if (clockwise)
                {
                    var temp0 = _faces[RubikFaceType.Front][0];
                    var temp3 = _faces[RubikFaceType.Front][3];
                    var temp6 = _faces[RubikFaceType.Front][6];
                    SwitchFaces(RubikFaceType.Top, 0, RubikFaceType.Front, 0);
                    SwitchFaces(RubikFaceType.Top, 3, RubikFaceType.Front, 3);
                    SwitchFaces(RubikFaceType.Top, 6, RubikFaceType.Front, 6);
                    SwitchFaces(RubikFaceType.Back, 8, RubikFaceType.Top, 0);
                    SwitchFaces(RubikFaceType.Back, 5, RubikFaceType.Top, 3);
                    SwitchFaces(RubikFaceType.Back, 2, RubikFaceType.Top, 6);
                    SwitchFaces(RubikFaceType.Bottom, 6, RubikFaceType.Back, 2);
                    SwitchFaces(RubikFaceType.Bottom, 3, RubikFaceType.Back, 5);
                    SwitchFaces(RubikFaceType.Bottom, 0, RubikFaceType.Back, 8);
                    SwitchFaces(temp0, RubikFaceType.Bottom, 0);
                    SwitchFaces(temp3, RubikFaceType.Bottom, 3);
                    SwitchFaces(temp6, RubikFaceType.Bottom, 6);
                }
                else
                {
                    var temp0 = _faces[RubikFaceType.Front][0];
                    var temp3 = _faces[RubikFaceType.Front][3];
                    var temp6 = _faces[RubikFaceType.Front][6];
                    SwitchFaces(RubikFaceType.Bottom, 0, RubikFaceType.Front, 0);
                    SwitchFaces(RubikFaceType.Bottom, 3, RubikFaceType.Front, 3);
                    SwitchFaces(RubikFaceType.Bottom, 6, RubikFaceType.Front, 6);
                    SwitchFaces(RubikFaceType.Back, 8, RubikFaceType.Bottom, 0);
                    SwitchFaces(RubikFaceType.Back, 5, RubikFaceType.Bottom, 3);
                    SwitchFaces(RubikFaceType.Back, 2, RubikFaceType.Bottom, 6);
                    SwitchFaces(RubikFaceType.Top, 6, RubikFaceType.Back, 2);
                    SwitchFaces(RubikFaceType.Top, 3, RubikFaceType.Back, 5);
                    SwitchFaces(RubikFaceType.Top, 0, RubikFaceType.Back, 8);
                    SwitchFaces(temp0, RubikFaceType.Top, 0);
                    SwitchFaces(temp3, RubikFaceType.Top, 3);
                    SwitchFaces(temp6, RubikFaceType.Top, 6);
                }

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

                Right = RotateFace(Right, RubikFaceType.Right, clockwise);

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

                if (clockwise)
                {
                    var temp0 = _faces[RubikFaceType.Back][0];
                    var temp3 = _faces[RubikFaceType.Back][3];
                    var temp6 = _faces[RubikFaceType.Back][6];
                    SwitchFaces(RubikFaceType.Top, 8, RubikFaceType.Back, 0);
                    SwitchFaces(RubikFaceType.Top, 5, RubikFaceType.Back, 3);
                    SwitchFaces(RubikFaceType.Top, 2, RubikFaceType.Back, 6);
                    SwitchFaces(RubikFaceType.Front, 2, RubikFaceType.Top, 2);
                    SwitchFaces(RubikFaceType.Front, 5, RubikFaceType.Top, 5);
                    SwitchFaces(RubikFaceType.Front, 8, RubikFaceType.Top, 8);
                    SwitchFaces(RubikFaceType.Bottom, 2, RubikFaceType.Front, 2);
                    SwitchFaces(RubikFaceType.Bottom, 5, RubikFaceType.Front, 5);
                    SwitchFaces(RubikFaceType.Bottom, 8, RubikFaceType.Front, 8);
                    SwitchFaces(temp6, RubikFaceType.Bottom, 2);
                    SwitchFaces(temp3, RubikFaceType.Bottom, 5);
                    SwitchFaces(temp0, RubikFaceType.Bottom, 8);
                }
                else
                {
                    var temp0 = _faces[RubikFaceType.Back][0];
                    var temp3 = _faces[RubikFaceType.Back][3];
                    var temp6 = _faces[RubikFaceType.Back][6];
                    SwitchFaces(RubikFaceType.Bottom, 8, RubikFaceType.Back, 0);
                    SwitchFaces(RubikFaceType.Bottom, 5, RubikFaceType.Back, 3);
                    SwitchFaces(RubikFaceType.Bottom, 2, RubikFaceType.Back, 6);
                    SwitchFaces(RubikFaceType.Front, 2, RubikFaceType.Back, 2);
                    SwitchFaces(RubikFaceType.Front, 5, RubikFaceType.Bottom, 5);
                    SwitchFaces(RubikFaceType.Front, 8, RubikFaceType.Bottom, 8);
                    SwitchFaces(RubikFaceType.Top, 2, RubikFaceType.Front, 2);
                    SwitchFaces(RubikFaceType.Top, 5, RubikFaceType.Front, 5);
                    SwitchFaces(RubikFaceType.Top, 8, RubikFaceType.Front, 8);
                    SwitchFaces(temp6, RubikFaceType.Top, 2);
                    SwitchFaces(temp3, RubikFaceType.Top, 5);
                    SwitchFaces(temp0, RubikFaceType.Top, 8);
                }

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

                Top = RotateFace(Top, RubikFaceType.Top, clockwise);

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

                if (clockwise)
                {
                    var temp6 = _faces[RubikFaceType.Right][6];
                    var temp7 = _faces[RubikFaceType.Right][7];
                    var temp8 = _faces[RubikFaceType.Right][8];
                    SwitchFaces(RubikFaceType.Back, 6, RubikFaceType.Right, 6);
                    SwitchFaces(RubikFaceType.Back, 7, RubikFaceType.Right, 7);
                    SwitchFaces(RubikFaceType.Back, 8, RubikFaceType.Right, 8);
                    SwitchFaces(RubikFaceType.Left, 6, RubikFaceType.Back, 6);
                    SwitchFaces(RubikFaceType.Left, 7, RubikFaceType.Back, 7);
                    SwitchFaces(RubikFaceType.Left, 8, RubikFaceType.Back, 8);
                    SwitchFaces(RubikFaceType.Front, 6, RubikFaceType.Left, 6);
                    SwitchFaces(RubikFaceType.Front, 7, RubikFaceType.Left, 7);
                    SwitchFaces(RubikFaceType.Front, 8, RubikFaceType.Left, 8);
                    SwitchFaces(temp6, RubikFaceType.Front, 6);
                    SwitchFaces(temp7, RubikFaceType.Front, 7);
                    SwitchFaces(temp8, RubikFaceType.Front, 8);
                }
                else
                {
                    var temp6 = _faces[RubikFaceType.Right][6];
                    var temp7 = _faces[RubikFaceType.Right][7];
                    var temp8 = _faces[RubikFaceType.Right][8];
                    SwitchFaces(RubikFaceType.Front, 6, RubikFaceType.Right, 6);
                    SwitchFaces(RubikFaceType.Front, 7, RubikFaceType.Right, 7);
                    SwitchFaces(RubikFaceType.Front, 8, RubikFaceType.Right, 8);
                    SwitchFaces(RubikFaceType.Left, 6, RubikFaceType.Front, 6);
                    SwitchFaces(RubikFaceType.Left, 7, RubikFaceType.Front, 7);
                    SwitchFaces(RubikFaceType.Left, 8, RubikFaceType.Front, 8);
                    SwitchFaces(RubikFaceType.Back, 6, RubikFaceType.Left, 6);
                    SwitchFaces(RubikFaceType.Back, 7, RubikFaceType.Left, 7);
                    SwitchFaces(RubikFaceType.Back, 8, RubikFaceType.Left, 8);
                    SwitchFaces(temp6, RubikFaceType.Back, 6);
                    SwitchFaces(temp7, RubikFaceType.Back, 7);
                    SwitchFaces(temp8, RubikFaceType.Back, 8);
                }

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

                Bottom = RotateFace(Bottom, RubikFaceType.Bottom, clockwise);

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

                if (clockwise)
                {
                    var temp0 = _faces[RubikFaceType.Right][0];
                    var temp1 = _faces[RubikFaceType.Right][1];
                    var temp2 = _faces[RubikFaceType.Right][2];
                    SwitchFaces(RubikFaceType.Front, 0, RubikFaceType.Right, 0);
                    SwitchFaces(RubikFaceType.Front, 1, RubikFaceType.Right, 1);
                    SwitchFaces(RubikFaceType.Front, 2, RubikFaceType.Right, 2);
                    SwitchFaces(RubikFaceType.Left, 0, RubikFaceType.Front, 0);
                    SwitchFaces(RubikFaceType.Left, 1, RubikFaceType.Front, 1);
                    SwitchFaces(RubikFaceType.Left, 2, RubikFaceType.Front, 2);
                    SwitchFaces(RubikFaceType.Back, 0, RubikFaceType.Left, 0);
                    SwitchFaces(RubikFaceType.Back, 1, RubikFaceType.Left, 1);
                    SwitchFaces(RubikFaceType.Back, 2, RubikFaceType.Left, 2);
                    SwitchFaces(temp0, RubikFaceType.Back, 0);
                    SwitchFaces(temp1, RubikFaceType.Back, 1);
                    SwitchFaces(temp2, RubikFaceType.Back, 2);
                }
                else
                {
                    var temp0 = _faces[RubikFaceType.Right][0];
                    var temp1 = _faces[RubikFaceType.Right][1];
                    var temp2 = _faces[RubikFaceType.Right][2];
                    SwitchFaces(RubikFaceType.Back, 0, RubikFaceType.Right, 0);
                    SwitchFaces(RubikFaceType.Back, 1, RubikFaceType.Right, 1);
                    SwitchFaces(RubikFaceType.Back, 2, RubikFaceType.Right, 2);
                    SwitchFaces(RubikFaceType.Left, 0, RubikFaceType.Back, 0);
                    SwitchFaces(RubikFaceType.Left, 1, RubikFaceType.Back, 1);
                    SwitchFaces(RubikFaceType.Left, 2, RubikFaceType.Back, 2);
                    SwitchFaces(RubikFaceType.Front, 0, RubikFaceType.Left, 0);
                    SwitchFaces(RubikFaceType.Front, 1, RubikFaceType.Left, 1);
                    SwitchFaces(RubikFaceType.Front, 2, RubikFaceType.Left, 2);
                    SwitchFaces(temp0, RubikFaceType.Front, 0);
                    SwitchFaces(temp1, RubikFaceType.Front, 1);
                    SwitchFaces(temp2, RubikFaceType.Front, 2);
                }

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

            for (int i = 0; i < 9; i++)
            {
                var current = Front[i];
                var face = current.GetFaceWithType(RubikFaceType.Front);
                if (face != null)
                {
                    face.Index = i;
                    face.Type = RubikFaceType.Front;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                var current = Back[i];
                var face = current.GetFaceWithType(RubikFaceType.Back);
                if (face != null)
                {
                    face.Index = i;
                    face.Type = RubikFaceType.Back;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                var current = Left[i];
                var face = current.GetFaceWithType(RubikFaceType.Left);
                if (face != null)
                {
                    face.Index = i;
                    face.Type = RubikFaceType.Left;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                var current = Right[i];
                var face = current.GetFaceWithType(RubikFaceType.Right);
                if (face != null)
                {
                    face.Index = i;
                    face.Type = RubikFaceType.Right;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                var current = Top[i];
                var face = current.GetFaceWithType(RubikFaceType.Top);
                if (face != null)
                {
                    face.Index = i;
                    face.Type = RubikFaceType.Top;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                var current = Bottom[i];
                var face = current.GetFaceWithType(RubikFaceType.Bottom);
                if (face != null)
                {
                    face.Index = i;
                    face.Type = RubikFaceType.Bottom;
                }
            }

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
            _audioSource.PlayOneShot(OnMoveSound);
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

        public Result<RubikFaceType, int> GetSubcubeFaceAndIndex(RubikSubcube subcube)
        {
            var instanceId = subcube.GetInstanceID();

            for (var i = 0; i < 9; i++)
            {
                if (Front[i].GetInstanceID() == instanceId)
                {
                    return Result<RubikFaceType, int>.Create(RubikFaceType.Front, i);
                }
            }

            for (var i = 0; i < 9; i++)
            {
                if (Left[i].GetInstanceID() == instanceId)
                {
                    return Result<RubikFaceType, int>.Create(RubikFaceType.Left, i);
                }
            }

            for (var i = 0; i < 9; i++)
            {
                if (Right[i].GetInstanceID() == instanceId)
                {
                    return Result<RubikFaceType, int>.Create(RubikFaceType.Right, i);
                }
            }

            for (var i = 0; i < 9; i++)
            {
                if (Top[i].GetInstanceID() == instanceId)
                {
                    return Result<RubikFaceType, int>.Create(RubikFaceType.Top, i);
                }
            }

            for (var i = 0; i < 9; i++)
            {
                if (Back[i].GetInstanceID() == instanceId)
                {
                    return Result<RubikFaceType, int>.Create(RubikFaceType.Back, i);
                }
            }

            for (var i = 0; i < 9; i++)
            {
                if (Bottom[i].GetInstanceID() == instanceId)
                {
                    return Result<RubikFaceType, int>.Create(RubikFaceType.Bottom, i);
                }
            }

            throw new ArgumentException("This subcube is not contained in the initial state.", "subcube");
        }

        public RubikMove GetStartingFaceData(RubikFace face)
        {
            return _moves[face.Type][face.Index];
        }

        private void OnCompleted()
        {
            GameTimerUpdater.StopTimer();

            DecodeTexture();

            _audioSource.PlayOneShot(VictorySound);
        }

        private void DecodeTexture()
        {
            var texture = new Texture2D(PuzzleImage.width, PuzzleImage.height);

            var index = 0;

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    var tmpTexture = TilesForSteganography[index].GetComponent<Renderer>().material.mainTexture as Texture2D;
                    var pixels = tmpTexture.GetPixels((int)(texture.width / 3) * j, (int)(texture.height / 3) * i, (int)(texture.width / 3), (int)(texture.height / 3));
                    texture.SetPixels((int)(texture.width / 3) * j, (int)(texture.height / 3) * i, (int)(texture.width / 3), (int)(texture.height / 3), pixels);
                }
            }

            var decoded = Steganography.Decode(texture);
            Debug.Log(decoded);
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
                    var type = (RubikFaceType) _moveTypes.GetValue(UnityEngine.Random.Range(0, _moveTypes.Length - 1));
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