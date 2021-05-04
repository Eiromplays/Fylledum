using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public GameObject optionsMenu;

        public void Play()
        {

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
