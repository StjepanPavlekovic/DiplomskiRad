using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public CurrentQuest respawnForQuest;

    void Start()
    {
        GameManager.instance.respawnPoints.Add(this);
    }
}
