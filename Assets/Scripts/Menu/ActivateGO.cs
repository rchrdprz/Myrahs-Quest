using UnityEngine;

namespace Richie.GameProject
{
    public class ActivateGO : MonoBehaviour
    {
        [SerializeField] private GameObject _object;

        public void SetActive()
        {
            if (_object.activeInHierarchy)
            {
                _object.SetActive(false);
            }
            else _object.SetActive(true);
        }
    }
}