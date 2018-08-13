using MHLab.SlidingTilePuzzle.Data;
using MHLab.Web.Storage;
using UnityEngine;
using UnityEngine.UI;

namespace MHLab.UI
{
    public class SoundsActivator : MonoBehaviour
    {
        public Text Text;

        public void OnSoundsButtonPressed()
        {
            if (AudioListener.volume > 0f)
            {
                AudioListener.volume = 0f;
                Text.text = "SOUNDS: OFF";
                LocalStorage.Store(StorageKeys.SoundsMode, 0);
            }
            else
            {
                AudioListener.volume = 1f;
                Text.text = "SOUNDS: ON";
                LocalStorage.Store(StorageKeys.SoundsMode, 1);
            }
        }
    }
}
