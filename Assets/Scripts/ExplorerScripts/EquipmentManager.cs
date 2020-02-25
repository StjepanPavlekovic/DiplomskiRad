using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;
    public List<Tool> equipment;
    public Tool equippedTool;

    private void Awake()
    {
        instance = this;
    }

    public void AddToEquipment(ItemType item)
    {
        GameManager.instance.NextQuest();
        for (int i = 0; i < equipment.Count; i++)
        {
            if (equipment[i].type == item)
            {
                equipment[i].EquipTool(true);
                equippedTool = equipment[i];
            }
            else
            {
                equipment[i].EquipTool(false);
            }
        }
    }

    public void RemoveFromEquipment()
    {
        GameManager.instance.NextQuest();
        equippedTool.gameObject.SetActive(false);
        equipment.Remove(equippedTool);
        equippedTool = null;
    }
}
