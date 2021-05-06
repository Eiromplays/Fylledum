using Assets.Scripts.Helpers;
using Assets.Scripts.Loading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public GameObject optionsMenu;

        public void Play()
        {
            LoadingHelper.LoadScene((int)SceneIndexes.Game);
        }

        public void Options()
        {
            optionsMenu.SetActive(true);
        }

        public void Quit()
        {
            if (EditorApplication.isPlaying)
                EditorApplication.isPlaying = false;
            else Application.Quit();
        }
    }
}
