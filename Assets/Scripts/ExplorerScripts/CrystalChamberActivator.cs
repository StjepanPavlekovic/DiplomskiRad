using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalChamberActivator : MonoBehaviour
{
    public GameObject chamberGate;
    public CameraShake shaker;

    [SerializeField]
    private bool gemRoom;

    void Start()
    {
        chamberGate.GetComponent<Animator>().enabled = true;
        AudioManager.instance.PlaySound(AudioClips.Stairs);
        if (gemRoom)
        {
            shaker.enabled = true;
            shaker.shakeDuration = 2f;
        }
        gameObject.SetActive(false);
    }
}
