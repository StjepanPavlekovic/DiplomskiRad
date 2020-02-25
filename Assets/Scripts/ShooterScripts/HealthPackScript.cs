using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrateType
{
    Health, Ammunition
}

public class HealthPackScript : MonoBehaviour
{
    [SerializeField]
    private CrateType type;
    [SerializeField]
    private int value;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            switch (type)
            {
                case CrateType.Health:
                    player.Heal(value, this);
                    return;
                default:
                    WeaponManager.instance.CollectedAmmo(GunType.Gun, value);
                    player.ammoParticleSystem.Play();
                    AudioManager.instance.PlaySound(AudioClips.CollectItem);
                    Destroy(gameObject);
                    return;
            }
        }
    }

}
