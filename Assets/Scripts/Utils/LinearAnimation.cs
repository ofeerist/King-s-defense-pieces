using UnityEngine;

namespace Assets.Scripts.Utils
{
    internal class LinearAnimation
    {
        public static float EaseInOut(float initial, float final, float time, float duration)
        {
            float change = final - initial;
            time /= duration / 2;
            if (time < 1f) return change / 2 * time * time + initial;
            time--;
            return -change / 2 * (time * (time - 2) - 1) + initial;
        }

        public static Vector3 EaseInOut(Vector3 initial, Vector3 final, float time, float duration)
        {
            return new Vector3(
                EaseInOut(initial.x, final.x, time, duration),
                EaseInOut(initial.y, final.y, time, duration),
                EaseInOut(initial.z, final.z, time, duration));
        }
    }
}
