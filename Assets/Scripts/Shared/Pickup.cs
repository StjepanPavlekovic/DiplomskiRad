using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Transform pick;
    [SerializeField]
    private bool held;

    Vector3 previousPosition;
    Vector3 deltaPos;
    Rigidbody rb;

    private void Start()
    {
        pick = GameObject.Find("Pickup").transform;

        held = false;

        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, pick.position) <= 2.25f)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;

            this.transform.position = pick.position;
            this.transform.parent = pick;
            held = true;
        }
    }

    private void Update()
    {
        if (held)
        {
            deltaPos = transform.position - previousPosition;
            if (Input.GetMouseButtonDown(1))
            {
                held = false;
                Debug.Log("Shoot?");
                this.transform.parent = null;
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.None;
                rb.AddForce(pick.transform.forward * 800f);
            }
            previousPosition = transform.position;
        }
    }

    void OnMouseUp()
    {
        if (held)
        {
            Vector3 throwVector = deltaPos;
            float throwSpeed = Mathf.Clamp((throwVector.magnitude / Time.deltaTime), 0, 45f);
            Vector3 throwVelocity = (throwSpeed / 3) * throwVector.normalized;
            Debug.Log(throwSpeed);
            rb.isKinematic = false;
            this.transform.parent = null;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            held = false;
            rb.velocity = throwVelocity;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        held = false;
        this.transform.parent = null;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
    }
}
