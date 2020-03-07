using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public bool friendly = true;
    public GunScript gun;
    public Material normalMaterial;
    public Material laserMaterial;
    public Renderer bulletRenderer;

    public float speed;

    public float lifeTime;

    private float currentLifetime;
    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentLifetime = lifeTime;
    }

    private void Update()
    {
        if ((currentLifetime -= Time.deltaTime) <= 0)
        {
            PoolingManager.instance.StoreBullet(gameObject);
        }

        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Target") && friendly)
        {
            other.gameObject.GetComponent<EnemyScript>().GetHit(this);
        }
        else if (other.gameObject.CompareTag("Player") && !friendly)
        {
            other.gameObject.GetComponent<PlayerController>().GetHit(gun.damage);
            PoolingManager.instance.StoreBullet(gameObject);
        }
        else if (other.gameObject.CompareTag("CameraTarget") && friendly)
        {
            other.gameObject.GetComponent<SecurityCameraController>().GetHit();
        }
    }

    private void OnDisable()
    {
        currentLifetime = lifeTime;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    public void StoreBullet()
    {
        PoolingManager.instance.StoreBullet(gameObject);
    }
}
