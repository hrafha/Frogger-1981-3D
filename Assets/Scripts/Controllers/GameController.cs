using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {

        [Header("Game Settings")]
        [SerializeField] private int defaultFrogs;
        [SerializeField] private float defaultTime;

        public int extraFrogs { get; private set; }
        public float timeLeft { get; private set; }

        public static bool[] homeSpots = new bool[5];

        private void Start()
        {
            ResetExtraFrogs();
            ResetTimer();
        }

        private void Update()
        {
            TimerUpdate();
        }

        private void ResetExtraFrogs()
        {
            extraFrogs = defaultFrogs;
        }

        public void ResetTimer()
        {
            timeLeft = defaultTime;
        }

        private void TimerUpdate()
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
                GameOver();
        }

        public void GameOver()
        {
            PauseGame(true);
            extraFrogs--;
            if (extraFrogs < 0)
                EndGame();
            else
                RestartLevel();
        }

        private void PauseGame(bool b)
        {
            if (b)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        }

        private void EndGame()
        {
            PauseGame(true);
            // Implement "ShowGameOverScreen()" here.
        }

        public void RestartLevel()
        {
            ResetTimer();
            PauseGame(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void FinishLevel()
        {
            ResetExtraFrogs();
            RestartLevel();

            // Clean out spots
            for (int i = 0; i < homeSpots.Length; i++)
                homeSpots[i] = false;
        }

        //DontDestroyOnLoad
        public static GameController control;
        void Awake()
        {
            if (control == null)
            {
                DontDestroyOnLoad(gameObject);
                control = this;
            }
            else if (control != this)
            {
                Destroy(gameObject);
            }
        }

    }
}