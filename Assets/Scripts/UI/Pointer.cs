using UnityEngine;

namespace Richie.GameProject
{
    public class Pointer : MonoBehaviour
    {
        private void Start() => gameObject.SetActive(false);

        public void SetActive(bool toggle)
        {
            if (toggle)
            {
                gameObject.SetActive(true);
            }
            else gameObject.SetActive(false);
        }

        private void OnDisable() => gameObject.SetActive(false);
    }
}