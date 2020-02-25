using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public CurrentQuest checkpointForQuest;

    [SerializeField]
    private GameObject[] toActivate;
    [SerializeField]
    private GameObject[] toDisable;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance.currentQuest == checkpointForQuest)
            {
                GameManager.instance.NextQuest();
                ActivateAndDisableObjects();
                this.gameObject.SetActive(false);
            }
        }
    }

    private void ActivateAndDisableObjects()
    {
        for (int i = 0; i < toActivate.Length; i++)
        {
            toActivate[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < toDisable.Length; i++)
        {
            toDisable[i].gameObject.SetActive(false);
        }
    }
}
