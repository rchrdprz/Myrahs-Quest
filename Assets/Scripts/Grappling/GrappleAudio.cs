using UnityEngine;

namespace Richie.GameProject
{
    public class GrappleAudio : MonoBehaviour
    {
        [Header("Audio Clips")]
        [SerializeField] private AudioClip _release;
        [SerializeField] private AudioClip _anchored;
        [SerializeField] private AudioClip _return;

        [Header("References")]
        [SerializeField] private GrappleSystem _grapple;
        [SerializeField] private PlayerGrappler _player;

        private AudioSource _audio;

        private void Awake()
        {   // plays an certain audio clip when events are received //
            _audio = GetComponent<AudioSource>();

            _grapple.OnAchored += Grapple_OnAchored;
            _grapple.OnRelease += Grapple_OnRelease;

            _player.OnRetract += Player_OnRetract;
        }

        private void Player_OnRetract()
            => _audio.PlayOneShot(_return);

        private void Grapple_OnRelease() 
            =>_audio.PlayOneShot(_release);

        private void Grapple_OnAchored(float distance) 
            => _audio.PlayOneShot(_anchored);
    }
}