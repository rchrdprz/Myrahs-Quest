using UnityEngine;

namespace Richie.GameProject
{
    public class DustDespawn : MonoBehaviour
    {
        [SerializeField] private float maxTime = 2f;
        private SpriteRenderer sprite;
        private float time;

        void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
            time = maxTime;
        }

        void Update()
        {   // fades the player's jump visuals overtime and then destroys them //
            time -= Time.deltaTime;
            sprite.color = new Color(1f, 1f, 1f, time);
            if (time <= 0) Destroy(gameObject);
        }
    }
}