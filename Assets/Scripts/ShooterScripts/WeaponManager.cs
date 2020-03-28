using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
    public GunScript[] weapons;
    [SerializeField]
    private CameraShake shaker;

    public GunScript currentWeapon;
    private int minimumAmmo;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentWeapon = weapons[0];
        minimumAmmo = currentWeapon.clipSize * 3;
        UIManager.instance.UpdateAmmo();
    }

    public void ResetAmmo()
    {
        if (currentWeapon.type != GunType.Rifle)
        {
            if (currentWeapon.ammo <= minimumAmmo)
            {
                currentWeapon.ammo = minimumAmmo;
            }
            UIManager.instance.UpdateAmmo();
        }
    }

    public void SwitchWeapon()
    {
        foreach (var item in weapons)
        {
            if (!item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(true);
                currentWeapon = item;
            }
            else
            {
                item.gameObject.SetActive(false);
            }
            UIManager.instance.UpdateAmmo();
        }
    }

    private void Update()
    {
        if (!GameManager.instance.gamePaused)
        {
            if (Input.GetMouseButton(0) && currentWeapon != null)
            {
                currentWeapon.Shoot(shaker);
                UIManager.instance.UpdateAmmo();
            }
            if (Input.GetKeyDown(KeyCode.R) && currentWeapon != null)
            {
                currentWeapon.Reload();
                UIManager.instance.UpdateAmmo();
            }
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.T))
            {
                SwitchWeapon();
            }
#endif
        }
    }

    public void CollectedAmmo(GunType type, int collectedAmmo)
    {
        foreach (var item in weapons)
        {
            if (item.type == type)
            {
                AudioManager.instance.PlaySound(AudioClips.GunClick);
                item.ammo += collectedAmmo;
                UIManager.instance.UpdateAmmo();
            }
        }
    }
}
