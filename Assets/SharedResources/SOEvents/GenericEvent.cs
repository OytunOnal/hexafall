using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SOEvents
{
    public abstract class GenericEvent<T> : ScriptableObject
    {
        private event Action<T> eventActions;

        public void RegisterListener(Action<T> listener)
        {
            eventActions += listener;
        }

        public void UnregisterListener(Action<T> listener)
        {
            eventActions -= listener;
        }

        public void Raise(T arg)
        {
            eventActions?.Invoke(arg);
        }

    }
}
