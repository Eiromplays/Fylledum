using System.Collections;
using Player;
using UnityEngine;
using VehicleBehaviour;

namespace Assets.Scripts.Managers
{
    public class VehicleManager : MonoBehaviour
    {
        public static VehicleManager Instance;

        public GameObject enterVehicleUi;
        [HideInInspector]
        public GameObject currentEnterVehicle;

        [InspectorName("Player")]
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

            if (!GetVehicleAndCamera(out var camera, false, true)) return;

            UpdateCamera(camera, true);

            enterVehicleUi.SetActive(false);
            inVehicle = true;
            Debug.Log($"Entered Vehicle");
            StartCoroutine(ExitVehicle());
        }

        private bool GetVehicleAndCamera(out Transform camera, bool handbrake, bool isPlayer)
        {
            var vehicle = currentEnterVehicle.GetComponentInParent<WheelVehicle>();
            if (vehicle == null)
            {
                camera = null;
                return false;
            }

            camera = vehicle.transform.Find("Camera");
            if (camera == null) return false;

            vehicle.toogleHandbrake(handbrake);
            vehicle.IsPlayer = isPlayer;
            return true;
        }

        private IEnumerator ExitVehicle()
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));

            if (currentEnterVehicle == null) yield break;

            if (!GetVehicleAndCamera(out var camera, true, false)) yield break;

            UpdateCamera(camera, false);

            inVehicle = false;
        }

        private void UpdateCamera(Transform camera, bool active)
        {
            CameraSwitch.instance.cameras[CameraSwitch.instance.currentCam].SetActive(true);
            camera.gameObject.SetActive(active);
            if (player != null)
            {
                player.SetActive(!active);
                player.transform.position = currentEnterVehicle.transform.position + Vector3.left * 3.5f;
            }
        }
    }
}