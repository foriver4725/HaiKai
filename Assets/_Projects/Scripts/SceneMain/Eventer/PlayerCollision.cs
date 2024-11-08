using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Main.Eventer
{
    [Serializable]
    public sealed class PlayerCollision
    {
        [SerializeField, Required, SceneObjectsOnly, LabelText("�v���C���[")]
        private Collider _playerCollider;

        // �ŏ��ɂ�����ĂԂ���
        public void Init(Action<Collider> onTriggerEnter)
        {
            if (_playerCollider == null) return;
            if (onTriggerEnter is null) return;

            PlayerCollider playerColliderComponent = _playerCollider.gameObject.AddComponent<PlayerCollider>();
            playerColliderComponent
                .GetAsyncTriggerEnterTrigger()
                .Subscribe(onTriggerEnter)
                .AddTo(playerColliderComponent.destroyCancellationToken);
        }

        private sealed class PlayerCollider : MonoBehaviour { }
    }
}
