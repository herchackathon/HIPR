using UnityEngine;
using UnityEngine.UI;

namespace MHLab.Games.Rubik
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
            }
            else
            {
                AudioListener.volume = 1f;
                Text.text = "SOUNDS: ON";
            }
        }
    }
}
