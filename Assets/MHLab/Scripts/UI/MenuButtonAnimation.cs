using UnityEngine;
using UnityEngine.EventSystems;

namespace MHLab.UI
{
    public class MenuButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TriggerAnimation _anim;

        protected void Awake()
        {
            _anim = GetComponent<TriggerAnimation>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!_anim.IsPlaying())
                _anim.Trigger("ButtonOver");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_anim.IsPlaying())
                _anim.Trigger("ButtonOut");
        }
    }
}
