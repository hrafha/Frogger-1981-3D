using System.Collections;
using UnityEngine;

namespace Scripts.Level
{
    public class Alligator : MonoBehaviour
    {

        private Collider _collider;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false;

            transform.position += new Vector3(0, 0, 0.5f);
            StartCoroutine(Siege(1f));
        }

        private IEnumerator Siege(float moveDelay)
        {
            yield return new WaitForSeconds(moveDelay);
            transform.position -= new Vector3(0, 0, 0.5f);
            _collider.enabled = true;
            yield return new WaitForSeconds(moveDelay);
            transform.position += new Vector3(0, 0, 0.5f);
            _collider.enabled = false;
            Destroy(gameObject, moveDelay);
        }

        private void OnTriggerStay(Collider other)
        {
            // Self destroy on a filled home spot
            if (other.GetComponent<HomeSpot>() && other.GetComponent<HomeSpot>().filled)
                Destroy(gameObject);
        }

    }
}