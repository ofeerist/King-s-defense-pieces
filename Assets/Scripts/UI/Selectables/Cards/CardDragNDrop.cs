using Assets.Scripts.Building;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Selectables.Cards
{
    internal class CardDragNDrop : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
    {
        [SerializeField] private BuildRaycaster _buildRaycaster;
        [SerializeField] private GraphicRaycaster _raycaster;

        [Space]

        [SerializeField] private GameObject _cardHolderGameObject;

        [SerializeField] private RectTransform _cardPrefabParent;
        [SerializeField] private RectTransform _gridHolder;
        [SerializeField] private RectTransform _cardHolder;

        [Space]

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardAnimator _cardAnimator;
        [SerializeField] private LayoutElement _layoutElement;

        [Space]

        [SerializeField] private float _lerpSpeed;

        public bool IsIso { get; set; }

        private readonly SerialDisposable _serialDisposable = new();
        private readonly SerialDisposable _parentDisposable = new();
        private Transform _targetPosition;

        private BuildCell _currentCellSelectable;
        private readonly List<RaycastResult> _results = new();

        [SerializeField] private InputAction _mousePosition;

        private void Start()
        {
            _serialDisposable.AddTo(this);
            _parentDisposable.AddTo(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (IsRaycastCardHolder(eventData.position))
            {
                _cardAnimator.IsHand = true;
                _cardPrefabParent.SetParent(_cardHolder, false);
                _parentDisposable.Disposable = Observable.NextFrame().Subscribe(x =>
                {
                    _rectTransform.position = eventData.position;
                });
                _rectTransform.position = eventData.position;

                var distance = _cardPrefabParent.position.x - eventData.position.x;

                var index = _cardPrefabParent.GetSiblingIndex();
                if (distance > 0 && distance > _cardPrefabParent.sizeDelta.x / 2)
                {
                    if (index - 1 < 0) return;

                    _cardPrefabParent.SetSiblingIndex(index - 1);

                    _parentDisposable.Disposable = Observable.NextFrame().Subscribe(x =>
                    {
                        _rectTransform.position = eventData.position; 
                    });
                    _rectTransform.position = eventData.position;
                }

                if (distance < 0 && distance < -_cardPrefabParent.sizeDelta.x / 2)
                {
                    if (index + 1 >= _cardHolder.childCount) return;

                    _cardPrefabParent.SetSiblingIndex(_cardPrefabParent.GetSiblingIndex() + 1);

                    _parentDisposable.Disposable = Observable.NextFrame().Subscribe(x =>
                    {
                        _rectTransform.position = eventData.position;
                    });
                    _rectTransform.position = eventData.position;
                }

                return;
            }

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
            if (_cardAnimator.IsHand)
            {
                IsIso = false;
                Deselect(_currentCellSelectable);

                _rectTransform.anchoredPosition = Vector2.zero;

                return;
            }

            if (IsIso)
            {
                _serialDisposable.Disposable.Dispose();

                _cardPrefabParent.SetParent(_gridHolder, true);

                _cardAnimator.IsHand = false;

                _rectTransform.position = _buildRaycaster.CameraWorldToScreenPoint(_targetPosition.position);
            }
            else
            {
                _serialDisposable.Disposable.Dispose();

                _cardPrefabParent.SetParent(_cardHolder, true);

                _cardAnimator.IsHand = true;
                Deselect(_currentCellSelectable);

                _rectTransform.anchoredPosition = Vector2.zero;
            }
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

        public bool IsRaycastCardHolder(Vector2 position)
        {
            _results.Clear();

            var pointerData = new PointerEventData(EventSystem.current)
            {
                position = position
            };

            _raycaster.Raycast(pointerData, _results);

            foreach (RaycastResult result in _results)
            {
                var resultObject = result.gameObject;

                if (resultObject == _cardHolderGameObject) return true;
            }

            return false;
        }
    }
}
