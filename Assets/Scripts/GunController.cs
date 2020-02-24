using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Networking;

public class GunController : MonoBehaviour
{
    public GameObject bullet;

    public GameObject bulletSpawn;

    public GameObject lookAtTarget;

    public GameObject character;
    public GameObject head;

    public VisualEffect muzzleFlash;


    public bool moveGun;


    public float shootVelocity = 100f;
    public float shootAngle = 10f;
    public float gunWhipLerpSpeed = .1f;
    public float gunWhipSpeed;

    public Vector3 oldPosition;

    public Vector3 axis = Vector3.up;

    public Vector3 startPosition;
    public Vector3 desiredPosition;

    public Vector3 lastForward;

    public float radius = 2f;

    public float timer;

    public float gunOffset;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        if (!character.GetComponent<CharacterController>().isLocalPlayer)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        moveGun = Input.GetMouseButton(1);
        lastForward = head.transform.forward;
        //transform.LookAt(lookAtTarget.transform);
        
    }

    /*
    private void FixedUpdate()
    {
        if (moveGun)
        {
            timer += Time.deltaTime;
            //transform.RotateAround(character.transform.position, character.transform.up, gunWhipSpeed * Time.deltaTime);

            Debug.Log(-Input.GetAxis("Mouse X") * gunOffset);
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(
                (Input.GetAxis("Mouse X") * gunWhipSpeed + (Input.GetAxis("Mouse X") == 0f ? 0 : Mathf.Clamp(-Mathf.Sign(Input.GetAxis("Mouse X")), -0.5f, 0.5f))),
                transform.localPosition.y,
                transform.localPosition.z), gunWhipLerpSpeed * Time.deltaTime);
            //Debug.Log(new Vector3(character.transform.position.x + Mathf.Sin(timer) * radius, character.transform.position.y, character.transform.position.z + Mathf.Cos(timer) * radius));
            //transform.position = new Vector3(character.transform.position.x + Mathf.Sin(timer) * radius, character.transform.position.y, character.transform.position.z + Mathf.Cos(timer) * radius);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, gunWhipSpeed);
        }
        oldPosition = transform.localPosition;

    }*/


    public void Shoot()
    {
        var tempBullet = Instantiate(bullet);
        tempBullet.transform.position = bulletSpawn.transform.position;
        tempBullet.transform.rotation = bullet.transform.rotation;
        tempBullet.GetComponent<Bullet>().parentObject = bullet.transform.parent.gameObject;
        
        tempBullet.GetComponent<Bullet>().rotationCurveHorizontal = Vector3.SignedAngle(Vector3.ProjectOnPlane(lastForward, head.transform.up), head.transform.forward, Vector3.up) /** 100f*/;
        tempBullet.GetComponent<Bullet>().rotationCurveVertical = Vector3.SignedAngle(Vector3.ProjectOnPlane(lastForward, head.transform.right), head.transform.forward, head.transform.right) /** 100f*/;
        //Debug.Log(tempBullet.GetComponent<Bullet>().rotationCurve);
        tempBullet.SetActive(true);
        muzzleFlash.Play();

        tempBullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.transform.forward * shootVelocity);
        NetworkServer.Spawn(tempBullet);
    }
}
