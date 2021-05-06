using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Player
{
    public class CameraSwitch : MonoBehaviour
    {
        public static CameraSwitch Instance;

        public List<GameObject> cameras = new List<GameObject>();

        public List<GameObject> vehicleCameras = new List<GameObject>();

        public int currentCam;

        private void Awake()
        {
            Instance = this;
        }

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