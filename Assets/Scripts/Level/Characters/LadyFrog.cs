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
                moveVec = other.ClosestPoint(moveVec);
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

    }
}