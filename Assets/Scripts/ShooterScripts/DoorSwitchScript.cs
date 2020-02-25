using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitchScript : MonoBehaviour, Interactable
{
    public bool isSequenceTerminal = false;
    public DoorScript parentDoor;
    [SerializeField]
    private Renderer displayRenderer;
    [SerializeField]
    private Material activeMaterial;
    private Material originalMaterial;
    private static List<DoorSwitchScript> terminals;
    [SerializeField]
    private int[] sequence = { 3, 1, 0, 2 };
    [SerializeField]
    private int terminalNumber;
    private static int currentSequence = 0;
    private static int failCounter = 3;
    private bool interactable = true;

    private void Start()
    {
        if (isSequenceTerminal)
        {
            if (terminals == null)
            {
                terminals = new List<DoorSwitchScript>();
            }
            terminals.Add(this);
            originalMaterial = displayRenderer.material;
            Debug.Log(terminals.Count);
        }
    }

    private void OpenGate()
    {
        parentDoor.OpenDoor();
    }

    private void ResetSequence()
    {
        AudioManager.instance.PlaySound(AudioClips.WrongSequence);
        foreach (var item in terminals)
        {
            item.interactable = true;
            item.displayRenderer.material = originalMaterial;
        }
        if ((failCounter -= 1) <= 0)
        {
            failCounter = 3;
            AudioManager.instance.PlaySound(AudioClips.Alert);
            GameManager.instance.KillPlayer(null, "You were detected...");
        }
        currentSequence = 0;
    }

    private void OnDisable()
    {
        if (terminals.Count > 0)
        {
            terminals.Clear();
        }
        currentSequence = 0;
    }

    public void Interact()
    {
        if (interactable)
        {
            interactable = false;
            if (!isSequenceTerminal)
            {
                AudioManager.instance.PlaySound(AudioClips.KeyAccepted);
                parentDoor.OpenDoor();
                displayRenderer.material = activeMaterial;
            }
            else
            {
                if (terminalNumber == sequence[currentSequence])
                {
                    currentSequence++;
                    displayRenderer.material = activeMaterial;
                    AudioManager.instance.PlaySound(AudioClips.KeyAccepted);
                    if (sequence.Length == currentSequence)
                    {
                        OpenGate();
                        GameManager.instance.NextQuest();
                    }
                }
                else
                {
                    ResetSequence();
                }
            }
        }
    }
}
