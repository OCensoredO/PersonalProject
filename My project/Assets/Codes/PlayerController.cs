using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerSpeed = 8;
    public GameObject bulletPrefab;
    public int bulletSpeed = 1000;
    public int jumpForce = 300;
    private bool isInAir;

    private void Start()
    {
        isInAir = false;
    }

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

    public void Jump()
    {
        if (isInAir) return;

        Rigidbody rd = GetComponent<Rigidbody>();
        isInAir = true;
        rd.AddForce(Vector3.up * jumpForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isInAir = false;
        }
    }
}
