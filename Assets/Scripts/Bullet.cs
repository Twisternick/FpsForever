using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public Collider collide;

    public VisualEffect bulletTrail;

    public GameObject parentObject;

    public float rotationCurveHorizontal = 0f;
    public float rotationCurveVertical = 0f;
    public float rotationSpeed = 1f;


    public LayerMask layerMask = -1;

    float timer;
    Vector3 previousPosition;

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;

    public Vector3 random;
    public Vector3 playerPosition;
    public Vector3 startRight;
    public Vector3 startUp;


    private void Start()
    {
        collide = GetComponent<Collider>();

        previousPosition = rb.position;
        
        minimumExtent = Mathf.Min(Mathf.Min(collide.bounds.extents.x, collide.bounds.extents.y), collide.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - 0.1f);
        sqrMinimumExtent = minimumExtent * minimumExtent;

        bulletTrail.SetVector3("Bullet Position", transform.position);

        playerPosition = parentObject.transform.right;

        random = Random.insideUnitSphere;

        startRight = transform.right;
        startUp = transform.forward;
        //Debug.Log(random);

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 8 && collision.gameObject != parentObject)
        {
            if (collision.gameObject.GetComponent<Health>())
            {
                collision.gameObject.GetComponent<Health>().Hit();
            }
            //Debug.Log("Hit " + collision.gameObject.name);
            Destroy(gameObject);
        }
    }

    private void Update()
    {        
        bulletTrail.SetVector3("Bullet Position", transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        //Debug.Log(rb.velocity.magnitude);
        timer += Time.deltaTime;

        if (timer >= 3f)
        {
            //Destroy(gameObject);
        }


        // (new Vector3(.6f, .1f, .2f) is a good drop and feel but its not consistent to where the player is facing
        // Applying a force is making the bullets curve up???
        rb.AddForce((startRight * rotationSpeed * rotationCurveHorizontal) * Time.deltaTime);
        rb.AddForce((startUp * rotationSpeed * rotationCurveVertical) * Time.deltaTime);

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
