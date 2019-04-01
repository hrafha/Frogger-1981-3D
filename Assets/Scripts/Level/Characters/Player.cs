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

        private Rigidbody rigbd;
        private GameController gameController;
        private LevelController levelController;
        private ScoreController scoreController;

        private Vector3 moveVec;
        private Vector3 startPosition;

        private GameObject riverPlatform;

        private void Start()
        {
            rigbd = GetComponent<Rigidbody>();
            gameController = FindObjectOfType<GameController>();
            levelController = FindObjectOfType<LevelController>();
            scoreController = FindObjectOfType<ScoreController>();

            startPosition = transform.position;
        }

        private void Update()
        {
            if (!isMoving)
                UpdateMoveVec();

            if (CanMove())
                StartCoroutine(Move());

            if (PlayerMovedOutScreen())
                gameController.GameOver();
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
            CheckCollisions();
            while (transform.position != moveVec)
            {
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
                    Debug.Log(collider.tag);
                    collide = true;
                    CheckOtherCollider(collider);
                    break;
                }
            }
            if (collide == false)
                riverPlatform = null;
        }

        private void CheckOtherCollider(Collider other)
        {
            if (other.CompareTag("Platform"))
            {
                if (riverPlatform != other.gameObject)
                    riverPlatform = other.gameObject;
                rigbd.velocity = GetRiverPlatformRigidbody().velocity;
            }
            if (other.GetComponent<HomeSpot>())
            {
                StartCoroutine(ResetPosition());
                if (!other.GetComponent<HomeSpot>().filled)
                {
                    rigbd.velocity = Vector3.zero;
                    other.GetComponent<HomeSpot>().FillSpot(true);
                    ScoreController.IncreaseScore(ScoreController.ScoreType.Home);
                    ScoreController.IncreaseScore(ScoreController.ScoreType.Time);
                    levelController.CheckSpots();
                }
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

        private bool PlayerMovedOutScreen()
        {
            return transform.position.x > 15f
                || transform.position.x < -15f
                || transform.position.z < -14.5f
                || transform.position.z > -0.5f;
        }

        public IEnumerator ResetPosition()
        {
            rigbd.velocity = Vector3.zero;
            while (isMoving)
                yield return new WaitForEndOfFrame();
            transform.position = startPosition;
            gameController.ResetTimer();
            isMoving = true;
            yield return new WaitForSeconds(moveDelay);
            isMoving = false;
            hitted = false;
            ScoreController.ResetScoreLines();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle") && !hitted)
            {
                hitted = true;
                gameController.GameOver();
            }
        }

    }
}
