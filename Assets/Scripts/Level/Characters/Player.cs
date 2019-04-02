using System.Collections;
using UnityEngine;
using Scripts.Controllers;

namespace Scripts.Level
{
    public class Player : Frog
    {

        private GameController gameController;
        private LevelController levelController;
        private ScoreController scoreController;

        private Vector3 startPosition;

        private bool hitted;

        private void Start()
        {
            gameController = FindObjectOfType<GameController>();
            levelController = FindObjectOfType<LevelController>();
            scoreController = FindObjectOfType<ScoreController>();

            startPosition = transform.position;
        }

        private void Update()
        {
            if (!isMoving)
                UpdateMoveVec();
            else
                scoreController.CheckScoreLine(this);

            if (CanMove() && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
                StartCoroutine(Move());

            if (MovedOutScreen())
                gameController.GameOver();
        }

        protected override void UpdateMoveVec()
        {
            // Moves prioritizing horizontal 
            if (Input.GetAxisRaw("Horizontal") != 0)
                moveVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0) + transform.position;
            else if (Input.GetAxisRaw("Vertical") != 0)
                moveVec = new Vector3(0, 0, Input.GetAxisRaw("Vertical")) + transform.position;
        }

        protected override void CheckOtherCollider(Collider other)
        {
            if (other.GetComponent<HomeSpot>())
            {
                rigbd.velocity = Vector3.zero;
                StartCoroutine(ResetPosition());
                if (!other.GetComponent<HomeSpot>().filled)
                {
                    other.GetComponent<HomeSpot>().FillSpot(true);
                    ScoreController.IncreaseScore(ScoreController.ScoreType.Home);
                    ScoreController.IncreaseScore(ScoreController.ScoreType.Time);
                    levelController.CheckSpots();
                }
            }
            base.CheckOtherCollider(other);
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
