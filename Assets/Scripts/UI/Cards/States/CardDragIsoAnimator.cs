using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI.Cards.States
{
    internal class CardDragIsoAnimator : MonoBehaviour
    {
        [SerializeField] private CardStateAnimator _animator;

        [Space]

        [SerializeField] private float _lerpSpeed;

        [Header("Rotation")]

        [SerializeField] private RectTransform _cardRectTransform;
        [SerializeField] private Vector3 _targetRotation;
        private Vector3 _startRotation;

        [Header("Scaler Size")]

        [SerializeField] private RectTransform _scalerRectTransform;
        [SerializeField] private Vector2 _targetScalerSize;
        private Vector2 _startScalerSize;

        [Header("Icon Size")]

        [SerializeField] private RectTransform _iconRectTransform;
        [SerializeField] private Vector2 _targetIconSize;
        [SerializeField] private Vector2 _targetIconPosition;
        private Vector2 _startIconSize;
        private Vector2 _startIconPosition;

        [Header("Text Size")]

        [SerializeField] private RectTransform _textRectTransform;
        [SerializeField] private Vector2 _targetTextPosition;
        private Vector2 _startTextPosition;

        private void Start()
        {
            _startRotation = _cardRectTransform.rotation.eulerAngles;
            _startScalerSize = _scalerRectTransform.sizeDelta;
            _startIconSize = _iconRectTransform.sizeDelta;
            _startIconPosition = _iconRectTransform.anchoredPosition;
            _startTextPosition = _textRectTransform.anchoredPosition;

            Observable.EveryUpdate().Subscribe(x =>
            {
                var speed = _lerpSpeed * Time.deltaTime;

                if (!_animator.AnimationStates.DragState.IsIso)
                {
                    _cardRectTransform.rotation =
                        Quaternion.Lerp(_cardRectTransform.rotation, Quaternion.Euler(_startRotation), speed);

                    _scalerRectTransform.sizeDelta =
                        Vector2.Lerp(_scalerRectTransform.sizeDelta, _startScalerSize, speed);

                    _iconRectTransform.sizeDelta =
                        Vector2.Lerp(_iconRectTransform.sizeDelta, _startIconSize, speed);

                    _iconRectTransform.anchoredPosition =
                        Vector2.Lerp(_iconRectTransform.anchoredPosition, _startIconPosition, speed);

                    _textRectTransform.anchoredPosition =
                        Vector2.Lerp(_textRectTransform.anchoredPosition, _startTextPosition, speed);
                }
                else
                {
                    _cardRectTransform.rotation =
                        Quaternion.Lerp(_cardRectTransform.rotation, Quaternion.Euler(_targetRotation), speed);

                    _scalerRectTransform.sizeDelta =
                        Vector2.Lerp(_scalerRectTransform.sizeDelta, _targetScalerSize, speed);

                    _iconRectTransform.sizeDelta =
                        Vector2.Lerp(_iconRectTransform.sizeDelta, _targetIconSize, speed);

                    _iconRectTransform.anchoredPosition =
                        Vector2.Lerp(_iconRectTransform.anchoredPosition, _targetIconPosition, speed);

                    _textRectTransform.anchoredPosition =
                        Vector2.Lerp(_textRectTransform.anchoredPosition, _targetTextPosition, speed);
                }

            }).AddTo(this);
        }
    }
}
