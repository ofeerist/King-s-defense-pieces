using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Cards.Events
{
    internal class CardDragEvent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private CardStateAnimator _animator;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _animator.ChangeState(_animator.AnimationStates.DragState);
        }

        public void OnDrag(PointerEventData eventData)
        {

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_animator.CurrentState != _animator.AnimationStates.DragState) return;

            _animator.ChangeState(_animator.AnimationStates.StillState);
        }
    }
}
