using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public PlayerController player;

    //initial 0.15f
    [SerializeField]
    private float checkRadius;
    public LayerMask groundableSurfaces;

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, groundableSurfaces);

        if (hitColliders.Length > 0)
        {
            player.isGrounded = true;
        }
        else
        {
            player.isGrounded = false;
        }
    }
}
