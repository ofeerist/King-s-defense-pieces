using UnityEngine;

namespace Assets.Scripts.Wave.Effects
{
    internal class KeyPointsParticlePlayer : MonoBehaviour
    {
        [SerializeField] private PawnFactory _pawnFactory;
        public void PlayAnimation(PawnAnimator pawn)
        {
            _pawnFactory.KeyPoints[pawn.KeyID].PlayParticle();
        }
    }
}
