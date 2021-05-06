using Assets.Scripts.Loading;
using Assets.Scripts.Managers;
using Cysharp.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Helpers
{
    public class LoadingHelper : MonoBehaviour
    {
        public static async UniTask LoadSceneAsync(int sceneToLoad, [Optional] int sceneToUnload,
            bool unloadActiveScene = true)
        {
            await UniTask.SwitchToMainThread();
            if (LoadingManager.Instance == null)
            {
                await SceneManager.LoadSceneAsync((int) SceneIndexes.MainMenu, LoadSceneMode.Additive);
            }

            LoadingManager.Instance.Load(sceneToLoad, sceneToUnload, unloadActiveScene);
        }

        public static void LoadScene(int sceneToLoad, [Optional] int sceneToUnload,
            bool unloadActiveScene = true)
        {
            AsyncHelper.Schedule("LoadSceneAsync",
                () => LoadSceneAsync(sceneToLoad, sceneToUnload, unloadActiveScene).AsTask());
        }
    }
}
