using UnityEngine;

namespace Richie.GameProject
{
    public class PlayerAudio : MonoBehaviour
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource _move;
        [SerializeField] private AudioSource _jump;
        [SerializeField] private AudioSource _slide;

        [Header("References")]
        [SerializeField] private PlayerMovement _player;

        // uses animation events or events received from the "PlayerMovement" script
        private void Awake()
        {
            _player = GetComponent<PlayerMovement>();
            _player.OnJump += Player_OnJump;
        }

        private void Player_OnJump(bool state)
        {
            if (!state) return;
            _jump.Stop();
            _jump.Play();
        }

        public void PlayStep()
        {
            _move.Stop();
            _move.Play();
        }

        public void PlaySlide()
        {
            _move.Stop();
            _slide.Stop();
            _slide.Play();
        }
    }
}