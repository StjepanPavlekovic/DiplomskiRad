using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public ParticleSystem healthParticleSystem;
    public ParticleSystem ammoParticleSystem;
    public ParticleSystem damageParticleSystem;

    public List<KeyItem> keyItems;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float sprintFactor;
    [SerializeField]
    private float gravityForce = -18f;
    private bool sprinting;
    private Vector3 deltaPos;
    private Vector3 previousPosition;
    public bool isGrounded = false;
    public bool jumping = false;
    public Rigidbody rb;
    public bool died = false;

    //Shooter related
    public float health;
    private float initialHealth;

    void Start()
    {
        if (GameManager.instance.game == Game.Shooter)
        {
            keyItems = new List<KeyItem>();
            initialHealth = health;
        }
        rb = GetComponent<Rigidbody>();
        GameManager.instance.player = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.gamePaused && !GameManager.instance.isDead)
        {
            handleMovement();
            handleForces();
            handleJumping();

            if (Input.GetKeyDown(KeyCode.K))
            {
                GameManager.instance.KillPlayer();
            }
        }
        else if (GameManager.instance.isDead && !died)
        {
            died = true;
            if (GameManager.instance.game == Game.Explorer)
            {
                AudioManager.instance.PlaySound(AudioClips.ExplorerDeath);
            }
        }
    }

    private void handleMovement()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            direction = new Vector3(1, 0, 1);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            direction = new Vector3(-1, 0, 1);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            direction = new Vector3(-1, 0, -1);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            direction = new Vector3(1, 0, -1);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direction = Vector3.forward;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = Vector3.right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = -Vector3.right;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = -Vector3.forward;
        }
        if (!sprinting)
        {
            transform.Translate(direction.normalized * movementSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction.normalized * movementSpeed * sprintFactor * Time.deltaTime);
        }
    }

    private void handleJumping()
    {
        deltaPos = (transform.position - previousPosition);
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isGrounded = false;
            rb.AddForce((Vector3.up + (((!isGrounded) ? deltaPos : (deltaPos / 2))).normalized / 4.2f) * jumpPower);
            GameManager.instance.jumpsCount++;
        }
        previousPosition = transform.position;
    }

    private void handleForces()
    {
        if (!isGrounded)
        {
            if (rb.velocity.y < 0)
            {
                jumping = false;
                rb.velocity += Vector3.up * gravityForce * 2 * Time.deltaTime;
            }
            else
            {
                rb.velocity += Vector3.up * gravityForce * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathTrap"))
        {
            if (GameManager.instance.game == Game.Explorer)
            {
                GameManager.instance.KillPlayer();
            }
            else
            {
                GameManager.instance.KillPlayer(null, "You have alerted the security...");
            }
        }
    }

    public void ResetPlayer()
    {
        health = initialHealth;
        GetComponent<WeaponManager>().ResetAmmo();
        UIManager.instance.UpdateHealth(health);
    }

    public void Heal(int healAmount, HealthPackScript hps)
    {
        GameManager.instance.interactionsCount++;
        if (health < initialHealth)
        {
            if ((health += healAmount) > initialHealth)
            {
                health = initialHealth;
            }
            Destroy(hps.gameObject);
            AudioManager.instance.PlaySound(AudioClips.CollectItem);
            healthParticleSystem.Play();
            UIManager.instance.UpdateHealth(health);
        }
    }

    public void GetHit(float damage)
    {
        GetComponentInChildren<CameraShake>().enabled = true;
        GetComponentInChildren<CameraShake>().shakeDuration = 0.15f;
        damageParticleSystem.Play();
        if ((health -= damage) <= 0)
        {
            health = 0;
            GameManager.instance.KillPlayer();
        }
        UIManager.instance.UpdateHealth(health);
    }
}
