using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneButtonPuzzle : MonoBehaviour
{
    public static StoneButtonPuzzle instance;

    public GameObject[] buttons;

    private int buttonIndex = 0;

    [SerializeField]
    private GameObject stairs;

    [SerializeField]
    private GameObject dustFall;

    public CameraShake shaker;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buttons[buttonIndex].gameObject.SetActive(true);
    }

    public void NextButton()
    {
        buttonIndex++;
        if (buttonIndex < buttons.Length)
        {
            buttons[buttonIndex].gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(EndPuzzle());
        }
    }

    private IEnumerator EndPuzzle()
    {
        yield return new WaitForSeconds(2);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        shaker.enabled = true;
        shaker.shakeDuration = 3.1f;
        AudioManager.instance.PlaySound(AudioClips.Stairs);
        stairs.GetComponent<Animator>().SetTrigger("Lower");
        dustFall.SetActive(true);

        this.enabled = false;
    }
}
