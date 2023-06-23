using System.Collections;
using UnityEngine;

namespace Richie.GameProject
{
    public class Dialogue : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _popupTime = 0.75f;
        private bool _isSkipped, _isActive, _isOpened;
        private float _timer;

        [Header("References")]
        [SerializeField] private TextBox _textBox;
        [SerializeField] private Typewriter _typewriter;
        [SerializeField] private FollowTarget _followTarget;
        private PlayerManager _playerManager;

        public event SkipText OnSkip;
        public delegate void SkipText();

        private void Awake() 
            => _playerManager = GetComponent<PlayerManager>();

        private void Start()
        {
            _textBox.OnOpen += TextBox_OnOpen;
            _textBox.OnClose += Textbox_OnClose;

            _typewriter.OnComplete += Typewriter_OnComplete;

            _followTarget.OnSwitch += FollowTarget_OnSwitch;
        }

        private void Typewriter_OnComplete() => _isSkipped = true;

        private void Update()
        {   // pressing any key twice may skip dialgue completely //
            if (!_isActive) return;
            if (Input.anyKeyDown) Skip();
        }

        private void Skip()
        {   // allows the diague to be skipped //
            if(!_isActive) return;

            if (!_isSkipped)
            {
                OnSkip?.Invoke();
            }
            else
            {
                if (!_isOpened) return;
                _textBox.GetComponent<Animator>().SetTrigger("isClose");
            }
        }

        private void TextBox_OnOpen() => _isOpened = true;

        private void Textbox_OnClose()
        {   // when text box closes, then the players neccesary components get enabled //
            _playerManager.Activate();
            _textBox.gameObject.SetActive(false);
            GetComponent<Dialogue>().enabled = false;
        }

        private void FollowTarget_OnSwitch()
        {   // after the camera switches from transition to following the player, enable to timer to show dialogue //
            _isActive = true;
            StartCoroutine(ShowTime());
        } 

        private IEnumerator ShowTime()
        {  // enables the dialogue box which makes its animation play //
            _timer = _popupTime;
            while (_timer >= 0)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0) _textBox.gameObject.SetActive(true);
                yield return null;
            }
        }
    }
}