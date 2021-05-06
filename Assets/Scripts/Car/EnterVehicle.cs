using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Car
{
    public class EnterVehicle : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collider)
        {
            if (!collider.name.Equals("Enter Vehicle")) return;
            VehicleManager.Instance.currentEnterVehicle = collider.gameObject;
            Cursor.lockState = CursorLockMode.None;
            VehicleManager.Instance.enterVehicleUi.SetActive(true);
        }

        private void OnTriggerExit(Collider collider)
        {
            VehicleManager.Instance.currentEnterVehicle = null;
            Cursor.lockState = CursorLockMode.Locked;
            VehicleManager.Instance.enterVehicleUi.SetActive(false);
        }
    }
}