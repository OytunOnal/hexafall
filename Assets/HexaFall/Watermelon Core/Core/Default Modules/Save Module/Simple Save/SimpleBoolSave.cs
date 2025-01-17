﻿using UnityEngine;

namespace HexFall
{
    [System.Serializable]
    public class SimpleBoolSave : ISaveObject
    {
        [SerializeField] bool value;
        public virtual bool Value
        {
            get => value;
            set => this.value = value;
        }

        public virtual void Flush() { }
    }
}