using UnityEngine;

namespace Scripts.Level
{
    public class ModelSpawner : MonoBehaviour
    {

        [SerializeField] private GameObject[] models;

        private GameObject currentModel;
        private GameObject lastModel;

        private void Start()
        {
            currentModel = GetRandomModel();
            FirstSpawn();
        }

        private void Update()
        {
            SpawnUpdate();
        }

        private GameObject GetRandomModel()
        {
            return models[Random.Range(0, models.Length)];
        }

        private void FirstSpawn()
        {
            Vector3 spawnVec = new Vector3(0, transform.position.y, transform.position.z);
            currentModel = Instantiate(currentModel, spawnVec, Quaternion.identity);
        }

        private void SpawnUpdate()
        {
            if (currentModel && Mathf.Abs(Vector2.Distance(transform.position, currentModel.transform.position)) > Mathf.Abs(transform.position.x))
            {
                lastModel = currentModel;
                Spawn();
            }
            if (lastModel && Mathf.Abs(Vector2.Distance(transform.position, lastModel.transform.position)) > Mathf.Abs(transform.position.x * 2))
                Destroy(lastModel);
        }

        private void Spawn()
        {
            currentModel = Instantiate(currentModel, transform.position, Quaternion.identity);

            // Prevents spawn another player when spawn models children
            if (currentModel.gameObject.GetComponentInChildren<Player>())
                Destroy(currentModel.gameObject.GetComponentInChildren<Player>().gameObject);
        }

    }
}