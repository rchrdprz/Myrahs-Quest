using UnityEngine;

namespace Richie.GameProject
{
    public class Despawner : MonoBehaviour
    {
        public void Despawn() => gameObject.SetActive(false);
    }
}
