using UnityEngine;
using System.Collections;
using Scripts.Level;

namespace Scripts.Controllers
{
    public class LevelController : MonoBehaviour
    {

        [Header("HomeSpot Settings")]
        [SerializeField] private GameObject[] spotSurprises;

        [Header("LadyFrog Settings")]
        [SerializeField] private GameObject ladyFrogPrefab;

        private GameController gameController;
        private HomeSpot[] spots;

        private void Start()
        {
            gameController = FindObjectOfType<GameController>();
            spots = FindObjectsOfType<HomeSpot>();

            SetSpotsStates();
            StartCoroutine(HomeSpotSurprise(6));
            StartCoroutine(LadyFrogSpawn(5));
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
                ScoreController.IncreaseScore(ScoreController.ScoreType.FiveHome);
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

        private IEnumerator LadyFrogSpawn(float delay)
        {
            LadyFrog ladyFrogSpawned = FindObjectOfType<LadyFrog>();
            Vector3 spawnPosition = RandomPositionOnLine(-5.5f);
            Collider platform = PlatformOnPosition(spawnPosition);
            Debug.Log(spawnPosition);
            if (!ladyFrogSpawned && platform)
                Instantiate(ladyFrogPrefab, platform.ClosestPoint(spawnPosition), Quaternion.identity);
            yield return new WaitForSeconds(delay);
            StartCoroutine(LadyFrogSpawn(delay));
        }

        private Vector3 RandomPositionOnLine(float line)
        {
            // Line == Z stage position
            return new Vector3(Random.Range(-13, 13), 1, line);
        }

        private Collider PlatformOnPosition(Vector3 position)
        {
            Collider[] collisions = Physics.OverlapBox(position, ladyFrogPrefab.transform.localScale / 2f);
            foreach (Collider collider in collisions)
            {
                if (collider.isTrigger && collider.CompareTag("Platform"))
                    return collider;
            }
            return null;
        }

    }
}