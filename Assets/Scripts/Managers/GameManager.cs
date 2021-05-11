using Assets.Scripts.Helpers;
using Assets.Scripts.Loading;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public GameObject titleScreen;
        public GameObject optionsMenu;

        public void Play()
        {
            titleScreen.SetActive(false);
            LoadingHelper.LoadScene((int)SceneIndexes.Game);
        }

        public void Options()
        {
            titleScreen.SetActive(false);
            optionsMenu.SetActive(true);
        }

        public void Quit()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            Application.Quit();
        }
    }
}
