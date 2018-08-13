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
    }
}
