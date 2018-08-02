using UnityEngine;
using UnityEngine.UI;

namespace MHLab.Games.Rubik
{
    public class SwipingModeActivator : MonoBehaviour
    {
        public Text Text;
        public GameObject RotateClockwiseButton;
        public GameObject RotateAnticlockwiseButton;

        protected void Awake()
        {
            SetGui();
        }

        public void OnSwipingButtonPressed()
        {
            ControlsManager.EnableSwiping = !ControlsManager.EnableSwiping;
            
            SetGui();
        }

        private void SetGui()
        {
            if (ControlsManager.EnableSwiping)
            {
                Text.text = "SWIPING: ON";
                RotateAnticlockwiseButton.SetActive(false);
                RotateClockwiseButton.SetActive(false);
            }
            else
            {
                Text.text = "SWIPING: OFF";
                RotateAnticlockwiseButton.SetActive(true);
                RotateClockwiseButton.SetActive(true);
            }
        }
    }
}
