using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.Serialization;
using VehicleBehaviour;

namespace Assets.Scripts.Managers
{
    public class VehicleManager : MonoBehaviour
    {
        public static VehicleManager Instance;

        public GameObject enterVehicleUi;
        [HideInInspector]
        public GameObject currentEnterVehicle;

        public GameObject player;

        public bool inVehicle;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            enterVehicleUi.SetActive(false);
        }

        public void EnterVehicle()
        {
            if (currentEnterVehicle == null) return;

            var vehicle = currentEnterVehicle.GetComponentInParent<WheelVehicle>();
            if (vehicle == null) return;

            var camera = vehicle.transform.Find("Camera");
            if (camera == null) return;

            vehicle.toogleHandbrake(false);
            vehicle.IsPlayer = true;

            CameraSwitch.Instance.cameras[CameraSwitch.Instance.currentCam].SetActive(false);
            camera.gameObject.SetActive(true);
            player.SetActive(false);

            enterVehicleUi.SetActive(false);
            inVehicle = true;
            Debug.Log($"Entered Vehicle");
            StartCoroutine(ExitVehicle());
        }

        private IEnumerator ExitVehicle()
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));

            if (currentEnterVehicle == null) yield break;

            var vehicle = currentEnterVehicle.GetComponentInParent<WheelVehicle>();
            if (vehicle == null) yield break;

            var camera = vehicle.transform.Find("Camera");
            if (camera == null) yield break;

            vehicle.toogleHandbrake(true);
            vehicle.IsPlayer = false;

            CameraSwitch.Instance.cameras[CameraSwitch.Instance.currentCam].SetActive(true);
            camera.gameObject.SetActive(false);
            player.SetActive(true);

            inVehicle = false;
        }
    }
}