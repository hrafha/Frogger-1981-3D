using UnityEngine;
using System.Collections;
using Scripts.Level;

namespace Scripts.Controllers
{
    public class LevelController : MonoBehaviour
    {

        [SerializeField] private GameObject[] spotSurprises;

        private GameController gameController;
        private HomeSpot[] spots;

        private void Start()
        {
            gameController = FindObjectOfType<GameController>();
            spots = FindObjectsOfType<HomeSpot>();

            SetSpotsStates();
            StartCoroutine(HomeSpotSurprise(6));
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
                SetSpotsStates();
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

        private IEnumerator HomeSpotSurprise(float delay)
        {
            yield return new WaitForSeconds(delay);
            Instantiate(RandomSuprise(), RandomAvailableHomeSpot().transform.position, Quaternion.identity);
            StartCoroutine(HomeSpotSurprise(delay));
        }

        private GameObject RandomSuprise()
        {
            return spotSurprises[Random.Range(0, spotSurprises.Length)];
        }

        private HomeSpot RandomAvailableHomeSpot()
        {
            HomeSpot randomSpot = spots[Random.Range(0, spots.Length)];
            while (randomSpot.filled)
                randomSpot = spots[Random.Range(0, spots.Length)];
            return randomSpot;
        }

    }
}