using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterController : NetworkBehaviour
{
    public Rigidbody rb;

    public Transform head;

    public GameObject gun;

    public GameObject camera;

    public float health = 100f;

    public float moveSpeed = 100f, strafeSpeed = 100f, jumpSpeed = 10f;

    public float sprintModifier = 1.3f;

    public float gunLerpSpeed;

    public float mouseXSpeed, mouseYSpeed;

    public bool isGrounded;

    private float mouseX, mouseY;
    float forward;
    float sideways;
    float jump;
    bool sprint;
    bool crouch;

    bool locked;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (isLocalPlayer)
        {
            camera.SetActive(true);
        }
        else
        {
            gameObject.layer = 0;
        }

        Cursor.lockState = CursorLockMode.Locked;
        locked = true;
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
        if (!isLocalPlayer)
        {
            return;
        }
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
        if (!Input.GetMouseButton(1))
        {
            int mask = (1 << 8);
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {
                Debug.Log("Hit: " + hit.transform.name + " " + hit.point);
                if (!hit.collider.gameObject.GetComponent<Bullet>())
                {
                    Vector3 relativePos = hit.point - gun.transform.position;
                    Quaternion toRotation = Quaternion.LookRotation(relativePos);
                    gun.transform.rotation = Quaternion.Lerp(gun.transform.rotation, toRotation, gunLerpSpeed * Time.deltaTime);
                    //gun.transform.LookAt(hit.point);
                }
            }
            else
            {
                gun.transform.localRotation = Quaternion.Lerp(gun.transform.localRotation, Quaternion.Euler(new Vector3(-0.5f, -1.67f, 0f)), gunLerpSpeed * Time.deltaTime);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            locked = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            locked = true;
        }

        Cursor.lockState = (locked) ? CursorLockMode.Locked : CursorLockMode.None;

        head.transform.localPosition = Vector3.Lerp(head.transform.localPosition,  new Vector3(0f, (!crouch) ? 1.094f : .55f, 0f), .4f);
        


    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }


        //if (isGrounded)
        {
            //Debug.Log(rb.velocity.z);
            var temp = 1f;
            if (rb.velocity.magnitude < 0.02f)
            {
                temp = 2f;
            }

            //rb.AddForce(transform.forward * temp *(1 - (rb.velocity.magnitude/12f)) * forward * (moveSpeed * ((sprint && transform.InverseTransformDirection(rb.velocity).z > 7f) ? (1 + Mathf.Log(sprintModifier)) : 1f)) + transform.right * sideways * strafeSpeed);
            rb.MovePosition(transform.position + (transform.forward * forward * ((sprint) ? (1 + Mathf.Log(sprintModifier)) : 1f)) * moveSpeed + (transform.right * sideways) * strafeSpeed);
        }
    }
}
