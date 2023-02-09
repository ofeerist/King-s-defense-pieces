using System;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Level
{
    internal class KeyPoint : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private Vector3 _cachedPosition;

        public Vector3 position { get
            {  
                if (_cachedPosition == Vector3.zero)
                {
                    _cachedPosition = transform.position;
                }

                return _cachedPosition;
            }  
        }


        public void PlayParticle()
        {
            var particle = Instantiate(_particleSystem, transform);

            particle.Play();

            Observable.Timer(TimeSpan.FromSeconds(particle.main.duration)).Subscribe(x =>
            {
                Destroy(particle.gameObject);
            }).AddTo(this);
        }
    }
}
