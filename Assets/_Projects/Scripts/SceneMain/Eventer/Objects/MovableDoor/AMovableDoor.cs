using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Eventer.Objects
{
    public abstract class AMovableDoor<T> where T : AMovableDoor<T>
    {
#if UNITY_EDITOR
        [ReadOnly, ShowInInspector]
        private static readonly string ClassName = typeof(T).Name;
#endif

        [SerializeField, Required, SceneObjectsOnly, Tooltip("コライダー")]
        private Collider _collider;

        [SerializeField, Required, Tooltip("開く時の、ローカル変化量")]
        private Vector3 _delta;

        [SerializeField, Required, Tooltip("補間方法")]
        protected Ease _ease;

        [SerializeField, Range(0.1f, 20.0f), Tooltip("アニメーション時間")]
        protected float _duration;

        [SerializeField, Tooltip("trueなら1回限り、falseなら何回も動く")]
        private bool _isMoveOnce;

        private bool _hasPlayedIfMoveOnce = false;
        private bool _hasOpenedIfNotMoveOnce = false;
        private bool _isMoving = false;
        public bool IsMoving => _isMoving;

        public void Trigger()
        {
            if (_isMoving is true) return;
            if (_collider == null) return;

            if (_isMoveOnce)
            {
                if (_hasPlayedIfMoveOnce is true) return;
                UpdateColliderAndDoMove(true, _collider.GetCancellationTokenOnDestroy()).Forget();
                _hasPlayedIfMoveOnce = true;
            }
            else
            {
                UpdateColliderAndDoMove(!_hasOpenedIfNotMoveOnce, _collider.GetCancellationTokenOnDestroy()).Forget();
                Inverse(ref _hasOpenedIfNotMoveOnce);
            }

            static void Inverse(ref bool value) => value = !value;
            async UniTaskVoid UpdateColliderAndDoMove(bool isOpen, CancellationToken ct)
            {
                _isMoving = true;
                _collider.enabled = !isOpen; // 当たり判定とRayCast判定が同時に有効/無効化
                await DoMove(_collider.transform, isOpen ? _delta : -_delta, ct);
                _isMoving = false;
            }
        }

        protected abstract UniTask DoMove(Transform transform, Vector3 delta, CancellationToken ct);
    }
}
