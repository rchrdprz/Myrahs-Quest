using UnityEngine;

namespace Richie.GameProject
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private PlayAudio _sound;
        [SerializeField] private PlayerManager _mananger;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _sound.Play();
            _mananger.ToggleAim(true);
            gameObject.SetActive(false);

            //collision.GetComponent<PlayerAim>().enabled = true;
        }
    }
}
