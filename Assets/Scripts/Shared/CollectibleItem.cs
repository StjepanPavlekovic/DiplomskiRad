using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour, ICollectible, Interactable
{
    public ItemType itemType;
    public bool interactable;
    public bool collectible = true;
    [SerializeField]
    public bool needsToDisappear;
    public CurrentQuest questItem;

    public GameObject[] itemsToActivate;
    public GameObject[] itemsToDisable;

    private void Start()
    {
        GameManager.instance.questItems.Add(this);
    }

    public void CollectItem()
    {
        EquipmentManager.instance.AddToEquipment(itemType);
        ActivateAndDisableObjects();
        if (needsToDisappear)
        {
            Destroy(gameObject);
        }
    }

    public void HandleInteraction()
    {
        ActivateAndDisableObjects();
        if (itemType != ItemType.NONE)
        {
            EquipmentManager.instance.RemoveFromEquipment();
        }
        else
        {
            GameManager.instance.NextQuest();
        }

        if (needsToDisappear)
        {
            Destroy(gameObject);
        }
    }

    private void ActivateAndDisableObjects()
    {
        for (int i = 0; i < itemsToActivate.Length; i++)
        {
            itemsToActivate[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < itemsToDisable.Length; i++)
        {
            itemsToDisable[i].gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        if (interactable)
        {
            if (collectible)
            {
                CollectItem();
            }
            else
            {
                HandleInteraction();
            }
            interactable = false;
        }
    }
}
