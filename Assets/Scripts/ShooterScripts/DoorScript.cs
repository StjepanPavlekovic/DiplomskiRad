using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;

    public bool requiresKeyCard = true;

    private bool doorOpen = false;

    public void OpenDoor()
    {
        StartCoroutine(DoorRoutine());
    }

    IEnumerator DoorRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlaySound(AudioClips.DoorSlide);
        leftDoor.GetComponent<Animator>().enabled = true;
        rightDoor.GetComponent<Animator>().enabled = true;
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (requiresKeyCard && !doorOpen)
        {
            if (other.CompareTag("Player") && !doorOpen)
            {
                foreach (var item in GameManager.instance.player.keyItems)
                {
                    if (item.type == KeyItemType.KeyCard)
                    {
                        AudioManager.instance.PlaySound(AudioClips.KeyAccepted);
                        doorOpen = true;
                        OpenDoor();
                    }
                }
            }
        }
    }
}
