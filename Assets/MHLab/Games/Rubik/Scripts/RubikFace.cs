using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MHLab.Games.Rubik
{
    public enum RubikFaceType
    {
        Top,
        Left,
        Front,
        Right,
        Bottom,
        Back,
        None
    }

    public struct RubikMove
    {
        public RubikFaceType Type;
        public int Index;

        public RubikFaceType VerticalMove;
        public bool InvertVerticalMove;

        public RubikFaceType HorizontalMove;
        public bool InvertHorizontalMove;
    }

    public class RubikFace : MonoBehaviour
    {
        public RubikFaceType Type;
        public int Index;

        public RubikFaceType VerticalMove;
        public bool InvertVerticalMove;

        public RubikFaceType HorizontalMove;
        public bool InvertHorizontalMove;
    }
}
