using Assets.Scripts.UI.Selectables;
using Assets.Scripts.Utils;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI.Cards
{
    public class CardsLayout : MonoBehaviour, ISelectable
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector3 _targetPosition;
        private Vector3 _startPosition;

        [SerializeField] private float _duration;

        private readonly SerialDisposable _serialDisposable = new();

        private bool _isSelected;

        private void Start()
        {
            _startPosition = _rectTransform.position;

            _serialDisposable.AddTo(this);
        }

        public void Deselect()
        {
            if (!_isSelected) return;

            _isSelected = false;
            _serialDisposable.Disposable = Observable.FromMicroCoroutine(Hover).Subscribe();
        }

        public bool IsSeleceted()
        {
            return _isSelected;
        }

        public void Select()
        {
            if (_isSelected) return;

            _isSelected = true;
            _serialDisposable.Disposable = Observable.FromMicroCoroutine(UnHover).Subscribe();
        }

        public IEnumerator Hover()
        {
            float time = 0.0f;
            while (time < _duration)
            {
                _rectTransform.position = 
                    LinearAnimation.EaseInOut(_rectTransform.position, _targetPosition, time, _duration);

                yield return null;
                time += Time.deltaTime;
            }

            _rectTransform.position = _targetPosition;
        }

        public IEnumerator UnHover()
        {
            float time = 0.0f;
            while (time < _duration)
            {
                _rectTransform.position =
                    LinearAnimation.EaseInOut(_rectTransform.position, _startPosition, time, _duration);

                yield return null;
                time += Time.deltaTime;
            }

            _rectTransform.position = _startPosition;
        }
    }
}
