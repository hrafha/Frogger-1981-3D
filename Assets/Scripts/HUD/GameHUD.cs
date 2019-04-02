using UnityEngine;
using UnityEngine.UI;
using Scripts.Controllers;

namespace Scripts.HUD
{
    public class GameHUD : MonoBehaviour
    {

        public static GameHUD Instance;

        [Header("Menus")]
        [SerializeField] private GameObject gameOverMenu;

        [Header("InGameHUD")]
        [SerializeField] private Image timer;
        [SerializeField] private Text score;
        [SerializeField] private Text lives;

        private GameController gameController;

        private void Start()
        {
            Instance = this;
            gameController = FindObjectOfType<GameController>();
            UpdateTexts();
        }

        private void Update()
        {
            UpdateTimerImage();
        }

        public void UpdateTexts()
        {
            score.text = "Score: " + ScoreController.score.ToString();
            if (gameController.extraFrogs >= 0)
                lives.text = "Extra Lives: " + gameController.extraFrogs.ToString();
            else
                lives.text = "Extra Lives: --";
        }

        private void UpdateTimerImage()
        {
            timer.fillAmount = SetTimer((int)gameController.timeLeft, 0, gameController.defaultTime, 0, 1);
        }

        private float SetTimer(float time, float inMin, float inMax, float outMin, float outMax)
        {
            return (time - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }

        public void ShowGameOverMenu()
        {
            gameOverMenu.SetActive(true);
        }

    }
}