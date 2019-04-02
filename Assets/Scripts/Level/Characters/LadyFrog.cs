using System.Collections;
using UnityEngine;

namespace Scripts.Level
{
    public class LadyFrog : Frog
    {

        private MeshRenderer render;

        private void Start()
        {
            render = GetComponent<MeshRenderer>();
            render.material.color = Random.ColorHSV();

            StartCoroutine(MovementBehavior());
        }

        private void Update()
        {
            if (MovedOutScreen())
                Destroy(gameObject);
        }

        protected override void UpdateMoveVec()
        {
            bool toRight = Random.Range(0, 2) == 1;
            if (toRight)
                moveVec = Vector3.right + transform.position;
            else
                moveVec = Vector3.left + transform.position;
        }

        protected override void CheckOtherCollider(Collider other)
        {
            base.CheckOtherCollider(other);
            if (other.CompareTag("Platform"))
            {
                moveVec = other.ClosestPoint(moveVec);
                moveVec = FitInPlatform(other);
            }
        }

        private IEnumerator MovementBehavior()
        {
            UpdateMoveVec();
            if(CanMove())
                StartCoroutine(Move());
            if (!riverPlatform) // Dont move off the platform
                moveVec = transform.position;
            yield return new WaitUntil(CanMove);
            StartCoroutine(MovementBehavior());
        }

        private Vector3 FitInPlatform(Collider platform)
        {
            if (!platform.bounds.Contains(moveVec))
            {
                if (moveVec.x > platform.transform.position.x)
                    return moveVec + new Vector3(transform.localScale.x - moveVec.x, 0, 0);
                else if (moveVec.x < platform.transform.position.x)
                    return moveVec + new Vector3(transform.localScale.x + moveVec.x, 0, 0);
            }
            return moveVec;
        }

    }
}