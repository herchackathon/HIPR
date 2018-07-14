using UnityEngine;

namespace MHLab.UI
{
    public class EnableForLimitedTime : MonoBehaviour
    {
        private float _currentTime = 0f;
        private float _targetTime;

        public void EnableFor(float seconds)
        {
            _targetTime = seconds;
            this.gameObject.SetActive(true);
        }

        protected void Update()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _targetTime)
            {
                this.gameObject.SetActive(false);
                _currentTime = 0f;
                _targetTime = 0f;
            }
        }
    }
}
