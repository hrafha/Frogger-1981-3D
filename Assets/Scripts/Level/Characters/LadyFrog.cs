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
            if (other.CompareTag("Platform"))
            {
                moveVec = other.ClosestPoint(moveVec);
                moveVec = FitInPlatform(other);
            }
            base.CheckOtherCollider(other);
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

        public void OnPlayerCollide(Player player)
        {
            StopAllCoroutines();
            player.SetLadyFrog(this);
            transform.position = player.transform.position + Vector3.up * 0.5f;
            transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
            transform.parent = player.transform;
            StartCoroutine(ClimbOnPlayer(player));
        }

        private IEnumerator ClimbOnPlayer(Player player)
        {
            rigbd.velocity = player.GetComponent<Rigidbody>().velocity;
            yield return new WaitForFixedUpdate();
            StartCoroutine(ClimbOnPlayer(player));
        }

    }
}