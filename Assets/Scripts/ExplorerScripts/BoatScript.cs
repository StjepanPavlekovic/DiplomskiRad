using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatScript : MonoBehaviour
{
    public void BeginEndGameSequence()
    {
        UIManager.instance.EndTheGame();
        this.enabled = false;
    }
}
