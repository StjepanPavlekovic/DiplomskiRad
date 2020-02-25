using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GunType type;
    public int ammo;

    private void Start()
    {
        GameManager.instance.cleanUpOnReset.Add(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            WeaponManager.instance.CollectedAmmo(type, ammo);
            Destroy(gameObject);
        }
    }
}
