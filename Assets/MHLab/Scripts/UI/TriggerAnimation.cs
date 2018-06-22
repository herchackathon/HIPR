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

        public bool IsPlaying()
        {
            return _anim.isPlaying;
        }

        public bool IsPlaying(string anim)
        {
            return _anim.IsPlaying(anim);
        }

        public void Trigger(string anim)
        {
            _anim.Play(anim);
        }
    }
}