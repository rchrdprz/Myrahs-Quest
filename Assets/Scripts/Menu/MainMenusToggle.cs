using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Richie.GameProject
{
    public class MainMenusToggle : MonoBehaviour
    {
        [SerializeField] private GameObject _current;
        [SerializeField] private GameObject[] _previous;

        public void SetActive()
        {
            if (_current.activeInHierarchy)
            {
                _current.SetActive(false);
            }
            else 
            {
                foreach (var menu in _previous) menu.SetActive(false);
                _current.SetActive(true);
            }
        }

        public void MenuOverlayed()
        {
            if (_current.activeInHierarchy)
            {
                _current.SetActive(false);
            }
            else _current.SetActive(true);
        }
    }
}
