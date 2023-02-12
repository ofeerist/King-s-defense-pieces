using Assets.Scripts.Building;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Selectables.Cards
{
    internal class CardDragNDrop : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private BuildRaycaster _buildRaycaster;

        [Space]

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardAnimator _cardAnimator;
        [SerializeField] private LayoutElement _layoutElement;

        [Space]

        [SerializeField] private float _lerpSpeed;

        public bool IsIso { get; set; }

        private readonly SerialDisposable _serialDisposable = new();
        private Transform _targetPosition;

        private BuildCell _currentCellSelectable;

        private void Start()
        {
            _serialDisposable.AddTo(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _cardAnimator.IsHand = false;

            IsIso = _buildRaycaster.TryRaycastBuildCellPosition(eventData.position, 
                out var position, out var cell);

            Deselect(_currentCellSelectable);

            if (IsIso)
            {
                _targetPosition = cell.transform;

                Select(cell);
            }
            else
            {
                _rectTransform.position = eventData.position;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _serialDisposable.Disposable = Observable.EveryUpdate().Subscribe(x =>
            {
                if (!IsIso) return;

                _rectTransform.position = 
                    Vector3.Lerp(_rectTransform.position,
                    _buildRaycaster.CameraWorldToScreenPoint(_targetPosition.position), 
                    _lerpSpeed * Time.deltaTime);
            });
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _cardAnimator.IsHand = true;
            IsIso = false;
            Deselect(_currentCellSelectable);

            _rectTransform.anchoredPosition = Vector2.zero;
        }

        private void Select(BuildCell selectable)
        {
            if (selectable == null) return;

            selectable.Select();
            _currentCellSelectable = selectable;
        }

        private void Deselect(BuildCell selectable)
        {
            if (selectable == null) return;

            selectable.Deselect();
            _currentCellSelectable = null;
        }
    }
}
