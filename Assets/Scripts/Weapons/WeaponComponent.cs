using System;
using Character;
using UnityEngine;

public enum WeaponType
{
    None, 
    Pistol,
    MachineGun
}



    [Serializable]
    public struct WeaponStats
    {
        public WeaponType WaeaponType;
        public string Name;
        public float Damage;
        public int BulletsInClip;
        public int ClipSize;
        public int BulletsAvailable;

        public float FireStartDelay;
        public float FireRate;
        public float FireDistance;
        public bool Repeating;

        public LayerMask WeaponHitLayer;
    }
    
    public class WeaponComponent : MonoBehaviour
    {
        public Transform HandPosition => GripIKLocation;
        [SerializeField] private Transform GripIKLocation;

        public bool Firing { get; private set; }
        public bool Reloading { get; private set; }

        //public WeaponStats WeaponImforamtion => WeaponStats;
        [SerializeField] public WeaponStats WeaponStats;


        //protected Camera ViewCamera;
        protected WeaponHolder WeaponHolder;
        protected CrosshairScript Crosshair;

    private void Awake()
    {
        //ViewCamera = Camera.main;
    }
    public void Initialize(WeaponHolder weaponHolder, CrosshairScript crosshair)
        {
            WeaponHolder = weaponHolder;
            Crosshair = crosshair;
        }


        public virtual void StartFiring()
        {
            Firing = true;
            if (WeaponStats.Repeating)
            {
                InvokeRepeating(nameof(FireWeapon),WeaponStats.FireStartDelay, WeaponStats.FireRate);
            }
            else
            {
                FireWeapon();
            }
        }
        
        public virtual void StopFiring()
        {
            Firing = false;
            CancelInvoke(nameof(FireWeapon));
        }

        protected virtual void FireWeapon()
        {
        WeaponStats.BulletsInClip--;
        }

        public void StartReloading()
        {
            Reloading = true;
            ReloadWeapon();
        }
    public void StopReloading()
    {
        Reloading = false;
        
    }

    private void ReloadWeapon()
        {
            int bulletToReload = WeaponStats.ClipSize - WeaponStats.BulletsAvailable;
            if (bulletToReload < 0)
            {
            Debug.Log("Reload");
            WeaponStats.BulletsInClip = WeaponStats.ClipSize;
            WeaponStats.BulletsAvailable -= WeaponStats.ClipSize;
            
            }
            else
            {
            Debug.Log("Reload - Out of Ammo");
            WeaponStats.BulletsInClip = WeaponStats.BulletsAvailable;
            WeaponStats.BulletsAvailable = 0;
            }
        }

     
    }

