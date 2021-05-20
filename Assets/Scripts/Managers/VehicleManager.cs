using System.Collections;
using Assets.Scripts.Helpers;
using Assets.Scripts.Loading;
using Assets.Scripts.Player;
using Player;
using UnityEngine;
using VehicleBehaviour;

namespace Managers
{
    public class VehicleManager : MonoBehaviour
    {
        public static VehicleManager Instance;

        public GameObject enterVehicleUi;
        [HideInInspector]
        public GameObject currentEnterVehicle;

        public GameObject gameOver;

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

        public void GameOver()
        {
            gameOver.SetActive(true);
            ExitCurrentVehicle();
            PlayerController.CanMove = false;
            StartCoroutine(GameOverOutro());
        }

        public void EnterVehicle()
        {
            if (currentEnterVehicle == null) return;

            if (!GetVehicleAndCamera(out var camera, false, true)) return;

            UpdateCamera(camera, true);

            enterVehicleUi.SetActive(false);
            inVehicle = true;
            
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

            vehicle.ToggleHandbrake(handbrake);
            vehicle.IsPlayer = isPlayer;
            return true;
        }

        private IEnumerator ExitVehicle()
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));

            ExitCurrentVehicle();
        }

        private IEnumerator GameOverOutro()
        {
            yield return new WaitForSeconds(3f);
            LoadingHelper.LoadScene((int)SceneIndexes.Outro);
        }

        private void ExitCurrentVehicle()
        {
            if (currentEnterVehicle == null) return;

            if (!GetVehicleAndCamera(out var camera, true, false)) return;

            UpdateCamera(camera, false);

            inVehicle = false;

            var vehicle = currentEnterVehicle.GetComponentInParent<WheelVehicle>();

            player.transform.position = vehicle.transform.position + (Vector3.left * 3.5f);
        }

        private void UpdateCamera(Transform camera, bool active)
        {
            CameraSwitch.instance.cameras[CameraSwitch.instance.currentCam].SetActive(true);
            camera.gameObject.SetActive(active);
            if (player != null)
            {
                player.SetActive(!active);
            }
        }
    }

    public class VehicleManagerTest
    {
        public void GameOver()
        {
            if (VehicleManager.Instance == null) return;

            VehicleManager.Instance.GameOver();
        }
    }
}