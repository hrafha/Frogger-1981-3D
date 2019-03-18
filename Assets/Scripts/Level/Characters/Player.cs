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
        private bool hitted;
        private bool delivered;

        private Rigidbody rigbd;
        private GameController gameController;
        private LevelController levelController;

        private Vector3 moveVec;
        private Vector3 startPosition;

        private GameObject riverPlatform;

        private void Start()
        {
            rigbd = GetComponent<Rigidbody>();
            gameController = FindObjectOfType<GameController>();
            levelController = FindObjectOfType<LevelController>();

            startPosition = transform.position;
        }

        private void Update()
        {
            if (!isMoving)
                UpdateMoveVec();
            if (CanMove())
                StartCoroutine(Move());

            MoveOnRiverPlatform();
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
            transform.parent = null;
            while (transform.position != moveVec)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveVec, 0.25f);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(moveDelay);
            //transform.position = moveVec;
            isMoving = false;
        }

        private void MoveOnRiverPlatform()
        {
            if (riverPlatform)
                transform.parent = riverPlatform.transform;
            else
                transform.parent = null;
        }

        public IEnumerator ResetPosition()
        {
            transform.parent = null;
            while (isMoving)
                yield return new WaitForEndOfFrame();
            transform.position = startPosition;
            gameController.ResetTimer();
            isMoving = true;
            yield return new WaitForSeconds(moveDelay);
            isMoving = false;
            hitted = false;
            delivered = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Platform"))
            {
                riverPlatform = other.gameObject;
                rigbd.velocity = Vector3.zero;
            }
            if (other.CompareTag("Obstacle") && !delivered)
            {
                hitted = true;
                StartCoroutine(ResetPosition());
                gameController.GameOver();
            }
            else if (other.GetComponent<HomeSpot>() && !hitted)
            {
                delivered = true;
                rigbd.velocity = Vector3.zero;
                StartCoroutine(ResetPosition());
                other.GetComponent<HomeSpot>().FillSpot(true);
                levelController.CheckSpots();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == riverPlatform)
                riverPlatform = null;
        }

        private void OnBecameInvisible()
        {
            gameController.GameOver();
        }

    }
}
