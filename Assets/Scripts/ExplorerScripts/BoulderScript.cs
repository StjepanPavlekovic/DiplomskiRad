using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderScript : MonoBehaviour
{
    [SerializeField]
    public float boulderSpeed;

    private Vector3 initialPosition;

    private Rigidbody rb;

    private void Awake()
    {
        initialPosition = gameObject.transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = true;
        AudioManager.instance.PlaySound(AudioClips.Stairs);
    }

    private void FixedUpdate()
    {
        if (rb.useGravity)
        {
            rb.AddForce(Vector3.right * boulderSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathTrap"))
        {
            StartCoroutine(BoulderFallBuffer());
        }
    }

    private IEnumerator BoulderFallBuffer()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
        transform.position = initialPosition;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.KillPlayer(gameObject);
            transform.position = initialPosition;
        }
    }
}
