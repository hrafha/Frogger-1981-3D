using UnityEngine;
using Scripts.Level;

namespace Scripts.Controllers
{
    public class LevelController : MonoBehaviour
    {

        private GameController gameController;
        private HomeSpot[] spots;

        private void Start()
        {
            gameController = FindObjectOfType<GameController>();
            spots = FindObjectsOfType<HomeSpot>();

            SetSpotsStates();
        }

        private void SetSpotsStates()
        {
            for (int i = 0; i < spots.Length; i++)
                spots[i].FillSpot(GameController.homeSpots[i]);
        }

        public void CheckSpots()
        {
            if (AllSpotsAreFilled())
            {
                gameController.FinishLevel();
                // Implement "LoadNewLevel()" here.
            }
            else
                GetSpotsStates();
        }

        private bool AllSpotsAreFilled()
        {
            int aux = 0;
            for (int i = 0; i < spots.Length; i++)
                if (spots[i].filled)
                    aux++;
            return aux == spots.Length;
        }

        private void GetSpotsStates()
        {
            for (int i = 0; i < spots.Length; i++)
                GameController.homeSpots[i] = spots[i].filled;
        }

    }
}