using Assets.Scripts.UI.Selectables;
using Assets.Scripts.Utils;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Building
{
    internal class BuildCell : MonoBehaviour, ISelectable
    {
        [Header("Animation")]

        [SerializeField] private Transform _graphicTransform;

        private Vector3 _startPosition;
        [SerializeField] private Vector3 _targetPosition;

        [SerializeField] private float _animationDuration;

        private bool _isSelected;

        private readonly SerialDisposable _serialDisposable = new();

        public void Deselect()
        {
            if (!_isSelected) return;

            _isSelected = false;

            _serialDisposable.Disposable?.Dispose();
            _serialDisposable.Disposable = Observable.FromMicroCoroutine(DeselectAnimation).Subscribe();
        }

        public bool IsSeleceted()
        {
            return _isSelected;
        }

        public void Select()
        {
            if (_isSelected) return;

            _isSelected = true;

            _serialDisposable.Disposable?.Dispose();
            _serialDisposable.Disposable = Observable.FromMicroCoroutine(SelectAnimation).Subscribe();
        }

        private void Start()
        {
            _startPosition = transform.position;
            _targetPosition += _startPosition;

            _serialDisposable.AddTo(this);
        }

        private IEnumerator SelectAnimation()
        {
            var start = transform.position;

            float time = 0.0f;
            while (time < _animationDuration)
            {
                transform.position =
                    LinearAnimation.EaseInOut(start, _targetPosition, time, _animationDuration);

                yield return null;
                time += Time.deltaTime;
            }

            transform.position = _targetPosition;
        }

        private IEnumerator DeselectAnimation()
        {
            var start = transform.position;

            float time = 0.0f;
            while (time < _animationDuration)
            {
                transform.position =
                    LinearAnimation.EaseInOut(start, _startPosition, time, _animationDuration);

                yield return null;
                time += Time.deltaTime;
            }

            transform.position = _startPosition;
        }
    }
}
