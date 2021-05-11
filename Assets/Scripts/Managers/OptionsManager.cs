using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class OptionsManager : MonoBehaviour
    {
        public GameObject titleScreen;
        public GameObject optionsMenu;

        private void Start()
        {
            optionsMenu = gameObject;
        }

        public void OptionsBack()
        {
            optionsMenu.SetActive(false);
            titleScreen.SetActive(true);
        }
    }
}
