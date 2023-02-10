using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI.Selectables.Cards
{
    internal class CardAnimator : MonoBehaviour, ISelectable
    {
        [SerializeField] private RectTransform _rectToExpand;
        [SerializeField] private RectTransform _rectToScale;

        [SerializeField] private float _targetMultiplier;
        [SerializeField] private float _lerpSpeed;

        private Vector2 _startSize;
        private Vector2 _targetSize;

        private Vector3 _startScale;
        private Vector3 _targetScale;

        private bool _isSelected;

        private void Start()
        {
            _startSize = _rectToExpand.sizeDelta;
            _targetSize = _startSize * _targetMultiplier;

            _startScale = _rectToScale.localScale;
            _targetScale = _startScale * _targetMultiplier;

            Observable.EveryUpdate().Subscribe(x =>
            {
                if (_isSelected)
                {
                    _rectToExpand.sizeDelta = 
                        Vector2.Lerp(_rectToExpand.sizeDelta, _targetSize, _lerpSpeed * Time.deltaTime);

                    _rectToScale.localScale =
                        Vector3.Lerp(_rectToScale.localScale, _targetScale, _lerpSpeed * Time.deltaTime);
                }
                else
                {
                    _rectToExpand.sizeDelta =
                        Vector2.Lerp(_rectToExpand.sizeDelta, _startSize, _lerpSpeed * Time.deltaTime);

                    _rectToScale.localScale =
                        Vector3.Lerp(_rectToScale.localScale, _startScale, _lerpSpeed * Time.deltaTime);
                }
            }).AddTo(this);
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
