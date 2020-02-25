using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustFallController : MonoBehaviour
{
    [SerializeField]
    private float duration;

    private float currentDurration = 0;

    private void Start()
    {
        currentDurration = duration;
    }

    void Update()
    {
        if ((currentDurration -= Time.deltaTime) <= 0)
        {
            currentDurration = duration;
            gameObject.SetActive(false);
        }
    }
}
