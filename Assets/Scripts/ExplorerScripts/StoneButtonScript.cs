using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneButtonScript : MonoBehaviour, Interactable
{
    private bool pressed;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        AudioManager.instance.PlaySound(AudioClips.ButtonSound);
    }

    public void Interact()
    {
        if (!pressed)
        {
            GameManager.instance.interactionsCount++;
            pressed = true;
            animator.SetTrigger("Press");
            AudioManager.instance.PlaySound(AudioClips.ButtonSound);
            GetComponentInChildren<ParticleSystem>().Stop();
            StoneButtonPuzzle.instance.NextButton();
        }
    }
}
