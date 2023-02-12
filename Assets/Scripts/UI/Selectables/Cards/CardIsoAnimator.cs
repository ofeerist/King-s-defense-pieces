using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI.Selectables.Cards
{
    internal class CardIsoAnimator : MonoBehaviour
    {
        [SerializeField] private CardDragNDrop _cardDragNDrop;
        [SerializeField] private CardAnimator _cardAnimator;

        [SerializeField] private float _lerpSpeed;

        [Header("Rotation")]

        [SerializeField] private RectTransform _cardRectTransform;
        [SerializeField] private Vector3 _targetRotation;
        [SerializeField] private Vector3 _targetPosition;
        private Vector3 _startRotation;
        private Vector3 _startPosition;

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
            _cardAnimator.OnShowSelf.AddListener(() =>
            {
                Main();
            });
        }

        private void Main() 
        {
            _startRotation = _cardRectTransform.rotation.eulerAngles;
            _startPosition = _scalerRectTransform.anchoredPosition;
            _startScalerSize = _scalerRectTransform.sizeDelta;
            _startIconSize = _iconRectTransform.sizeDelta;
            _startIconPosition = _iconRectTransform.anchoredPosition;
            _startTextPosition = _textRectTransform.anchoredPosition;

            Observable.EveryUpdate().Subscribe(x =>
            {
                var speed = _lerpSpeed * Time.deltaTime;

                if (!_cardDragNDrop.IsIso)
                {
                    _cardRectTransform.rotation =
                        Quaternion.Lerp(_cardRectTransform.rotation, Quaternion.Euler(_startRotation), speed);

                    _scalerRectTransform.anchoredPosition = new Vector2(
                        Mathf.Lerp(_scalerRectTransform.anchoredPosition.x, _startPosition.x, speed),
                        _scalerRectTransform.anchoredPosition.y);

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

                    _scalerRectTransform.anchoredPosition =
                        Vector2.Lerp(_scalerRectTransform.anchoredPosition, _targetPosition, speed);

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
