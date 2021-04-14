using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Weapons;

public class WeaponInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CurrentClipText;
    [SerializeField] private TextMeshProUGUI WeaponNameText;
    [SerializeField] private TextMeshProUGUI TotalAmmoText;

    private WeaponComponent EquippedWeapon;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerEvents.OnWeaponEquipped += OnWeaponEquipped;
    }
    
    // Start is called before the first frame update
    void OnDisable()
    {
        PlayerEvents.OnWeaponEquipped -= OnWeaponEquipped;
    }

    private void OnWeaponEquipped(WeaponComponent weapon)
    {

       // Debug.Log("Weapon Equipped");
        EquippedWeapon = weapon;
        WeaponNameText.text = weapon.WeaponStats.Name;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentClipText.text = EquippedWeapon.WeaponStats.BulletsInClip.ToString();
        TotalAmmoText.text = EquippedWeapon.WeaponStats.BulletsAvailable.ToString();
    }
}
