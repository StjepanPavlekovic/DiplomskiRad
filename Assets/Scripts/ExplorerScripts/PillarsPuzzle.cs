using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarsPuzzle : MonoBehaviour
{
    public PillarScript[] pillars;
    public PillarLock[] pillarLocks;
    public GemScript[] unlitGems;
    public GemScript[] litGems;
    public GameObject exitGate;

    private int correctCounter = 0;
    private bool puzzleComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < pillars.Length; i++)
        {
            pillars[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < pillarLocks.Length; i++)
        {
            pillars[i].gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (correctCounter < 4 && !puzzleComplete)
        {
            for (int i = 0; i < pillars.Length; i++)
            {
                if (!pillars[i].locked)
                {
                    for (int j = 0; j < pillarLocks.Length; j++)
                    {
                        if (pillars[i].type == pillarLocks[j].type)
                        {
                            if (closeEnough(pillars[i].transform.position, pillarLocks[j].transform.position))
                            {
                                correctCounter++;
                                pillars[i].locked = true;
                                ToggleGems(pillars[i].type);
                                AudioManager.instance.PlaySound(AudioClips.ToggleCrystal);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (!puzzleComplete)
            {
                puzzleComplete = true;
                StartCoroutine(CompletePuzzle());
            }
        }
    }

    void ToggleGems(PillarType pt)
    {
        for (int i = 0; i < unlitGems.Length; i++)
        {
            if (unlitGems[i].type == pt)
            {
                unlitGems[i].gameObject.SetActive(false);
                litGems[i].gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator CompletePuzzle()
    {
        exitGate.GetComponent<Animator>().enabled = true;
        AudioManager.instance.PlaySound(AudioClips.Stairs);

        GameManager.instance.NextQuest();

        yield return new WaitForSeconds(3);

        this.enabled = false;
    }

    public static bool closeEnough(Vector3 pos1, Vector3 pos2, float maxDifference = 0.2f)
    {
        return closeEnough(pos1.x, pos2.x, maxDifference) && closeEnough(pos1.y, pos2.y, maxDifference) && closeEnough(pos1.z, pos2.z, maxDifference);
    }

    public static bool closeEnough(float numberA, float numberB, float maxDifference = 0.2f)
    {
        return Mathf.Abs(numberA - numberB) < maxDifference;
    }
}
