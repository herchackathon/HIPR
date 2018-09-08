using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MHLab.UI.Tween;
using UnityEngine;

namespace MHLab.UI
{
    public class UIScaler : MonoBehaviour
    {
        public RectTransform TargetRect;
        public float Delay = 2f;
        public float Speed = 2f;

        private RectTransform _currentRect;
        private float _timer;
        private bool _completed = false;

        private void Start()
        {
            _currentRect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (_completed) return;

            _timer += Time.deltaTime;

            if (_timer >= Delay)
            {
                _currentRect.anchorMin = iTween.Vector2Update(_currentRect.anchorMin, TargetRect.anchorMin, Speed);
                _currentRect.anchorMax = iTween.Vector2Update(_currentRect.anchorMax, TargetRect.anchorMax, Speed);
                _currentRect.offsetMin = iTween.Vector2Update(_currentRect.offsetMin, TargetRect.offsetMin, Speed);
                _currentRect.offsetMax = iTween.Vector2Update(_currentRect.offsetMax, TargetRect.offsetMax, Speed);
            }

            if (_currentRect.rect == TargetRect.rect)
            {
                _completed = true;
            }
        }
    }
}
