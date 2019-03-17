using UnityEngine;

namespace Scripts.Level
{
    public class HomeSpot : MonoBehaviour
    {

        [SerializeField] private GameObject frog;

        public bool filled { get; private set; }

        public void FillSpot(bool isFilled)
        {
            filled = isFilled;
            frog.SetActive(isFilled);
            if (GetComponent<MeshRenderer>())
                GetComponent<MeshRenderer>().enabled = !isFilled;
        }

    }
}