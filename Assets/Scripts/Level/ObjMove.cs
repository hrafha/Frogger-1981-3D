using UnityEngine;

namespace Scripts.Level
{
    [RequireComponent(typeof(Rigidbody))]
    public class ObjMove : MonoBehaviour
    {

        private Rigidbody rigbd;

        public float baseSpeed;

        private void Start()
        {
            rigbd = GetComponent<Rigidbody>();
        }

        public void Update()
        {
            rigbd.velocity = Vector3.right * baseSpeed;
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

    }
}