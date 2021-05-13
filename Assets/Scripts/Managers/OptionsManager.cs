using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class OptionsManager : MonoBehaviour
    {
        public GameObject titleScreen;
        public GameObject optionsMenu;

        public TMP_Dropdown resolutionDropdown;

        public List<Resolution> resolutions = new List<Resolution>();

        private void Start()
        {
            optionsMenu = gameObject;

            resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });

            resolutions = Screen.resolutions.ToList();
            resolutionDropdown.ClearOptions();
            foreach (var resolution in resolutions)
            {
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));
            }

            // Set current Resolution as the resolution
            resolutionDropdown.value = resolutions.IndexOf(Screen.currentResolution);
            OnResolutionChange();
        }

        public void OnResolutionChange()
        {
            Screen.SetResolution(resolutions[resolutionDropdown.value].width,
                resolutions[resolutionDropdown.value].height, Screen.fullScreenMode);
        }

        public void OptionsBack()
        {
            optionsMenu.SetActive(false);
            titleScreen.SetActive(true);
        }
    }
}
