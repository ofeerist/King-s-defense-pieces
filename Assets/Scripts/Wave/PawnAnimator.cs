using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Wave
{
    internal class PawnAnimator : MonoBehaviour
    {
        [SerializeField] private float _height;
        [SerializeField] private AnimationCurve _horizontal;

        private PawnFactory _factory;
        private Vector3 _targetPosition;

        private readonly SerialDisposable _serial = new();

        public int KeyID { get; private set; }
        public event Action<PawnAnimator> OnAnimationFinished;

        public void SetFactory(PawnFactory factory)
        {
            _factory = factory;
        }
            
        public void Move(Vector3 position, int id)
        {
            _targetPosition = position;
            KeyID = id;

            var _transform = transform;

            //_transform.rotation = Quaternion.LookRotation(position - _transform.position);
            
            _serial.Disposable = Observable.FromMicroCoroutine(AnimationLoop).Subscribe(x =>
            {
                OnAnimationFinished?.Invoke(this);
            });
        }

        private IEnumerator AnimationLoop()
        {
            var _transform = transform;
            var startPosition = _transform.position;
            var distance = Vector3.Distance(new Vector3(startPosition.x, _targetPosition.y, startPosition.z), _targetPosition);

            var height = startPosition.y;

            var progress = 0f;

            while (progress != 1)
            {
                var originPosition = _transform.position;
                var speed = _factory.Speed * Time.deltaTime;

                progress = 1 - Vector3.Distance(new Vector3(originPosition.x, _targetPosition.y, originPosition.z), _targetPosition) / distance;

                _transform.position = 
                    new Vector3(Mathf.MoveTowards(originPosition.x, _targetPosition.x, speed),
                                height + _horizontal.Evaluate(progress) * _height,
                                Mathf.MoveTowards(originPosition.z, _targetPosition.z, speed));

                height = Mathf.MoveTowards(height, _targetPosition.y, speed);

                yield return null;
            }
        }
    }

}
