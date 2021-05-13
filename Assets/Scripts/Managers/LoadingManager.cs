using System;
using Assets.Scripts.Loading;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Managers
{
    public class LoadingManager : MonoBehaviour
    {
        public static LoadingManager Instance;

        private bool _alreadyLoaded;

        public GameObject loadingScreen;

        public Slider loadingBar;

        public TextMeshProUGUI loadingText;

        public TextMeshProUGUI tipsText;

        public Sprite[] backgrounds;
        public Image backgroundImage;

        public string[] tips;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            DontDestroyOnLoad(this);

            loadingScreen.SetActive(false);
            if (_alreadyLoaded || SceneManager.GetActiveScene().buildIndex == (int)SceneIndexes.MainMenu) return;

            Load((int)SceneIndexes.MainMenu);

            _alreadyLoaded = true;
        }

        public void Load(List<int> scenesToLoad, [Optional] List<int> scenesToUnload, 
            bool unloadActiveScene = true)
        {
            loadingScreen.SetActive(true);

            if (unloadActiveScene && SceneManager.GetActiveScene().buildIndex > 0)
            {
                ScenesLoading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex));
            }

            if (scenesToUnload != null)
                foreach (var scene in scenesToUnload.Where(scene => scene > 0).ToList())
                {
                    ScenesLoading.Add(SceneManager.UnloadSceneAsync(scene));
                }

            foreach (var scene in scenesToLoad)
            {
                ScenesLoading.Add(SceneManager.LoadSceneAsync(scene));
            }

            StartCoroutine(GetSceneLoadProgress());
            StartCoroutine(GenerateTips());
        }

        public void Load(int sceneToLoad, [Optional] int sceneToUnload, 
            bool unloadActiveScene = true)
        {
            Load(new List<int>{sceneToLoad}, new List<int> { sceneToUnload }, unloadActiveScene);
        }

        public List<AsyncOperation> ScenesLoading = new List<AsyncOperation>();

        private float _totalSceneProgress;

        // ReSharper restore Unity.ExpensiveCode
        public IEnumerator GetSceneLoadProgress()
        {
            var randomImageIndex = Random.Range(0, backgrounds.Length);
            var randomImage = backgrounds.ElementAtOrDefault(randomImageIndex);
            if (randomImage != null)
                backgroundImage.sprite = randomImage;

            foreach (var scene in ScenesLoading.ToList())
            {
                while (!scene.isDone)
                {
                    _totalSceneProgress = 0;

                    _totalSceneProgress = (scene.progress / ScenesLoading.Count) * 100f;

                    loadingBar.value = Mathf.RoundToInt(_totalSceneProgress);
                    loadingText.text = $"{_totalSceneProgress}%";

                    yield return null;
                }
            }
            ScenesLoading.Clear();

            loadingScreen.SetActive(false);
        }

        public int tipCount;
        public IEnumerator GenerateTips()
        {
            if (tips == null) yield break;
            tipCount = Random.Range(0, tips.Length);
            tipsText.text = tips[tipCount];

            while (loadingScreen.activeInHierarchy)
            {
                yield return new WaitForSeconds(3f);

                tipCount++;
                if (tipCount >= tips.Length)
                    tipCount = 0;

                tipsText.text = tips[tipCount];
            }
        }
    }
}