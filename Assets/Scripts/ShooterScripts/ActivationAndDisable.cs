using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationAndDisable : MonoBehaviour, Interactable
{
    public bool shouldPlaySound = false;
    public AudioClips clipToPlay;
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    public void Interact()
    {
        GameManager.instance.interactionsCount++;
        if (shouldPlaySound)
        {
            AudioManager.instance.PlaySound(clipToPlay);
        }
        foreach (var item in objectsToDisable)
        {
            item.SetActive(false);
        }
        foreach (var item in objectsToEnable)
        {
            item.SetActive(true);
        }
        GameManager.instance.NextQuest();
        this.enabled = false;
    }
}
