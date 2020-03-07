using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraController : MonoBehaviour
{
    public GameObject camComponents;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audioSource.Play();
    }

    public void GetHit()
    {
        camComponents.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Animator>().SetTrigger("Die");
        audioSource.Stop();
        audioSource.PlayOneShot(AudioManager.instance.GetAudioClip(AudioClips.PowerDown));
        this.enabled = false;
        SuperManager.instance.FoundOptionalMechanic("Destroy Security Camera");
    }
}
