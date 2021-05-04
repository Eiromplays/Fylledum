using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Assets.Scripts.Managers
{
    public class LoadingManager : MonoBehaviour
    {
        public static LoadingManager Instance;

        public GameObject loadingScreen;

        private void Awake()
        {
            Instance = this;
        }

        public void LoadGame()
        {
            SceneManager.LoadSceneAsync((int) SceneIndexes.Game);
        }

        public List<AsyncOperation> ScenesLoading = new List<AsyncOperation>();

        private float _totalSceneProgress;
        public IEnumerator GetSceneLoadProgress()
        {
            foreach (var scene in ScenesLoading)
            {
                while (!scene.isDone)
                {
                    _totalSceneProgress = 0;

                    foreach (AsyncOperation operation in ScenesLoading)
                    {
                        _totalSceneProgress += operation.progress;
                    }

                    _totalSceneProgress = (_totalSceneProgress / ScenesLoading.Count) * 100f;

                    // Assign the value to the loading bar bar.current = Mathf.RoundToInt(_totalSceneProgress);

                    yield return null;
                }
            }

            loadingScreen.SetActive(false);
        }
    }
}
