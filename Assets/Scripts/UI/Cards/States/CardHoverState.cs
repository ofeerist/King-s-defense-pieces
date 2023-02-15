using Assets.Scripts.Utils;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI.Cards.States
{
    [System.Serializable]
    public class CardHoverState : BaseHandCardState
    {
        private readonly SerialDisposable _serialDisposable = new();

        [SerializeField] private float _duration;
        [SerializeField] private float _scaleMultiplier;

        private Vector2 _startSize;
        private Vector3 _startScale;

        protected override void OnInitialize(CardStateAnimator animator)
        {
            base.OnInitialize(animator);

            var info = _animator.AnimationLinks;

            _startSize = new Vector2(info.LayoutLayoutElement.preferredWidth, info.LayoutLayoutElement.preferredHeight);
            _startScale = info.GraphicTransform.localScale;

            _serialDisposable.AddTo(animator);
        }

        protected override void OnEnter()
        {
            StartHover();
            LerpWidth = false;
        }
        protected override void OnExit()
        {
            StartUnHover();
            LerpWidth = true;
        }

        public void StartHover()
        {
            _serialDisposable.Disposable = Observable.FromMicroCoroutine(Hover).Subscribe();
        }

        public void StartUnHover()
        {
            _serialDisposable.Disposable = Observable.FromMicroCoroutine(UnHover).Subscribe();
        }

        public IEnumerator Hover()
        {
            var info = _animator.AnimationLinks;

            var targetSize = _startSize * _scaleMultiplier;
            var targetScale = _startScale * _scaleMultiplier;

            float time = 0.0f;
            while (time < _duration)
            {
                info.LayoutLayoutElement.preferredWidth =
                    LinearAnimation.EaseInOut(info.LayoutLayoutElement.preferredWidth, targetSize.x, time, _duration);
                info.LayoutLayoutElement.preferredHeight =
                    LinearAnimation.EaseInOut(info.LayoutLayoutElement.preferredHeight, targetSize.y, time, _duration);

                info.GraphicTransform.localScale =
                    LinearAnimation.EaseInOut(info.GraphicTransform.localScale, targetScale, time, _duration);

                yield return null;
                time += Time.deltaTime;
            }

            info.LayoutLayoutElement.preferredWidth = targetSize.x;
            info.LayoutLayoutElement.preferredHeight = targetSize.y;
            info.GraphicTransform.localScale = targetScale;
        }

        public IEnumerator UnHover()
        {
            var info = _animator.AnimationLinks;

            float time = 0.0f;
            while (time < _duration)
            {
                info.LayoutLayoutElement.preferredWidth =
                    LinearAnimation.EaseInOut(info.LayoutLayoutElement.preferredWidth, _startSize.x, time, _duration);
                info.LayoutLayoutElement.preferredHeight =
                    LinearAnimation.EaseInOut(info.LayoutLayoutElement.preferredHeight, _startSize.y, time, _duration);

                info.GraphicTransform.localScale =
                    LinearAnimation.EaseInOut(info.GraphicTransform.localScale, _startScale, time, _duration);

                yield return null;
                time += Time.deltaTime;
            }

            info.LayoutLayoutElement.preferredWidth = _startSize.x;
            info.LayoutLayoutElement.preferredHeight = _startSize.y;
            info.GraphicTransform.localScale = _startScale;
        }
    }
}
