using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public bool isFinalActivator = false;
    public GameObject[] objectsToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isFinalActivator)
            {
                UIManager.instance.HideScreens();
                GameManager.instance.gamePaused = true;
            }
            for (int i = 0; i < objectsToActivate.Length; i++)
            {
                objectsToActivate[i].SetActive(true);
            }
        }
    }
}
