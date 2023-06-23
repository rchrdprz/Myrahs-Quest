using UnityEngine;

namespace Richie.GameProject
{
    public class AnimTrigger : MonoBehaviour
    {
        private Animator _anim;

        private void Awake() 
            => _anim = GetComponent<Animator>();

        private void OnTriggerEnter2D(Collider2D _) 
            => _anim.SetTrigger("isActive");
    }
}