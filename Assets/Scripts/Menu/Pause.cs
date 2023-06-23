using UnityEngine;

namespace Richie.GameProject
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] private AudioSource _music;
        [SerializeField] private GameObject _pause;
        [SerializeField] private GameObject[] _menus;

        private void OnEnable()
        {
            _music.Pause();
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
            if(_music != null && !_music.isPlaying) _music.Play();

            _pause.SetActive(true);
            foreach (GameObject menu in _menus) menu.SetActive(false);
        }
    }
}
