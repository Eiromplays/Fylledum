using System;
using Assets.Scripts.Managers;
using Managers;
using UnityEngine;

namespace Car
{
    public class HitGround : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collider)
        {
            if (!collider.name.Equals("Ground", StringComparison.OrdinalIgnoreCase)) return;

            if (VehicleManager.Instance == null) return;

            VehicleManager.Instance.GameOver();
        }
    }
}
