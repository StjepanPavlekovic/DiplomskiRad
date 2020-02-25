using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAltarScript : MonoBehaviour
{
    public Animator gunChamberAnimator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gunChamberAnimator.enabled = true;
            AudioManager.instance.PlaySound(AudioClips.DoorSlide);
            GetComponent<BoxCollider>().enabled = false;
            this.enabled = false;
        }
    }
}
