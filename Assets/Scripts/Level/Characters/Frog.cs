using System.Collections;
using UnityEngine;

namespace Scripts.Level
{
    public abstract class Frog : MonoBehaviour
    {

        [Header("Movement Settings")]
        [SerializeField] [Range(0.1f, 0.2f)] protected float moveDelay = 0.15f;

        protected bool isMoving;

        protected Rigidbody rigbd;
        protected Vector3 moveVec;
        protected GameObject riverPlatform;

        protected void Awake()
        {
            rigbd = GetComponent<Rigidbody>();
        }

        protected abstract void UpdateMoveVec();

        protected bool CanMove()
        {
            return !isMoving && ValidMovement();
        }

        private bool ValidMovement()
        {
            return moveVec.x < 14f
                && moveVec.x > -14f
                && moveVec.z > -14.5f
                && moveVec.z < -0.5f;
        }

        protected IEnumerator Move()
        {
            isMoving = true;
            CheckCollisions();
            while (transform.position != moveVec)
            {
                if (riverPlatform)
                    moveVec += GetRiverPlatformRigidbody().velocity * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
                transform.position = Vector3.MoveTowards(transform.position, moveVec, 0.25f);
            }
            yield return new WaitForSeconds(moveDelay);
            isMoving = false;
        }

        private void CheckCollisions()
        {
            bool collide = false;
            Collider[] collisions = Physics.OverlapBox(moveVec, transform.localScale / 2f);
            foreach (Collider collider in collisions)
            {
                if (collider.isTrigger)
                {
                    collide = true;
                    CheckOtherCollider(collider);
                    break;
                }
            }
            if (collide == false)
                riverPlatform = null;
        }

        protected virtual void CheckOtherCollider(Collider other)
        {
            if (other.CompareTag("Platform"))
            {
                if (riverPlatform != other.gameObject)
                    riverPlatform = other.gameObject;
                rigbd.velocity = GetRiverPlatformRigidbody().velocity;
            }
        }

        private Rigidbody GetRiverPlatformRigidbody()
        {
            if (riverPlatform.GetComponentInParent<Rigidbody>())
                return riverPlatform.GetComponentInParent<Rigidbody>();
            else if (riverPlatform.GetComponent<Rigidbody>())
                return riverPlatform.GetComponent<Rigidbody>();
            else
                return null;
        }

        protected bool MovedOutScreen()
        {
            return transform.position.x > 15f
                || transform.position.x < -15f
                || transform.position.z < -14.5f
                || transform.position.z > -0.5f;
        }

    }
}