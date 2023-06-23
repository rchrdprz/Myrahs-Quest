using UnityEngine.SceneManagement;
using UnityEngine;

namespace Richie.GameProject
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(int index) => SceneManager.LoadScene(index);
    }
}
