using UnityEngine;

namespace Richie.GameProject
{
    public class Callback : MonoBehaviour
    {
        public event Call OnCall;
        public delegate void Call();

        private void OnParticleSystemStopped()
        {
            OnCall?.Invoke();
            Destroy(gameObject);
        }
    }
}
