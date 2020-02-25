using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterObject : MonoBehaviour
{
    public bool isQuestObject = false;
    public GameObject solidObject;
    public GameObject fracturedObject;
    public float disappearTimer;
    private bool fragmented = false;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!fragmented)
            {
                StartCoroutine(BeginFragmentation(other.gameObject));
                fragmented = true;
            }
        }
    }

    private IEnumerator BeginFragmentation(GameObject player)
    {
        if (isQuestObject)
        {
            GameManager.instance.NextQuest();
        }
        AudioManager.instance.PlaySound(AudioClips.WindowSmash);
        GetComponent<BoxCollider>().enabled = false;
        solidObject.SetActive(false);
        fracturedObject.SetActive(true);

        yield return new WaitForSeconds(disappearTimer);
        Destroy(transform.parent.gameObject);
    }
}
