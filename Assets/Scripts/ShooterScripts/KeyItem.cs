using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyItemType
{
    KeyCard, Note, LaserGun
}

public class KeyItem : MonoBehaviour, Interactable
{
    public KeyItemType type;
    public DoorScript doorForFinalKeyItem;

    private void Start()
    {
        if (type != KeyItemType.LaserGun)
        {
            GameManager.instance.cleanUpOnReset.Add(gameObject);
        }
    }

    public void Interact()
    {
        GameManager.instance.interactionsCount++;
        GameManager.instance.player.keyItems.Add(this);
        AudioManager.instance.PlaySound(AudioClips.Pickup);
        if (type == KeyItemType.Note && GameManager.instance.currentQuest == CurrentQuest.AvoidScouts)
        {
            UIManager.instance.UpdateHint();
        }
        if (type == KeyItemType.LaserGun)
        {
            UIManager.instance.ToggleInfiniteAmmo();
            WeaponManager.instance.SwitchWeapon();
            doorForFinalKeyItem.OpenDoor();
        }
        gameObject.SetActive(false);
    }
}
