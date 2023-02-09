using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Wave
{
    internal class WaveTicker : MonoBehaviour
    {
        [SerializeField] private float _tickTime;

        private int _tickCount;
        public int TickCount => _tickCount;

        public UnityEvent<int> OnTick;

        private void Start()
        {
            Observable.Interval(TimeSpan.FromSeconds(_tickTime)).Subscribe(x =>
            {
                OnTick?.Invoke(_tickCount);

                _tickCount++;
            }).AddTo(this);
        }
    }
}
