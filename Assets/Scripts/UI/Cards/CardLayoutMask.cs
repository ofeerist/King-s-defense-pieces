using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI.Cards
{
    internal class CardLayoutMask : MonoBehaviour
    {
        [SerializeField] private RectTransform _self;
        [SerializeField] private RectTransform _target;

        [SerializeField] private float _lerpSpeed;

        private void Start()
        {
            Observable.EveryUpdate().Subscribe(x =>
            {
                _self.sizeDelta = new Vector2(
                    Mathf.Lerp(_self.sizeDelta.x, _target.sizeDelta.x, _lerpSpeed * Time.deltaTime),
                    _self.sizeDelta.y);
            }).AddTo(this);
        }
    }
}
