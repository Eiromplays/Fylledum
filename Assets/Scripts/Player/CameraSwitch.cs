using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CameraSwitch : MonoBehaviour
    {
        public List<GameObject> cameras = new List<GameObject>();

        public int currentCam;

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Tab)) return;
            
            ChangeCam(currentCam, false);
            currentCam++;
            ChangeCam(currentCam, true);
        }

        private void ChangeCam(int cam, bool active)
        {
            if (cameras.Count > cam && cameras[cam] != null)
            {
                cameras[cam].SetActive(active);
            }
            else
            {
                currentCam = 0;
                cameras[currentCam].SetActive(true);
            }
        }
    }
}