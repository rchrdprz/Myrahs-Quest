using UnityEngine.SceneManagement;
using UnityEngine;

namespace Richie.GameProject
{
    public class SwitchScene : MonoBehaviour
    {
        [SerializeField] private int _sceneIndex;

        void Update()
        {
            if (Input.anyKey) SceneManager.LoadScene(_sceneIndex);
        }
    }
}