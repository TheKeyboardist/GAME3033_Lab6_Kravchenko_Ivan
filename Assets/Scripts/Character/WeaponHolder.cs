using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace Character
{
    public class WeaponHolder : MonoBehaviour
    {

        [SerializeField] private GameObject Weapon;

        [SerializeField] private Transform WeaponSocket;

        private Transform GripLocation;


        private bool WasFiring = false;
        private bool FiringPressed = false; 
        //Components
        public PlayerController Controller => PlayerController;
        private PlayerController PlayerController;
        private Animator PlayerAnimator;
        
        

        //Ref
        private Camera MainCamera;
        private WeaponComponent EquippedWeapon;
    
        //Animator Hashes
        private readonly int AimVerticalHash = Animator.StringToHash("AimVertical");
        private readonly int AimHorizontalHash = Animator.StringToHash("AimHorizontal");
        private readonly int IsFiringHash = Animator.StringToHash("IsFiring");
        private readonly int IsReloadingHash = Animator.StringToHash("IsReloading");

        private void Awake()
        {
            PlayerController = GetComponent<PlayerController>();
            PlayerAnimator = GetComponent<Animator>();
        
            MainCamera = Camera.main;
        }

        // Start is called before the first frame update
        private void Start()
        {
            GameObject spawnedWeapon = Instantiate(Weapon, WeaponSocket.position, WeaponSocket.rotation);

            if (!spawnedWeapon) return;

            if (spawnedWeapon)
            {
                WeaponComponent weapon = spawnedWeapon.GetComponent<WeaponComponent>();
                if(weapon)
                {
                    PlayerAnimator.SetInteger("WeaponType", (int)weapon.WeaponStats.WaeaponType);
                    spawnedWeapon.transform.parent = WeaponSocket;
                    EquippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();
                    GripLocation = EquippedWeapon.HandPosition;
                    EquippedWeapon.Initialize(this, PlayerController.CrosshairComponent);

                    PlayerEvents.Invoke_OnWeaponEquipped(EquippedWeapon);
                }
            }

            
        }

        public void OnLook(InputValue delta)
        {
            Vector3 independentMousePosition =
                MainCamera.ScreenToViewportPoint(PlayerController.CrosshairComponent.CurrentMousePosition);


           // Debug.Log(independentMousePosition);
            PlayerAnimator.SetFloat(AimVerticalHash, independentMousePosition.y);
            PlayerAnimator.SetFloat(AimHorizontalHash, independentMousePosition.x);
        }
        public void StopFiring()
        {
            PlayerController.IsFiring = false;
            PlayerAnimator.SetBool(IsFiringHash, PlayerController.IsFiring);
            EquippedWeapon.StopFiring();
        }
        public void StartFiring()
        {

            if (EquippedWeapon.WeaponStats.BulletsAvailable <= 0 &&
                EquippedWeapon.WeaponStats.BulletsInClip <= 0) return;
            
            PlayerController.IsFiring = true;
            PlayerAnimator.SetBool(IsFiringHash, PlayerController.IsFiring);
            EquippedWeapon.StartFiring();
        }
        public void OnFire(InputValue button)
        {
            FiringPressed = button.isPressed;
            if (FiringPressed)
            {
                StartFiring();
            }
            else
            {
                StopFiring();
            }
            
        }

        
        public void OnReload(InputValue button)
        {
            StartReloading();
            PlayerAnimator.SetBool(IsReloadingHash,PlayerController.IsReloading);
            
        }

        public void StartReloading()
        {
            if (PlayerController.IsFiring)
            {
                WasFiring = true;
                StopFiring();
            }
            PlayerController.IsReloading = true;
            PlayerAnimator.SetBool(IsReloadingHash, PlayerController.IsReloading);
            EquippedWeapon.StartReloading();
            
            InvokeRepeating(nameof(StopReloading),0, 0.1f);
        }
        
        public void StopReloading()
        {
            

            if (PlayerAnimator.GetBool(IsReloadingHash)) return;

            PlayerController.IsReloading = false;
            // PlayerAnimator.SetBool(IsReloadingHash, PlayerController.IsReloading);
            EquippedWeapon.StopReloading();

            CancelInvoke(nameof(StopReloading));


            if (!WasFiring || !FiringPressed)
            {
                return;
            }
            StartFiring();
            WasFiring = false;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            PlayerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            PlayerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, GripLocation.position);
        }
        
    }
}
