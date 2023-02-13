using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI.Cards.States
{
    [System.Serializable]
    public class BaseHandCardState : BaseCardAnimationState
    {
        private readonly SerialDisposable _serialDisposable = new();
        protected CardStateAnimator _animator;

        [SerializeField] private float _lerpSpeed;

        protected override void OnInitialize(CardStateAnimator animator)
        {
            _animator = animator;
            _serialDisposable.AddTo(_animator);

            var layoutTransform = _animator.AnimationLinks.LayoutTransform;
            var graphicTransform = _animator.AnimationLinks.GraphicTransform;

            OnStateEnter += () =>
            {
                _serialDisposable.Disposable = Observable.EveryUpdate().Subscribe(x =>
                { 
                    graphicTransform.position = 
                        Vector3.Lerp(graphicTransform.position, layoutTransform.position, _lerpSpeed * Time.deltaTime);
                });
            };

            OnStateExit += () =>
            {
                _serialDisposable.Disposable.Dispose();
            };
        }
    }
}
