using UnityEngine;

namespace Richie.GameProject
{
    public class PlayerAnimations : MonoBehaviour
    {
        private Animator _anim;
        private PlayerMovement _player;
        private PlayerGrappler _grappler;

        // dedicated script to set values in the player animator //

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _player = GetComponent<PlayerMovement>();
            _grappler = GetComponent<PlayerGrappler>();

            _player.OnJump += Player_OnJump;
            _player.OnWall += Player_OnWall;
            _player.OnCrouch += Player_OnCrouch;
            _player.OnMovement += Player_OnMovement;
            _player.OnSlide += Player_OnSlide;

            _grappler.OnSwing += Grappler_OnSwing;
            _grappler.OnJump += Grappler_OnJump;
        }

        private void Grappler_OnJump(float input)
           => _anim.SetFloat("Speed", Mathf.Abs(input));

        private void Grappler_OnSwing(bool state) 
            => _anim.SetBool("isGrappling", state);
        private void Player_OnSlide(bool state)
            => _anim.SetBool("isSlidingFloor", state);

        private void Player_OnCrouch(bool state)
            => _anim.SetBool("isCrouching", state);

        private void Player_OnWall(bool state)
            => _anim.SetBool("isSliding", state);

        private void Player_OnJump(bool state)
            => _anim.SetBool("isJumping", state);

        private void Player_OnMovement(float input)
            => _anim.SetFloat("Speed", Mathf.Abs(input));

        public void Grounded() // used at the end of the first level when the player is walking out of view and jumping is not allowed //
            => _anim.SetBool("isJumping", false);

        public void ResetAnims()
        {   // use at the end of the second level to turn off all animations //
            _anim.SetBool("isSliding", false);
            _anim.SetBool("isJumping", false);
            _anim.SetBool("isCrouching", false);
            _anim.SetBool("isGrappling", false);
            _anim.SetFloat("Speed", Mathf.Abs(0));
            _anim.SetBool("isSlidingFloor", false);
        }
    }
}