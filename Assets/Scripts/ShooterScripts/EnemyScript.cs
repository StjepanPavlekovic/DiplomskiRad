using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public GameObject weapon;
    public float shatterForce;
    public bool isDead = false;
    public float deathTime;
    public float health;
    private AudioSource audioSource;

    public GameObject[] bodies;

    public GameObject[] flashObjects;
    public GameObject keyItemDrop;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void GetHit(BulletScript bullet)
    {
        if ((health -= bullet.gun.damage) <= 0 && !isDead)
        {
            if (AudioManager.instance.useAudio)
            {
                int rand = Random.Range(0, 2);
                var clip = (rand == 1 ? AudioManager.instance.GetAudioClip(AudioClips.GlassSmash1) : AudioManager.instance.GetAudioClip(AudioClips.GlassSmash2));
                var clip2 = AudioManager.instance.GetAudioClip(AudioClips.GlassHit);
                if (clip != null && clip2 != null)
                {
                    audioSource.PlayOneShot(clip2);
                    audioSource.PlayOneShot(clip);
                }
            }
            isDead = true;
            var impactPosition = bullet.transform.position;
            var speed = bullet.speed;
            bullet.StoreBullet();
            weapon.GetComponent<Rigidbody>().useGravity = true;
            weapon.GetComponent<Rigidbody>().isKinematic = false;
            weapon.GetComponent<BoxCollider>().enabled = true;
            weapon.transform.parent = null;
            bodies[0].SetActive(false);
            bodies[1].SetActive(true);


            GetComponent<EnemyAI>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Rigidbody>().detectCollisions = false;
            Collider[] hitColliders = Physics.OverlapSphere(impactPosition, 0.5f);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].CompareTag("Enemy"))
                {
                    hitColliders[i].GetComponent<Rigidbody>().AddExplosionForce(shatterForce, impactPosition, 0.5f, 0.1f, ForceMode.Impulse);
                }
            }
            if (keyItemDrop != null)
            {
                if (!GameManager.instance.AlreadyCollected(keyItemDrop.GetComponentInChildren<KeyItem>().type))
                {
                    Instantiate(keyItemDrop, transform.position, keyItemDrop.transform.rotation);
                }
            }
            StartCoroutine(DeathSequence());
        }
        else
        {
            var clip = AudioManager.instance.GetAudioClip(AudioClips.GlassHit);
            GetComponent<EnemyAI>().Alert(Vector3.zero);
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
            bullet.StoreBullet();
        }
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(deathTime);

        Destroy(gameObject);
    }
}
