using UnityEngine;

namespace Scripts.Level
{
    public class ObjMove : MonoBehaviour
    {

        public float baseSpeed;

        private void Update()
        {
            transform.position += Vector3.right * baseSpeed * Time.deltaTime;
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

    }
}