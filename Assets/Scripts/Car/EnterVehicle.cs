using System.Collections;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Car
{
    public class EnterVehicle : MonoBehaviour
    {
        private Coroutine _enterVehicleCoroutine;

        private void OnTriggerEnter(Collider col)
        {
            if (!col.name.Equals("Enter Vehicle")) return;
            VehicleManager.Instance.currentEnterVehicle = col.gameObject;
            Cursor.lockState = CursorLockMode.None;
            VehicleManager.Instance.enterVehicleUi.SetActive(true);

            _enterVehicleCoroutine = StartCoroutine(EnterVehicleKeyPressed());
        }

        private void OnTriggerExit(Collider col)
        {
            VehicleManager.Instance.currentEnterVehicle = null;
            Cursor.lockState = CursorLockMode.Locked;
            VehicleManager.Instance.enterVehicleUi.SetActive(false);

            if(_enterVehicleCoroutine != null)
                StopCoroutine(_enterVehicleCoroutine);
        }

        IEnumerator EnterVehicleKeyPressed()
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

            VehicleManager.Instance.EnterVehicle();
        }
    }
}