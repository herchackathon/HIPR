using System.Collections.Generic;
using UnityEngine;

namespace MHLab.Games.Rubik
{
    public class RubikSubcube : MonoBehaviour
    {
        public RubikFaceType FaceForVerticalMoveNegative;
        public RubikFaceType FaceForVerticalMovePositive;
        public bool InvertOrientationForVerticalMove;
        
        public RubikFaceType FaceForHorizontalMoveNegative;
        public RubikFaceType FaceForHorizontalMovePositive;
        public bool InvertOrientationForHorizontalMove;

        private RubikFace[] _faces;

        protected void Awake()
        {
            _faces = GetComponentsInChildren<RubikFace>();
        }

        public RubikFace GetFaceWithType(RubikFaceType type)
        {
            foreach (var rubikFace in _faces)
            {
                if (rubikFace.Type == type)
                    return rubikFace;
            }

            return null;
        }
    }
}
