using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerTrapScript : MonoBehaviour
{
    public float initialDuration;
    private float duration;
    public ParticleSystem[] particles;
    public BoxCollider trigger;
    public Light pointLight;

    void Start()
    {
        duration = initialDuration;
        StartCoroutine(FlamethrowerSequence());
    }

    private IEnumerator FlamethrowerSequence()
    {
        while (enabled)
        {
            yield return new WaitForEndOfFrame();
            if ((duration -= Time.deltaTime) <= 0)
            {
                Toggle();
                duration = initialDuration;
            }
        }
    }

    private void Toggle()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].isPlaying)
            {
                particles[i].Stop();
            }
            else
            {
                particles[i].Play();
            }
        }
        if (trigger.enabled)
        {
            trigger.enabled = false;
        }
        else
        {
            trigger.enabled = true;
            GetComponent<AudioSource>().Play();
        }
        if (pointLight.gameObject.activeInHierarchy)
        {
            pointLight.gameObject.SetActive(false);
        }
        else
        {
            pointLight.gameObject.SetActive(true);
        }
    }
}
