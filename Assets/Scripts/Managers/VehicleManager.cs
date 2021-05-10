using System.Collections;
using Player;
using UnityEngine;
using VehicleBehaviour;

namespace Managers
{
    public class VehicleManager : MonoBehaviour
    {
        public static VehicleManager instance;

        public GameObject enterVehicleUi;
        [HideInInspector]
        public GameObject currentEnterVehicle;

        public GameObject player;

        public bool inVehicle;

        private void Awake()
        {
            instance = this;
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

            CameraSwitch.instance.cameras[CameraSwitch.instance.currentCam].SetActive(false);
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

            CameraSwitch.instance.cameras[CameraSwitch.instance.currentCam].SetActive(true);
            camera.gameObject.SetActive(false);
            player.SetActive(true);

            inVehicle = false;
        }
    }
}