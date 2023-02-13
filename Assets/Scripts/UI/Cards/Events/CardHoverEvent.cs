using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Cards.Events
{
    internal class CardHoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
    {
        [SerializeField] private CardStateAnimator _animator;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_animator.CurrentState != _animator.AnimationStates.StillState) return;

            _animator.ChangeState(_animator.AnimationStates.HoverState);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_animator.CurrentState != _animator.AnimationStates.HoverState) return;

            _animator.ChangeState(_animator.AnimationStates.StillState);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (_animator.CurrentState != _animator.AnimationStates.StillState) return;

            _animator.ChangeState(_animator.AnimationStates.HoverState);
        }
    }
}
