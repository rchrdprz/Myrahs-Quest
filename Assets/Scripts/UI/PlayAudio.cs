using UnityEngine;

namespace Richie.GameProject
{
    public class PlayAudio : MonoBehaviour
    {
        private AudioSource _source;

        private void Start() 
            => _source = GetComponent<AudioSource>();

        public void Play()
        {
            _source.Stop();
            _source.Play();
        }
    }
}