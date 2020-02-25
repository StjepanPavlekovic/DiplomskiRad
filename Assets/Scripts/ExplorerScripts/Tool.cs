using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Torch, ExtinguishedTorch, PlacedTorch, NONE, HeldGem
}

public class Tool : MonoBehaviour
{
    public ItemType type;
    public bool selected;

    private void Start()
    {
        EquipmentManager.instance.equipment.Add(this);
        gameObject.SetActive(false);
    }

    public void EquipTool(bool state)
    {
        if (state)
        {
            selected = true;
            gameObject.SetActive(selected);
            EquipmentManager.instance.equippedTool = this;
        }
        else
        {
            selected = false;
            gameObject.SetActive(false);
        }
    }
}
