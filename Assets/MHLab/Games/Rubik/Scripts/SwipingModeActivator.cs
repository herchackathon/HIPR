using UnityEngine;
using UnityEngine.UI;

namespace MHLab.Games.Rubik
{
    public class SwipingModeActivator : MonoBehaviour
    {
        public Text Text;
        public GameObject RotateClockwiseButton;
        public GameObject RotateAnticlockwiseButton;
        public GameObject ActivatedState;
        public GameObject DeactivatedState;

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
                ActivatedState.SetActive(true);
                DeactivatedState.SetActive(false);
                RotateAnticlockwiseButton.SetActive(false);
                RotateClockwiseButton.SetActive(false);
            }
            else
            {
                ActivatedState.SetActive(false);
                DeactivatedState.SetActive(true);
                RotateAnticlockwiseButton.SetActive(true);
                RotateClockwiseButton.SetActive(true);
            }
        }
    }
}
