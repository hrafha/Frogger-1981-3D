using UnityEngine;

namespace Scripts.Level
{
    public class ModelSpawner : MonoBehaviour
    {

        [SerializeField] private GameObject[] models;

        private GameObject currentModel;

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
                Spawn();
        }

        private void Spawn()
        {
            currentModel = Instantiate(currentModel, transform.position, Quaternion.identity);
        }

    }
}