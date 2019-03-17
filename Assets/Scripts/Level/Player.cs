using System.Collections;
using UnityEngine;
using Scripts.Controllers;

namespace Scripts.Level
{
    public class Player : MonoBehaviour
    {

        [Header("Movement Settings")]
        [SerializeField] private float stepRange;
        [SerializeField] private float moveDelay;

        private bool isMoving;

        private GameController gameController;

        private Vector3 moveVec;

        private void Start()
        {
            gameController = FindObjectOfType<GameController>();
        }

        private void Update()
        {
            if(!isMoving)
                UpdateMoveVec();
            if (CanMove())
                StartCoroutine(Move());
        }

        private void UpdateMoveVec()
        {
            // Moves prioritizing horizontal 
            if (Input.GetAxisRaw("Horizontal") != 0)
                moveVec = new Vector3(Input.GetAxisRaw("Horizontal") * stepRange, 0, 0) + transform.position;
            else if (Input.GetAxisRaw("Vertical") != 0)
                moveVec = new Vector3(0, 0, Input.GetAxisRaw("Vertical") * stepRange) + transform.position;
        }

        private bool CanMove()
        {
            bool pressingKey = (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);

            return pressingKey && !isMoving && ValidMovement();
        }

        private bool ValidMovement()
        {
            return moveVec.x < 14f
                && moveVec.x > -14f
                && moveVec.z > -14.5f
                && moveVec.z < -0.5f;
        }

        private IEnumerator Move()
        {
            isMoving = true;
            while (transform.position != moveVec)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveVec, 0.25f);
                yield return new WaitForEndOfFrame();
            }
            transform.position = moveVec;
            yield return new WaitForSeconds(moveDelay);
            isMoving = false;
        }

    }
}
