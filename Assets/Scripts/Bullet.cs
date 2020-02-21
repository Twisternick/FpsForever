using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public Collider collide;

    public LayerMask layerMask = -1;

    float timer;
    Vector3 previousPosition;

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;


    private void Start()
    {
        collide = GetComponent<Collider>();

        previousPosition = rb.position;
        
        minimumExtent = Mathf.Min(Mathf.Min(collide.bounds.extents.x, collide.bounds.extents.y), collide.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - 0.1f);
        sqrMinimumExtent = minimumExtent * minimumExtent;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 8)
        {
            if (collision.gameObject.GetComponent<Health>())
            {
                collision.gameObject.GetComponent<Health>().Hit();
            }
            //Debug.Log("Hit " + collision.gameObject.name);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= 3f)
        {
            Destroy(gameObject);
        }

        DontGoThrough();
    }


    public void DontGoThrough()
    {
        Vector3 movementThisStep = rb.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit hitInfo;

            //Debug.Log(layerMask.value);

            //check for obstructions we might have missed 
            if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
            {
                if (!hitInfo.collider)
                    return;

                if (hitInfo.collider.isTrigger)
                    hitInfo.collider.SendMessage("OnTriggerEnter", collide);

                if (!hitInfo.collider.isTrigger)
                    rb.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent;

            }
        }

        previousPosition = rb.position;
    }


}
