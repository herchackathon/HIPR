using UnityEngine;
using UnityEngine.EventSystems;

namespace MHLab.UI
{
    public class MenuButtonNavigation : MonoBehaviour, IPointerClickHandler
    {
        public RectTransform TargetScreen;
        public RectTransform OwnerScreen;

        public void OnClick()
        {
            OwnerScreen.gameObject.SetActive(false);
            TargetScreen.gameObject.SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }
    }
}
