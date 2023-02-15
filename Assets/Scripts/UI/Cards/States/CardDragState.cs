using Assets.Scripts.Building;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Cards.States
{
    [System.Serializable]
    public class CardDragState : BaseCardAnimationState
    {
        [SerializeField] private BuildRaycaster _buildRaycaster;
        [SerializeField] private GraphicRaycaster _raycaster;

        [SerializeField] private Vector3 _isoOffset;

        [Space]

        private readonly SerialDisposable _serialDisposable = new();

        protected CardStateAnimator _animator;

        [System.Serializable]
        public class AnimationStates
        {
            public BaseHandCardState StillState;
            public CardHoverState HoverState;
            public CardDragState DragState;
            public void InitializeStates(CardStateAnimator animator)
            {
                StillState.Initialize(animator);
                HoverState.Initialize(animator);
                DragState.Initialize(animator);
            }
        }

        [SerializeField] private float _lerpSpeed;
        [SerializeField] private InputAction _mousePositionAction;
        [SerializeField] private Camera _camera;

        public bool IsIso { get; private set; }

        private Vector2 _lastMousePosition;
        private BuildCell _currentCellSelectable;
        private float _startWidth;
        private readonly List<RaycastResult> _results = new();

        protected override void OnInitialize(CardStateAnimator animator)
        {
            _animator = animator;
            _serialDisposable.AddTo(_animator);

            _mousePositionAction.Enable();

            _startWidth = _animator.AnimationLinks.LayoutLayoutElement.preferredWidth;
        }

        protected override void OnEnter()
        {
            _animator.AnimationLinks.GraphicTransform.SetSiblingIndex(99);

            var mousePosition = _mousePositionAction.ReadValue<Vector2>();
            _lastMousePosition = mousePosition;

            _serialDisposable.Disposable = Observable.EveryUpdate().Subscribe(x =>
            {
                mousePosition = _mousePositionAction.ReadValue<Vector2>();
                var deltaPosition = mousePosition - _lastMousePosition;
                deltaPosition = new Vector2(deltaPosition.x * (1920f / Screen.width),
                    deltaPosition.y * (1080f / Screen.height));

                IsIso = _buildRaycaster.TryRaycastBuildCellPosition(mousePosition,
                        out var position, out var cell) && !IsRaycastCardHolder(mousePosition);

                if (cell != _currentCellSelectable)
                    Deselect(_currentCellSelectable);

                if (!IsRaycastCardHolder(mousePosition))
                {
                    _animator.AnimationLinks.CardsLayoutSelectable.Deselect();

                    if (IsIso)
                    {
                        Select(cell);

                        _animator.AnimationLinks.GraphicTransform.position =
                        Vector3.Lerp(_animator.AnimationLinks.GraphicTransform.position,
                            _buildRaycaster.CameraWorldToScreenPoint(cell.transform.position) + _isoOffset,
                            _lerpSpeed * Time.deltaTime);
                    }
                    else
                    {
                        _animator.AnimationLinks.GraphicTransform.anchoredPosition += deltaPosition;
                    }

                    _animator.AnimationLinks.LayoutLayoutElement.preferredWidth =
                            Mathf.Lerp(_animator.AnimationLinks.LayoutLayoutElement.preferredWidth, 0, _lerpSpeed * Time.deltaTime);
                }
                else
                {
                    _animator.AnimationLinks.CardsLayoutSelectable.Select();

                    _animator.AnimationLinks.LayoutLayoutElement.preferredWidth =
                            Mathf.Lerp(_animator.AnimationLinks.LayoutLayoutElement.preferredWidth, _startWidth, _lerpSpeed * Time.deltaTime);

                    _animator.AnimationLinks.GraphicTransform.anchoredPosition += deltaPosition;

                    SiblingsCheck(_animator.AnimationLinks.GraphicTransform.position);
                }

                _lastMousePosition = mousePosition;
            });
        }

        private void SiblingsCheck(Vector2 position)
        {
            var _cardPrefabParent = _animator.AnimationLinks.LayoutTransform;
            var distance = _cardPrefabParent.position.x - position.x;

            var index = _cardPrefabParent.GetSiblingIndex();
            if (distance > 0 && distance > _cardPrefabParent.sizeDelta.x / 2)
            {
                if (index - 1 < 0) return;

                _cardPrefabParent.SetSiblingIndex(index - 1);
            }

            if (distance < 0 && distance < -_cardPrefabParent.sizeDelta.x / 2)
            {
                if (index + 1 >= _animator.AnimationLinks.CardsLayout.childCount) return;

                _cardPrefabParent.SetSiblingIndex(_cardPrefabParent.GetSiblingIndex() + 1);
            }

            return;
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

                if (resultObject == _animator.AnimationLinks.CardsLayoutMask) return true;
            }

            return false;
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

        protected override void OnExit()
        {
            _animator.AnimationLinks.CardsLayoutSelectable.Select();

            Deselect(_currentCellSelectable);

            _serialDisposable.Disposable.Dispose();

            IsIso = false;
            _lastMousePosition = Vector2.zero;
        }
    }
}
