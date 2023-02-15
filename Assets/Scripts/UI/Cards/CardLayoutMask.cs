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
                _self.sizeDelta = Vector3.Lerp(_self.sizeDelta, _target.sizeDelta,
                    _lerpSpeed * Time.deltaTime);

                _self.position = Vector3.Lerp(_self.position, _target.position, 
                    _lerpSpeed * Time.deltaTime);
            }).AddTo(this);
        }
    }
}
