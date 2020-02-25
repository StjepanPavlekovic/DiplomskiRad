using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PillarType
{
    RED, GREEN, BLUE, YELLOW
}

public class PillarScript : MonoBehaviour, Interactable
{
    private Transform player;
    public bool locked = false;
    public PillarType type;
    public Rigidbody rb;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (locked)
        {
            rb.isKinematic = true;
            this.enabled = false;
        }
    }

    public void Interact()
    {
        if (!locked)
        {
            rb.AddForce(player.transform.forward * 8f, ForceMode.Impulse);
            AudioManager.instance.PlaySound(AudioClips.MovePillar);
        }
    }
}
