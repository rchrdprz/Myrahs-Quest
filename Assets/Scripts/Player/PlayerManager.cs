using UnityEngine;

namespace Richie.GameProject
{
    public class PlayerManager : MonoBehaviour
    {
        // used to turn of player related components at the beggining of the scene //
        [Header("Settings")]
        [SerializeField] private bool _aimActivate;
        [SerializeField] private bool _moveActivate;
        [SerializeField] private bool _isDialogue;

        [Header("References")]
        [SerializeField] PlayerAim _playerAim;
        [SerializeField] PlayerMovement _playerMove;
        [SerializeField] FollowTarget _followTarget;

        private MenuToggle _paused;
        private bool _isAimEnabled { get; set; }
        private bool _aimState { get; set; }

        private void Awake()
        {
            _paused = GetComponent<MenuToggle>();

            _aimState = _playerAim.enabled;
            _isAimEnabled = _playerAim.enabled;

            _playerAim.enabled = false;
            _playerMove.enabled = false;

            _followTarget.OnSwitch += FollowTarget_OnSwitch;
            _paused.OnPause += Paused_OnPause;
        }

        private void Paused_OnPause(bool state)
        {   // disable player aim when paused because it doesnt use an update function //

            if (state)
            {
                // game is paused // 
                _playerAim.enabled = false;
            }
            else _playerAim.enabled = _isAimEnabled;
        }

        private void FollowTarget_OnSwitch()
        {   // active certain components on the player when camera switches from transitioning, to follow the player //
            if (_isDialogue) return;
            if(_aimActivate) _playerAim.enabled = true;
            if(_moveActivate) _playerMove.enabled = true;
            _isAimEnabled = _aimState;
        }

        public void Activate()
        {   // activates all components //
            if (_aimActivate) _playerAim.enabled = true;
            if (_moveActivate) _playerMove.enabled = true;
            _isAimEnabled = _aimState;
        }

        public void Deactivate()
        {   // deactivates all components //
            _playerAim.enabled = false;
            _playerMove.enabled = false;
            _isAimEnabled = false;
        }

        public void ToggleAim(bool state) 
        {
            _aimState = state;
            _isAimEnabled = state;
            _playerAim.enabled = state;
        }
    }
}