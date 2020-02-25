using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    Gun, Rifle, None
}

public class GunScript : MonoBehaviour
{
    [SerializeField]
    private Gradient rifleColor;
    [SerializeField]
    private Gradient regularColor;
    [SerializeField]
    private float shootAlertRadius;
    [SerializeField]
    private LayerMask alertTargets;
    [SerializeField]
    private LayerMask obstacles;
    [SerializeField]
    private float flashIntensity = 2;
    public bool infiniteAmmo = false;
    public bool isEnemy = false;
    public GunType type;
    public int ammo;
    public int clipSize;
    public int ammoLeft;
    public float damage;
    public ParticleSystem flashParticles;
    public ParticleSystem smokeParticles;

    private Animator animator;
    public Light flash;

    public float fireRate;
    private float currentFireRate;

    public Transform bulletOrigin;

    private void Start()
    {
        AudioManager.instance.gunAudioSources.Add(GetComponent<AudioSource>());
        currentFireRate = fireRate;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentFireRate >= 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    public void Shoot(CameraShake shaker = null)
    {
        if (currentFireRate < 0 && ammoLeft > 0)
        {
            currentFireRate = fireRate;
            StartCoroutine(FlashHandler());
            animator.SetTrigger("Shoot");
            flashParticles.Play();
            if (type == GunType.Gun)
            {
                smokeParticles.Play();
            }
            GetAndSetupBullet();
            AlertEnemiesInRange();
            if (!isEnemy)
            {
                if (!infiniteAmmo)
                {
                    ammoLeft--;
                }
                shaker.enabled = true;
                shaker.shakeDuration = 0.15f;
            }
            AudioManager.instance.PlayShootSound(GetComponent<AudioSource>(), false);
        }
        else if (currentFireRate < 0 && ammoLeft <= 0)
        {
            currentFireRate = fireRate;
            AudioManager.instance.PlayShootSound(GetComponent<AudioSource>(), true);
        }
    }

    private void AlertEnemiesInRange()
    {
        Collider[] targetsInRange = Physics.OverlapSphere(bulletOrigin.position, shootAlertRadius, alertTargets);
        for (int i = 0; i < targetsInRange.Length; i++)
        {
            foreach (EnemyAI item in targetsInRange[i].GetComponents<EnemyAI>())
            {
                var dirToTarget = (item.transform.position - transform.position).normalized;
                float distToTarget = Vector3.Distance(transform.position, item.transform.position);

                Debug.DrawRay(bulletOrigin.position, dirToTarget * distToTarget, Color.red, 3f);
                if (!Physics.Raycast(bulletOrigin.position, dirToTarget, distToTarget, obstacles))
                {
                    item.Alert(bulletOrigin.position);
                }

            }
        }
    }

    public void Reload()
    {
        if (ammoLeft < clipSize)
        {
            for (int i = 0; i < clipSize; i++)
            {
                if (ammo > 0 && ammoLeft < clipSize)
                {
                    ammo--;
                    ammoLeft++;
                }
            }
            AudioManager.instance.PlaySound(AudioClips.GunClick);
        }
    }

    private void GetAndSetupBullet()
    {
        var bullet = PoolingManager.instance.SpawnBullet();
        var bulletScript = bullet.GetComponent<BulletScript>();
        if (type == GunType.Rifle)
        {
            bullet.GetComponent<TrailRenderer>().colorGradient = rifleColor;
            bulletScript.bulletRenderer.material = bulletScript.laserMaterial;
        }
        else
        {
            bullet.GetComponent<TrailRenderer>().colorGradient = regularColor;
            bulletScript.bulletRenderer.material = bulletScript.normalMaterial;
        }
        bullet.transform.position = bulletOrigin.position;
        bullet.transform.rotation = bulletOrigin.rotation;
        if (isEnemy)
        {
            bulletScript.friendly = false;
        }
        else
        {
            bulletScript.friendly = true;
        }
        bulletScript.gun = this;
        bullet.GetComponent<TrailRenderer>().Clear();
        bulletScript.rb.AddForce(bulletOrigin.forward * ((type == GunType.Rifle) ? (bulletScript.speed * 2) : bulletScript.speed));
    }
    private IEnumerator FlashHandler()
    {
        flash.intensity = flashIntensity;
        yield return new WaitForSeconds(flashParticles.main.duration);
        flash.intensity = 0;
    }

}