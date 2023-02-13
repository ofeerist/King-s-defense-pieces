using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.UI.Cards.States
{
    [System.Serializable]
    public class CardDragState : BaseCardAnimationState
    {
        private readonly SerialDisposable _serialDisposable = new();

        protected CardStateAnimator _animator;

        [SerializeField] private float _lerpSpeed;
        [SerializeField] private InputAction _mousePositionAction;
        [SerializeField] private Camera _camera;

        private Vector2 _lastMousePosition;

        protected override void OnInitialize(CardStateAnimator animator)
        {
            _animator = animator;
            _serialDisposable.AddTo(_animator);

            _mousePositionAction.Enable();
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

                _animator.AnimationLinks.GraphicTransform.anchoredPosition += deltaPosition;

                _lastMousePosition = mousePosition;

                SiblingsCheck(_animator.AnimationLinks.GraphicTransform.position);
            });

            _animator.AnimationStates.HoverState.StartHover();
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

        protected override void OnExit()
        {
            _serialDisposable.Disposable.Dispose();

            _lastMousePosition = Vector2.zero;

            _animator.AnimationStates.HoverState.StartUnHover();
        }
    }
}
