using UnityEngine;

namespace Utilities
{
    public class Rotate : MonoBehaviour
    {
        public Transform pivot;
        public Vector3 axis = Vector3.up;
        public bool reverse = false;
        public bool unscaledTime = false;
        public float speed = 1f;

        private void Update()
        {
            var angle = speed
                * (reverse ? -1f : 1f)
                * (unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);

            var point = pivot;
            if (point == null)
            {
                point = transform;
            }
            
            transform.RotateAround(point.position, axis, angle);
        }
    }
}
