using System;
using UnityEngine;

namespace Weapons
{
    public class AK47WeaponComponent : WeaponComponent
    {

        private Camera ViewCamera;

        private Vector3 HitLocation;

        private void Awake()
        {
          ViewCamera = Camera.main;
        }

        protected new void FireWeapon()
        {

            
            //Debug.Log("Firing Weapon");

            if (WeaponStats.BulletsInClip > 0 && !Reloading && !WeaponHolder.Controller.IsRunning)
            {
                base.FireWeapon();

                Ray screenRay = ViewCamera.ScreenPointToRay(new Vector3(Crosshair.CurrentMousePosition.x,
                    Crosshair.CurrentMousePosition.y, 0));

                if (!Physics.Raycast(screenRay, out RaycastHit hit, WeaponStats.FireDistance,
                    WeaponStats.WeaponHitLayer)) return;

                HitLocation = hit.point;
                Vector3 hitDirection = hit.point - ViewCamera.transform.position;
                Debug.DrawRay(ViewCamera.transform.position, hitDirection.normalized * WeaponStats.FireDistance, Color.red);

               
                

                
            }
            else if(WeaponStats.BulletsInClip <= 0 && WeaponStats.BulletsAvailable > 0)
            {
                if (!WeaponHolder) return;

                WeaponHolder.StartReloading();
            }
        }

        private void OnDrawGizmos()
        {
            if (HitLocation == Vector3.zero) return;
            
            Gizmos.DrawWireSphere(HitLocation, 0.2f);
           
        }
    }
}
