using System.Collections;
using UnityEngine;

namespace Richie.GameProject
{
    public class Respawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _respawnTime;
        [SerializeField] private float _travelTime;

        [Header("Visuals")]
        [SerializeField] private GameObject _deathParticles;
        [SerializeField] private GameObject _respawnParticles;

        [Header("Sound Effects")]
        [SerializeField] private PlayAudio _deathSound;
        [SerializeField] private PlayAudio _respawnSound;

        [Header("References")]
        [SerializeField] private Transform _player;
        [SerializeField] private FollowTarget _followTarget;
        [HideInInspector] public Transform CheckPoint;

        private GameObject _particles;
        private Vector3 _startPos;

        public event TargetChanged OnChange;
        public delegate void TargetChanged(); 

        private void Start() => _startPos = _player.position;

        public void Respawn()
        {   // plays a death sound and spawns particles //
            _deathSound.Play();
            Instantiate(_deathParticles, _player.position, Quaternion.identity);
            _player.gameObject.SetActive(false);

            if (CheckPoint != null)
            {   // start the necessay coroutine based on if a checkpoint is available //
                StartCoroutine(TargetChange());
            }
            else StartCoroutine(StartPos());
        }

        private void Respawner_OnCall()
        {   // when particle finish playing, an event is called to then respawn the character //
            _player.position = CheckPoint.position;
            _player.gameObject.SetActive(true);
            _followTarget.Target = _player;

            _particles.GetComponent<Callback>().OnCall -= Respawner_OnCall;
        }   // unsubscripe from the particle's event //

        private IEnumerator PlayerRespawn()
        {
            for (int i = 0; i <= 1; i++)
            {
                if (i >= 1)
                {   // instead of spawning player, spawns the respawning particles and subscripes to an event //
                    _respawnSound.Play();
                    _particles = Instantiate(_respawnParticles, CheckPoint.position, Quaternion.identity);
                    _particles.GetComponent<Callback>().OnCall += Respawner_OnCall;
                }
                yield return new WaitForSeconds(_respawnTime);
            }
        }

        private IEnumerator TargetChange()
        {   // wait for a moment, then travels to the last checkpoint// 
            for (int i = 0; i <= 1; i++)
            {   
                if (i >= 1)
                {  // start the "PlayerRespawn" coroutine  
                    OnChange?.Invoke();

                    _followTarget.Target = CheckPoint;
                    StartCoroutine(PlayerRespawn());
                }
                yield return new WaitForSeconds(_travelTime);
            }
        }

        private IEnumerator StartPos()
        {   // used if no checkpoint are available //
            for (int i = 0; i <= 1; i++)
            {
                if (i >= 1)
                {
                    _player.position = _startPos;
                    _player.gameObject.SetActive(true);
                }
                yield return new WaitForSeconds(_travelTime);
            }
        }
    }
}