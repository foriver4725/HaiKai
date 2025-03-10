using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Eventer.Objects
{
    [Serializable]
    public sealed class BlockingVolumeClass
    {
        [SerializeField, Required, SceneObjectsOnly]
        private Collider _collider;

        public bool IsEnabled
        {
            get
            {
                if (_collider == null) return false;
                return _collider.enabled;
            }
            set
            {
                if (_collider == null) return;
                _collider.enabled = value;
            }
        }
    }
}