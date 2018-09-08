using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MHLab.UI
{
    public class UIFader : MonoBehaviour
    {
        public float Delay = 2f;
        public float FadingTime = 2f;

        private Image _image;
        private float _timer;
        private bool _completed = false;

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            if (_completed) return;

            _timer += Time.deltaTime;

            if (_timer >= Delay)
            {
                StartCoroutine(FadeTo(0, FadingTime));
            }
        }

        private IEnumerator FadeTo(float value, float time)
        {
            float alpha = _image.color.a;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
            {
                var newColor = new Color(_image.color.r, _image.color.g, _image.color.b, Mathf.Lerp(alpha, value, t));
                _image.color = newColor;
                yield return null;
            }

            _completed = true;
            this.gameObject.SetActive(false);
        }
    }
}
