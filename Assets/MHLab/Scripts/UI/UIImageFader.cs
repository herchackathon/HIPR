using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MHLab.UI
{
    public class UIImageFader : MonoBehaviour
    {
        public float Delay = 2f;
        public float FadingTime = 2f;
        public Shader Shader;

        private float _timer;
        private bool _completed = false;
        
        private Material _material;

        private void Start()
        {
            GetComponent<Image>().material = new Material(GetComponent<Image>().material);
            _material = GetComponent<Image>().material;
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
            float alpha = _material.color.a;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
            {
                var newColor = new Color(_material.color.r, _material.color.g, _material.color.b, Mathf.Lerp(alpha, value, t));
                _material.color = newColor;
                yield return null;
            }

            _completed = true;
            this.gameObject.SetActive(false);
        }
    }
}
