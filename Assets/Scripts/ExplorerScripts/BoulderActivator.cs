using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderActivator : MonoBehaviour
{
    [SerializeField]
    private GameObject boulder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !boulder.activeInHierarchy)
        {
            boulder.SetActive(true);
        }
    }
}
