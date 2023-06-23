using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Richie.GameProject
{
    public class AsyncLoader : MonoBehaviour
    {
        [SerializeField] private int _sceneIndex;
        private AsyncOperation operation;

        public event SyncProgress OnProgress;
        public delegate void SyncProgress(float progress);

        private void Start() => StartCoroutine(LoadAsync(_sceneIndex));

        private IEnumerator LoadAsync(int index)
        {
            operation = SceneManager.LoadSceneAsync(index);
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                OnProgress?.Invoke(progress);
                yield return null;
            }
        }

        public void Activation() => operation.allowSceneActivation = true;
    }
}
