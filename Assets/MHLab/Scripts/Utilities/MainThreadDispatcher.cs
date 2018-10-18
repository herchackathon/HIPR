using System;
using System.Collections.Generic;
using UnityEngine;

namespace MHLab.Utilities
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static readonly Queue<Action> Actions = new Queue<Action>();

        protected void Awake()
        {
            DontDestroyOnLoad(this);
        }

        protected void Update()
        {
            while (Actions.Count > 0)
            {
                var action = Actions.Dequeue();
                action.Invoke();
            }
        }

        public static void EnqueueAction(Action callback)
        {
            Actions.Enqueue(callback);
        }
    }
}
