using Managers;
using UnityEngine;

namespace Car
{
    public class EnterVehicle : MonoBehaviour
    {
        private void OnTriggerEnter(Collider col)
        {
            if (!col.name.Equals("Enter Vehicle")) return;
            VehicleManager.instance.currentEnterVehicle = col.gameObject;
            Cursor.lockState = CursorLockMode.None;
            VehicleManager.instance.enterVehicleUi.SetActive(true);
        }

        private void OnTriggerExit(Collider col)
        {
            VehicleManager.instance.currentEnterVehicle = null;
            Cursor.lockState = CursorLockMode.Locked;
            VehicleManager.instance.enterVehicleUi.SetActive(false);
        }
    }
}