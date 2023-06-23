using UnityEngine;

namespace Richie.GameProject
{
    public class PortraitAnim : MonoBehaviour
    {
        [SerializeField, Range(0, 100)] private int _blinkChance;
        [SerializeField] private Animator _animator;

        public void Blink()
        {   // used to make the portrait in the dialgue blink randomly //
            int number = Random.Range(0, 100);
            if (number <= _blinkChance) _animator.SetTrigger("isActive");
        }
    }
}