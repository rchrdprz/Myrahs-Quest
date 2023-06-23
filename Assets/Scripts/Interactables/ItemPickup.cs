using UnityEngine;

namespace Richie.GameProject
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private PlayAudio _sound;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _sound.Play();
            collision.GetComponent<PlayerAim>().enabled = true;
            gameObject.SetActive(false);
        }
    }
}
