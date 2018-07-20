using UnityEngine;

namespace MHLab.Utilities
{
    public class BackgroundChanger : MonoBehaviour
    {
        public Material[] BackgroundMaterials;

        protected void Awake()
        {
            if (BackgroundMaterials.Length == 0) return;

            var index = UnityEngine.Random.Range(0, BackgroundMaterials.Length);
            RenderSettings.skybox = BackgroundMaterials[index];
        }
    }
}
