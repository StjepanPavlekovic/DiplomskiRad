using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndShooterScript : MonoBehaviour
{
    void Start()
    {
        UIManager.instance.EndTheGame();
        this.enabled = false;
    }
}
