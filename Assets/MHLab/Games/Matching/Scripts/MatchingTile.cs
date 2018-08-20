using MHLab.SlidingTilePuzzle.Data;
using MHLab.Web.Storage;
using UnityEngine;

namespace MHLab.Games.Matching
{
    public class MatchingTile : MonoBehaviour
    {
        public string Type;
        public Vector2 GridPosition;
        public Material ColorblindnessMaterial;

        private MeshRenderer _renderer;

        protected void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();

            if (LocalStorage.HasKey(StorageKeys.ColorBlindnessMode) && LocalStorage.GetInt(StorageKeys.ColorBlindnessMode).Value == 1)
            {
                _renderer.material = ColorblindnessMaterial;
            }
        }

        public bool IsNearTo(MatchingTile tile)
        {
            int thisX = (int)GridPosition.x;
            int thisY = (int)GridPosition.y;
            int neighbourX = (int)tile.GridPosition.x;
            int neighbourY = (int)tile.GridPosition.y;

            if (thisX == neighbourX)
            {
                if (neighbourY == thisY - 1) return true;
                if (neighbourY == thisY) return true;
                if (neighbourY == thisY + 1) return true;
            }
            if (thisY == neighbourY)
            {
                if (neighbourX == thisX - 1) return true;
                if (neighbourX == thisX) return true;
                if (neighbourX == thisX + 1) return true;
            }

            return false;
        }
    }
}
