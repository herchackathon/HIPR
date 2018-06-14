using UnityEngine;

namespace MHLab.UI
{
    public class TriggerAnimation : MonoBehaviour
    {
        private Animation _anim;

        protected void Start()
        {
            _anim = GetComponent<Animation>();
        }

        public void Trigger(string anim)
        {
            _anim.Play(anim);
        }
    }
}