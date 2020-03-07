using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameStoneButtonScript : MonoBehaviour, Interactable
{
    private bool pressed;
    private Animator animator;
    private static int counter = 0;
    public FlamethrowerTrapScript[] flameThrowers;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (!pressed)
        {
            GameManager.instance.interactionsCount++;
            pressed = true;
            animator.SetTrigger("Press");
            AudioManager.instance.PlaySound(AudioClips.ButtonSound);
            counter++;

            if(counter >= 2)
            {
                for(int i = 0; i < flameThrowers.Length; i++)
                {
                    flameThrowers[i].enabled = false;
                }
            }

            this.enabled = false;
            SuperManager.instance.FoundOptionalMechanic("Disable Flamethrowers");
        }
    }
}
