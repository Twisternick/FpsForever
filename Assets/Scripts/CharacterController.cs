using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Rigidbody rb;

    public Transform head;

    public float health = 100f;

    public float moveSpeed = 100f, strafeSpeed = 100f, jumpSpeed = 10f;

    public float sprintModifier = 1.3f;

    public float mouseXSpeed, mouseYSpeed;

    public bool isGrounded;

    private float mouseX, mouseY;
    float forward;
    float sideways;
    float jump;
    bool sprint;
    bool crouch;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }


    private void Update()
    {

        mouseX += Input.GetAxis("Mouse X") * mouseXSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * mouseYSpeed;

        head.transform.localRotation = Quaternion.Slerp(head.transform.localRotation, Quaternion.Euler(new Vector3(Mathf.Clamp(mouseY, -90f, 90f), 0f, 0f)), .99f);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(new Vector3(0f, mouseX, 0f)), .99f);

        forward = Input.GetAxis("Vertical");
        sideways = Input.GetAxis("Horizontal");
        sprint = Input.GetKey(KeyCode.LeftShift);
        crouch = Input.GetKey(KeyCode.C);

        if (isGrounded)
        {
            jump = (Input.GetKeyDown(KeyCode.Space)) ? 1f : 0f;

            rb.AddForce(transform.up * jump * jumpSpeed, ForceMode.Impulse);
        }


        head.transform.localPosition = Vector3.Lerp(head.transform.localPosition,  new Vector3(0f, (!crouch) ? 1.094f : .55f, 0f), .4f);
        


    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;


        //Debug.Log(rb.velocity.z);
        rb.AddForce(transform.forward * forward * (moveSpeed * ((sprint && transform.InverseTransformDirection(rb.velocity).z > 7f) ? (1 + Mathf.Log(sprintModifier)) : 1f)) + transform.right * sideways * strafeSpeed);
    }
}
