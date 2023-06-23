using UnityEngine;

namespace Richie.GameProject
{
    public class SwapMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _previous;
        [SerializeField] private GameObject _current;

        public void MenuSwap()
        {
            _previous.gameObject.SetActive(false);
            _current.gameObject.SetActive(true);
        }

        public void ReturnMenu()
        {
            if(_previous != null)
            _previous.gameObject.SetActive(true);
            _current.gameObject.SetActive(false);
        }

        public void MenuOverlay() 
            => _current.gameObject.SetActive(true);
    }
}