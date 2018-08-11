using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MHLab.Games.Matching
{
    public class BorderBall : MonoBehaviour
    {
        public List<Vector3> Points;
        public float Speed = 3f;
        private int _currentIndex = 0;
        private Vector3 _previousPosition;
        private bool _firstTime = true;

        protected void Update()
        {
            if (Points == null || Points.Count == 0) return;

            if (_firstTime)
            {
                _previousPosition = Points[0];
                _currentIndex++;
                _firstTime = false;
            }

            transform.position = Vector3.MoveTowards(transform.position, Points[_currentIndex], Time.deltaTime * Speed);

            if (transform.position == Points[_currentIndex])
            {
                _previousPosition = Points[_currentIndex];
                if (_currentIndex >= Points.Count - 1)
                    _currentIndex = 0;
                else
                    _currentIndex++;
            }
        }
    }
}
