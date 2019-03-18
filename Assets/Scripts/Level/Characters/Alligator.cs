﻿using System.Collections;
using UnityEngine;

namespace Scripts.Level
{
    public class Alligator : MonoBehaviour
    {

        private void Start()
        {
            transform.position += new Vector3(0, 0, 0.5f);
            StartCoroutine(Siege(1f));
        }

        private IEnumerator Siege(float moveDelay)
        {
            yield return new WaitForSeconds(moveDelay);
            transform.position -= new Vector3(0, 0, 0.5f);
            yield return new WaitForSeconds(moveDelay);
            transform.position += new Vector3(0, 0, 0.5f);
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