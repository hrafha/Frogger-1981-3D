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

        private LadyFrog ladyFrog;

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
                    ScoreController.IncreaseScore(ScoreType.Home);
                    ScoreController.IncreaseScore(ScoreType.Time);
                    CheckBonus(other);
                    levelController.CheckSpots();
                }
            }
            base.CheckOtherCollider(other);
        }

        private void CheckBonus(Collider other)
        {
            if (ladyFrog)
            {
                ladyFrog.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ladyFrog.transform.parent = null;
                ladyFrog.transform.position = other.transform.position + Vector3.up * 0.5f;
                ladyFrog.transform.localScale = Vector3.one - Vector3.up * 0.5f;
                ladyFrog.transform.parent = other.transform;
                Destroy(ladyFrog);
                ScoreController.IncreaseScore(ScoreType.Bonus);
            }
        }

        public IEnumerator ResetPosition()
        {
            rigbd.velocity = Vector3.zero;
            while (isMoving)
                yield return new WaitForEndOfFrame();
            transform.position = startPosition;
            gameController.ResetTimer();
            DestroyLadyFrog();
            isMoving = true;
            yield return new WaitForSeconds(moveDelay);
            isMoving = false;
            hitted = false;
            ScoreController.ResetScoreLines();
        }

        public void SetLadyFrog(LadyFrog ladyFrog)
        {
            this.ladyFrog = ladyFrog;
        }

        private void DestroyLadyFrog()
        {
            if (ladyFrog = FindObjectOfType<LadyFrog>())
                Destroy(ladyFrog.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle") && !hitted)
            {
                hitted = true;
                gameController.GameOver();
            }
            if (other.GetComponent<LadyFrog>())
                other.GetComponent<LadyFrog>().OnPlayerCollide(this);
        }

    }
}
