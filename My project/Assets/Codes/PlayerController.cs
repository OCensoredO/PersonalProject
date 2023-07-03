using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public GameObject bulletPrefab;
    public float bulletSpeed;

    public void Move(Vector3 direction)
    {
        transform.position += direction * playerSpeed * Time.deltaTime;
        transform.LookAt(transform.position + direction);
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position + new Vector3(0.0f, 0.0f, 2.0f), gameObject.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
        Destroy(bullet, 2.0f);
    }
}
