using UnityEngine;
using System.Collections;

namespace Scripts.Level
{
    public class Turtle : MonoBehaviour
    {
        
        [Header("Submerge Delays")]
        [SerializeField] private float timeToUp;
        [SerializeField] private float timeToDown;

        private float timer;
        private float defaultHeight;
        private float submerseHeight;

        private bool submerse;

        [Space]
        public float speed;

        private void Start()
        {
            SetSettings();

            // Will submerge
            if (speed > 0)
                StartCoroutine(SubmergeUpdate());
        }

        private void SetSettings()
        {
            if (timeToUp <= 0)
                timeToUp = 0.5f;
            if (timeToDown <= 0)
                timeToDown = 3f;
            defaultHeight = transform.position.y;
            submerseHeight = -0.5f;
        }

        private IEnumerator SubmergeUpdate()
        {
            timer += Time.deltaTime;
            if (!submerse)
            {
                if (transform.position.y > submerseHeight)
                    transform.position -= new Vector3(0, speed * Time.deltaTime, 0);

                if (timer > timeToUp)
                {
                    timer = 0;
                    submerse = true;
                }
            }
            else
            {
                if (transform.position.y < defaultHeight)
                    transform.position += new Vector3(0, speed * Time.deltaTime, 0);

                if (timer > timeToDown)
                {
                    timer = 0;
                    submerse = false;
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
            StartCoroutine(SubmergeUpdate());
        }

    }
}