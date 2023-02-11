using Assets.Scripts.Utils;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Selectables.Cards
{
    internal class CardAnimator : MonoBehaviour, ISelectable
    {
        [SerializeField] private RectTransform _rectToExpand;
        [SerializeField] private RectTransform _rectToScale;

        [SerializeField] private LayoutElement _layoutElement;
        [SerializeField] private float _startAnimationDuration;

        [SerializeField] private float _targetMultiplier;
        [SerializeField] private float _lerpSpeed;

        private Vector2 _startSize;
        private Vector2 _targetSize;

        private Vector3 _startScale;
        private Vector3 _targetScale;

        private float _startScaleY;
        private float _targetScaleY;

        private bool _isSelected;

        private void Start()
        {
            _startSize = new Vector2(_layoutElement.preferredWidth, _layoutElement.preferredHeight);
            _targetSize = _startSize * _targetMultiplier;

            _startScale = _rectToScale.localScale;
            _targetScale = _startScale * _targetMultiplier;

            _startScaleY = 0;
            _targetScaleY = (_startSize.y * _targetMultiplier - _startSize.y) / 2f;

            Observable.FromMicroCoroutine(ShowSelf).Subscribe(x =>
            {
                Observable.EveryUpdate().Subscribe(x =>
                {
                    var speed = _lerpSpeed * Time.deltaTime;

                    if (_isSelected)
                    {
                        LerpLayoutSize(_layoutElement, _targetSize, speed);

                        _rectToScale.localScale =
                            Vector3.Lerp(_rectToScale.localScale, _targetScale, speed);

                        _rectToScale.anchoredPosition = new Vector2(_rectToScale.anchoredPosition.x,
                            Mathf.Lerp(_rectToScale.anchoredPosition.y, _targetScaleY, speed));
                    }
                    else
                    {
                        LerpLayoutSize(_layoutElement, _startSize, speed);

                        _rectToScale.localScale =
                            Vector3.Lerp(_rectToScale.localScale, _startScale, speed);

                        _rectToScale.anchoredPosition = new Vector2(_rectToScale.anchoredPosition.x,
                            Mathf.Lerp(_rectToScale.anchoredPosition.y, _startScaleY, speed));
                    }
                }).AddTo(this);
            }).AddTo(this);
        }

        private void LerpLayoutSize(LayoutElement element, Vector2 targetSize, float speed)
        {
            element.preferredHeight = Mathf.Lerp(element.preferredHeight, targetSize.y, speed);
            element.preferredWidth = Mathf.Lerp(element.preferredWidth, targetSize.x, speed);
        }

        private IEnumerator ShowSelf()
        {
            float startWidth = 0;
            float endWidth = _rectToExpand.rect.width;

            float startY = _rectToScale.anchoredPosition.y;
            float endY = 0;

            float time = 0.0f;
            while (time < _startAnimationDuration)
            {
                _layoutElement.preferredWidth = 
                    LinearAnimation.EaseInOut(startWidth, endWidth, time, _startAnimationDuration);

                _rectToScale.anchoredPosition = new Vector2(_rectToScale.anchoredPosition.x,
                    LinearAnimation.EaseInOut(startY, endY, time, _startAnimationDuration));

                yield return null;
                time += Time.deltaTime;
            }

            _rectToScale.anchoredPosition = new Vector2(_rectToScale.anchoredPosition.x, endY);
            _layoutElement.preferredWidth = endWidth;
        }

        public void Deselect()
        {
            _isSelected = false;
        }

        public void Select()
        {
            _isSelected = true;
        }

        public bool IsSeleceted()
        {
            return _isSelected;
        }
    }
}
