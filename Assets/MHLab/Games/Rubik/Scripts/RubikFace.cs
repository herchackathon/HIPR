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
        Back
    }

    public class RubikFace : MonoBehaviour
    {
        public RubikFaceType Type;
        public GameObject[] Cubies;
    }
}
