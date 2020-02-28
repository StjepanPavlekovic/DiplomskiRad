using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookScript : MonoBehaviour
{
    public float interactionRange = 2.25f;
    public float rotationSpeed = 100f;
    public float clampAngle = 75f;

    float xRotation = 0f;

    public Transform playerBody;
    [SerializeField]
    private float interactionCheckFrequency = 0.1f;
    private List<Interactable> interactableElements;
    private bool lookingAtInteractable = false;
    [SerializeField]
    private LayerMask ignoreOnRaycast;

    void Start()
    {
        interactableElements = new List<Interactable>();
        StartCoroutine(CheckForInteractableObjects());
    }

    IEnumerator CheckForInteractableObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(interactionCheckFrequency);

            interactableElements.Clear();
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, ignoreOnRaycast))
            {
                MonoBehaviour[] monos = hit.collider.gameObject.GetComponents<MonoBehaviour>();

                foreach (MonoBehaviour mb in monos)
                {
                    if (mb is Interactable)
                    {
                        if (mb.enabled)
                        {
                            interactableElements.Add((Interactable)mb);
                        }
                    }
                }
            }
            if (interactableElements.Count > 0)
            {
                lookingAtInteractable = true;
            }
            else
            {
                lookingAtInteractable = false;
            }
            UIManager.instance.HandleCroshairs(lookingAtInteractable);
        }
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isDead && !GameManager.instance.gamePaused)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -clampAngle, clampAngle);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    private void Update()
    {
        if (GameManager.instance.game == Game.Explorer)
        {
            if (Input.GetMouseButtonDown(0) && !GameManager.instance.gamePaused)
            {
                CheckForInteraction();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E) && !GameManager.instance.gamePaused)
            {
                CheckForInteraction();
            }
        }
    }

    private void CheckForInteraction()
    {
        foreach (var item in interactableElements)
        {
            item.Interact();
            GameManager.instance.interactionsCount++;
        }
    }
}
