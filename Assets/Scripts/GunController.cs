using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bullet;

    public GameObject bulletSpawn;

    public GameObject lookAtTarget;

    public float shootVelocity = 100f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        //transform.LookAt(lookAtTarget.transform);
    }


    public void Shoot()
    {
        var tempBullet = Instantiate(bullet);
        tempBullet.transform.position = bulletSpawn.transform.position;
        tempBullet.transform.rotation = bullet.transform.rotation;
        tempBullet.SetActive(true);

        tempBullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.transform.forward * shootVelocity);

    }
}
